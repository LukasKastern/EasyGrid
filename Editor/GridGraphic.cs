using System.Linq;
using UnityEngine;

namespace EasyGrid
{
    public class GridGraphic
    {
        private Texture _texture;

        public CellBounds Bounds;

        public bool IsActive { get; set; }
    
        public bool IsInteractable
        {
            get => _isInteractable && IsActive;
        
            set { _isInteractable = true; }
        }

        private bool _isInteractable;

        public bool CanDrag
        {
            get => IsInteractable && _isDraggable;
            set => _isDraggable = value;
        }

        private bool _isDraggable;

        private Cell _dragStartOffset;
    
        public GridEventTrigger EventTrigger { get; private set; }

        /// <summary>
        /// Width and Height are in cells
        /// </summary>
        public GridGraphic(Cell position, int width, int height)
        {
            IsActive = true;

            Hierarchy.AddGraphic(this);
            Bounds = new CellBounds(position, width, height);
        
            EventTrigger = new GridEventTrigger();
            EventTrigger.AddEventTrigger(TriggerEvent.StartedDrag, StartedDrag);
            EventTrigger.AddEventTrigger(TriggerEvent.Drag, Drag);
        }
    
      
        public virtual void Draw(float currentZoom)
        {
        }

        /// <summary>
        /// This will only remove the graphic from the Hierarchy, if you still have references to this object it will still be alive.
        /// </summary>
        public void Destroy()
        {
            Hierarchy.RemoveGraphic(this);
        }
    
        #region DragFunctions
    
        private void StartedDrag()
        {
            if(!_isDraggable)
                return;
        
            var screenPointCell = Cell.FromScreenPoint(EditorGrid.MouseRelativeToActiveEditor);
        
            _dragStartOffset = Bounds.TopLeft - screenPointCell;
            
        }
    
        private void Drag()
        {
            if(!_isDraggable)
                return;
        
            var desiredPosition = Cell.FromScreenPoint(EditorGrid.MouseRelativeToActiveEditor) + _dragStartOffset;
        
            Bounds.TopLeft = desiredPosition;
        
        }

        /// <summary>
        /// Use this inside the Drag function if you don't want the graphic to overlap with other graphics
        /// </summary>
        /// <param name="positionToValidate"></param>
        /// <returns></returns>
        private bool ValidateDragPosition(Cell positionToValidate)
        {
            return Hierarchy.GetGraphicsInArea(new CellBounds(positionToValidate, Bounds.Width, Bounds.Height)).Count(i => i != this) == 0;
        }
        
        #endregion
    }
}