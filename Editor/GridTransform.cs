using System;
using Boo.Lang;
using UnityEngine;

namespace EasyGrid
{
    public class GridTransform
    {
        public GridTransform(CellBounds bounds)
        {
            LocalPosition = bounds.TopLeft;
            Width = bounds.Width;
            Height = bounds.Height;
            //LocalBounds = bounds;
        }
        
        public GridTransform Parent
        {
            get => _parent;
            set
            {
                //Remove this from the current parent children
                _parent?.Children.Remove(this);

                _parent = value;
                
                //Add this to the new parent children
                _parent?.Children.Add(this);
                
                RecalculateLocalOffset();
            }
        }

        private GridTransform _parent;
        
        public List<GridTransform> Children => _children; 

        private List<GridTransform> _children = new List<GridTransform>();
        
        private GridAnchor _anchor;

        public Cell LocalPosition { get; set; }

        public Cell LocalOffset { get; private set; }

        public GridAnchor Anchor
        {
            get => _anchor;
            set
            {
                _anchor = value;
                RecalculateLocalOffset();
            }
        }

        /// <summary>
        /// The Position of this Entity in ViewPort Space
        /// </summary>
        public Cell Position
        {
            get
            {
                var position = LocalPosition;

                if(_parent != null)
                    position += _parent.Position;
                
                return position + LocalOffset;
            }
        }

        
        public CellBounds Bounds
        {
            get
            {
                return new CellBounds(Position, Width, Height);
            }
        }
        
        public CellBounds ZoomedBounds
        {
            get
            {
                return new CellBounds(Position, (int)(Width * EditorGrid.ZoomedSpacing), (int)(Height * EditorGrid.ZoomedSpacing));
            }
        }

        /// <summary>
        /// In Cells
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// In Cells
        /// </summary>
        public int Height { get; set; }
        
        public Rect GetRectInViewport()
        {
            return EditorGrid.GetRectInViewport(new Vector2(Position.GetPosition().x, Position.GetPosition().y), Width * EditorGrid.ZoomedSpacing, Height * EditorGrid.ZoomedSpacing);
        }

        /// <summary>
        /// Anchors the local offset to the active GridAnchor
        /// </summary>
        private void RecalculateLocalOffset()
        {
            if (Parent == null)
            {
                LocalOffset = new Cell(0, 0);
                return;
            }
            
            var parentWidth = (int)(Parent.ZoomedBounds.Width / EditorGrid.LineSpacing);
            var desiredWidth = (int)(ZoomedBounds.Width / EditorGrid.LineSpacing);
            
            var parentHeight = (int)(Parent.ZoomedBounds.Height/ EditorGrid.LineSpacing);
            var desiredHeight = (int)(ZoomedBounds.Height / EditorGrid.LineSpacing);//(int) (_style.CalcSize(new GUIContent(Content)).y / EditorGrid.ZoomedSpacing);

            int column = 0, line = 0;
            
            switch (_anchor)
            {
                case GridAnchor.UpperLeft:
                case GridAnchor.MiddleLeft:
                case GridAnchor.LowerLeft:
                    column = 0;
                    break;
                
                case GridAnchor.MiddleCenter:
                case GridAnchor.LowerCenter:
                case GridAnchor.UpperCenter:
                {
                    column = parentWidth / 2 - desiredWidth / 2;
                    break;
                }

                case GridAnchor.UpperRight:
                case GridAnchor.MiddleRight:
                case GridAnchor.LowerRight:
                {
                    column = parentWidth - desiredWidth;
                    break;
                }
            }
            
            switch (_anchor)
            {
                
                case GridAnchor.UpperLeft:
                case GridAnchor.UpperCenter:
                case GridAnchor.UpperRight:
                    line = 0;
                    break;
                
                case GridAnchor.MiddleCenter:
                case GridAnchor.MiddleLeft:
                case GridAnchor.MiddleRight:
                {
                    line = parentHeight / 2 - desiredHeight / 2;
                    break;
                }
                
                case GridAnchor.LowerLeft:
                case GridAnchor.LowerCenter:
                case GridAnchor.LowerRight:
                {
                    line = parentHeight - desiredHeight;
                    break;
                }
            }
            
            LocalOffset = new Cell(column, line);
        }

        private Cell ParentPosition => Parent?.Position ?? new Cell(0, 0);

        /// <summary>
        /// Sets the local position in a way that the viewport position of this object is the desired position
        /// </summary>
        /// <param name="desiredPosition"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetGlobalPosition(Cell desiredPosition)
        {
            LocalPosition = desiredPosition - (ParentPosition + LocalOffset);
        }
    }
}