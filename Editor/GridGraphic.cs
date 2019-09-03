using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyGrid
{
    public class GridGraphic
    {
        private Texture _texture;
        
        public bool IsActive { get; set; }
    
        public bool IsInteractable
        {
            get => _isInteractable && IsActive;
        
            set { _isInteractable = true; }
        }

        public GridEventTrigger EventTrigger { get; private set; }

        public bool CanDrag
        {
            get => IsInteractable && _isDraggable;
            set => _isDraggable = value;
        }

        public GridTransform Transform { get; private set; }

        //public CellBounds Bounds => Transform.LocalBounds;
        
        private bool _isInteractable;

        private bool _isDraggable;

        private Cell _dragStartOffset;
        
        
        
        /// <summary>
        /// Width and Height are in cells
        /// </summary>
        public GridGraphic(Cell position, int width, int height)
        {
            Transform = new GridTransform(new CellBounds(position, width, height));

            IsActive = true;

            Hierarchy.AddGraphic(this);
        
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
        
            _dragStartOffset = Transform.ZoomedBounds.TopLeft - screenPointCell;
            
        }
    
        private void Drag()
        {
            if(!_isDraggable)
                return;
        
            var desiredPosition = Cell.FromScreenPoint(EditorGrid.MouseRelativeToActiveEditor) + _dragStartOffset;
        
            Transform.SetGlobalPosition(desiredPosition);
            
            //Transform.LocalBounds.TopLeft = desiredPosition;
        
        }

        /// <summary>
        /// Use this inside the Drag function if you don't want the graphic to overlap with other graphics
        /// </summary>
        /// <param name="positionToValidate"></param>
        /// <returns></returns>
        private bool ValidateDragPosition(Cell positionToValidate)
        {
            return Hierarchy.GetGraphicsInArea(new CellBounds(positionToValidate, Transform.Width, Transform.Height)).Count(i => i != this) == 0;
        }
        
        #endregion
    }
}