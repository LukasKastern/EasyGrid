using System;
using UnityEngine;

namespace EasyGrid
{
    public class GridLabel : GridGraphic
    {
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
 
                if(_guiContent == null)
                    _guiContent = new GUIContent();
                
                _guiContent.text = _content;
                
                OnContentSizeChanged();
            }
        }

        public TextAnchor AnchorToParent;
        
        private Font _font;

        private GUIStyle _style;

        private int _fontSize;

        private bool _hasAnchor = false;
        private string _content;

        private GUIContent _guiContent = new GUIContent();

        public GridLabel(Cell position) : base(position, 1, 1)
        {
            _style = new GUIStyle();
        
            _style.wordWrap = false;
            _style.clipping = TextClipping.Clip;

            _content = "";
            
            SetDefaultFont();
            SetFontSize(1);
        }
        
        /// <summary>
        /// Font size is defined in cells
        /// </summary>
        /// <param name="size"></param>
        public void SetFontSize(int size)
        {
            _fontSize = size;
            _style.fontSize = _fontSize * (int) EditorGrid.ZoomedSpacing;
            
            OnContentSizeChanged();

        }

        private void SetDefaultFont()
        {
            _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        
        /// <summary>
        /// Changes the Width and height of the Transform when the font or the Content has changed
        /// </summary>
        private void OnContentSizeChanged()
        {
            Transform.Width = (int)(_style.CalcSize(new GUIContent(Content)).x / EditorGrid.LineSpacing);
            Transform.Height = (int)(_style.CalcSize(new GUIContent(Content)).y / EditorGrid.LineSpacing);
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
            var desiredWidth = _style.CalcSize(_guiContent).x;
            var desiredHeight = _style.CalcSize(_guiContent).y;
        
            var center = new Vector2(gridPosition.x + desiredWidth, gridPosition.y + desiredHeight);

            var gridCenter = EditorGrid.GridPosition + new Vector2(EditorGrid.GridWidth / 2, +EditorGrid.GridHeight / 2);

            _style.alignment = EditorGridUtility.DirectionToAlignment(gridCenter - center);;
        }
    
        public override void Draw(float currentZoom)
        {
            var gridPosition = Transform.Position.GetPosition();

            _style.fontSize = _fontSize * (int)EditorGrid.ZoomedSpacing;

            var rectInViewport = Transform.GetRectInViewport();
        
            //Make sure that the text gets cutoff correctly 
            SetTextAlignment(gridPosition);

            GUI.Label(rectInViewport, Content, _style);    
        }
    }
}