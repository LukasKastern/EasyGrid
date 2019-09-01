using System;
using UnityEngine;

namespace EasyGrid
{
    public struct Cell
    {
        public int Line;
        public int Column;

        public Cell(int column, int line)
        {
            Column = column;
            Line = line;
        }

        /// <summary>
        /// The position is always the top left corner
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return new Vector2(Column * EditorGrid.ZoomedSpacing + (int)Math.Truncate( (double) (EditorGrid.ActiveView.CurrentViewPortCenter.x
                                                                                                 * EditorGrid.ActiveView.ZoomFactor)), Line * EditorGrid.ZoomedSpacing + (int)Math.Truncate((double) (EditorGrid.ActiveView.CurrentViewPortCenter.y* EditorGrid.ActiveView.ZoomFactor))) + EditorGrid.GridPosition;
        }

        public static Cell FromScreenPoint(Vector2 screenPoint)
        {
            var line = (int)Math.Truncate((screenPoint.y - EditorGrid.GridPosition.y - EditorGrid.ActiveView.CurrentViewPortCenter.y * EditorGrid.ActiveView.ZoomFactor) / EditorGrid.ZoomedSpacing);
            var column = (int)Math.Truncate((screenPoint.x - EditorGrid.GridPosition.x - EditorGrid.ActiveView.CurrentViewPortCenter.x * EditorGrid.ActiveView.ZoomFactor) / EditorGrid.ZoomedSpacing);
        
            // line = Math.Truncate(screnpoint - ViewPort) / ZoomedSpacing | * zoomedSpacing
            // line * zoomedSpacing = Math.Trunctate (ScreenPoint -Viewport) | + CurrentViewport
            // Line * zoomedSpacing + CurrentViewPort = ScreenPoint
            // 
        
            return new Cell(column, line);
        }
    
        public static Cell operator +(Cell a, Cell b)
        {
            return new Cell(a.Column + b.Column, a.Line + b.Line);
        }

        public static Cell operator -(Cell a, Cell b)
        {
            return new Cell(a.Column - b.Column, a.Line - b.Line);
        }

        public override string ToString()
        {
            return $"Column is {Column}, Line is {Line}";
        }
    }
}