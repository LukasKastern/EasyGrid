using System.Linq;
using UnityEngine;

namespace EasyGrid
{
    public static class InputManager
    {
        private static CellBounds _mousePosition = new CellBounds(new Cell(0, 0), 1, 1);

        private static Event e => Event.current;

        private static GridEventTrigger _eventTrigger;

        private static bool _isDragging = false;

        private static bool _prepareToDrag = false;
    
        public static void ProcessInput()
        {
            if(e == null)
                return;
        
            switch (e.type)
            {
                case EventType.Ignore:
                case EventType.Repaint:
                case EventType.Layout:
                case EventType.Used:
                    return;
                case EventType.MouseMove:
                    UpdateMousePosition();
                    break;
            }

            //If the mouse button was held last frame and the used is dragging the mouse we initialize the drag
            if (_prepareToDrag && e.type == EventType.MouseDrag)
            {
                _isDragging = true;
                _prepareToDrag = false;
            
                _eventTrigger.Invoke(TriggerEvent.StartedDrag);
            }
        
            else if (_prepareToDrag)
            {
                _prepareToDrag = false;
            }
        
            if (_isDragging)
            {
                ProcessDrag();
            }
        
            var triggerUnderCursor = !_isDragging ?  GetCurrentTriggerUnderCursor() : _eventTrigger;

            if (_eventTrigger != triggerUnderCursor)
            {
               OnTriggerUnderCursorChanged(triggerUnderCursor);
            }
            
            if (!EditorGrid.IsInViewportEditorSpace(_mousePosition.TopLeft.GetPosition(), _mousePosition.BottomRight.GetPosition()))
                return;
            
            if (_eventTrigger == null || e.type == EventType.ScrollWheel)
            {
                ProcessGridInput();
                return;
            }

            switch (e.type)
            {
                case EventType.DragPerform:
                    _eventTrigger.Invoke(TriggerEvent.Drag);
                    e.Use();

                    break;
                case EventType.DragExited:
                    _eventTrigger.Invoke(TriggerEvent.DragFinished);
                    e.Use();

                    break;
                case EventType.MouseDown:
                    _prepareToDrag = true;
                    _eventTrigger.Invoke(TriggerEvent.OnMouseDown);
                    e.Use();

                    break;
                case EventType.MouseUp:
                    _eventTrigger.Invoke(TriggerEvent.OnMouseUp);
                    e.Use();

                    break;
            } 
        }

        private static void ProcessDrag()
        {
            if (e.type == EventType.MouseLeaveWindow || e.type == EventType.MouseUp)
            {
                _isDragging = false;
                e.type = EventType.DragExited;
            }
        
            else
            {
                e.type = EventType.DragPerform;
            }
        }

        /// <summary>
        /// Performs things like zooming and scrolling through the grid
        /// </summary>
        private static void ProcessGridInput()
        {
            if (UnityEngine.Event.current.type == EventType.MouseDown && UnityEngine.Event.current.alt)
            {
            
                var cell = Cell.FromScreenPoint(EditorGrid.MouseRelativeToActiveEditor);
                new GridTexture(cell, 10, 10, Texture2D.whiteTexture) {IsInteractable = true, CanDrag = true};
            }

            else
                EditorGrid.ActiveView.ProcessInput();

        }

        private static void OnTriggerUnderCursorChanged(GridEventTrigger newTrigger)
        {
            _eventTrigger?.Invoke(TriggerEvent.OnMouseExit);

            newTrigger?.Invoke(TriggerEvent.OnMouseEnter);

            _eventTrigger = newTrigger;
        }

        private static GridEventTrigger GetCurrentTriggerUnderCursor()
        {
            return Hierarchy.GetGraphicsInArea(_mousePosition).FirstOrDefault(i => i.IsInteractable)?.EventTrigger;
        }

        private static void UpdateMousePosition()
        {
            _mousePosition.TopLeft = Cell.FromScreenPoint(e.mousePosition);
        }
    }
    
}