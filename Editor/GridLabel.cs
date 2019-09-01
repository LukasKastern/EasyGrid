using UnityEngine;

namespace EasyGrid
{
    public class GridLabel : GridGraphic
    {
        public string Content { get; set; }
    
        private Font _font;

        private GUIStyle _style;

        private int _fontSize;
    
        public GridLabel(Cell position) : base(position, 1, 1)
        {
            _style = new GUIStyle();
        
            _style.wordWrap = false;
            _style.clipping = TextClipping.Clip;
        
            SetDefaultFont();
            SetFontSize(5);
        }

        /// <summary>
        /// Font size is defined in cells
        /// </summary>
        /// <param name="size"></param>
        public void SetFontSize(int size)
        {
            _fontSize = size;
        }

        private void SetDefaultFont()
        {
            _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        /// <summary>
        /// Sets a custom Font with which this Label should be renderer
        /// If null is given as the parameter the default font is used
        /// </summary>
        /// <param name="font"></param>
        public void SetFont(Font font)
        {
            if(font == null)
                SetDefaultFont();
        
            else
                _style.font = font;
        }

        /// <summary>
        /// We have to align the text based on it's direction to the center of the viewport so it gets cutoff correctly
        /// </summary>
        private void SetTextAlignment(Vector2 gridPosition)
        {        
            var desiredWidth = _style.CalcSize(new GUIContent(Content)).x;
            var desiredHeight = _style.CalcSize(new GUIContent(Content)).y;
        
            var center = new Vector2(gridPosition.x + desiredWidth, gridPosition.y + desiredHeight);

            var gridCenter = EditorGrid.GridPosition + new Vector2(EditorGrid.GridWidth / 2, +EditorGrid.GridHeight / 2);

            _style.alignment = EditorGridUtility.DirectionToAlignment(gridCenter - center);;
        }
    
        public override void Draw(float currentZoom)
        {
            var gridPosition = Bounds.TopLeft.GetPosition();

            _style.fontSize = _fontSize * (int)EditorGrid.ZoomedSpacing;

            var desiredWidth = _style.CalcSize(new GUIContent(Content)).x;
            var desiredHeight = _style.CalcSize(new GUIContent(Content)).y;

            Bounds.Width = (int)(desiredWidth / EditorGrid.ZoomedSpacing);
            Bounds.Height = (int)(desiredHeight / EditorGrid.ZoomedSpacing);

            var rectInViewport = EditorGrid.GetRectInViewport(new Vector2(gridPosition.x, gridPosition.y),
                desiredWidth, desiredHeight);
        
            SetTextAlignment(gridPosition);

            GUI.Label(rectInViewport, Content, _style);    
        }
    }
}