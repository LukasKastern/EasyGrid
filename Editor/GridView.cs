using UnityEngine;

namespace EasyGrid
{
    public class GridView
    {
        public const float ViewPortUnit = 20f;

        public Vector2 CurrentViewPortCenter { get; private set; }

        private float _currentHorizontalZoomLine = 0;
        private float _currentVerticalZoomLine = 0;
    
        public GridView()
        {
            _currentHorizontalZoomLine = EditorGrid.GridWidth / EditorGrid.LineSpacing;
            _currentVerticalZoomLine = EditorGrid.GridHeight / EditorGrid.LineSpacing;
        }
    
        public float ZoomFactor
        {
            get => _zoomFactor;

            set
            {
                var clampedValue = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);

                var horizontalLinesWithCurrentZoom = EditorGrid.GridWidth / (EditorGrid.LineSpacing * clampedValue);

                var verticalLineWithCurrentZoom = EditorGrid.GridHeight / (EditorGrid.LineSpacing * clampedValue);

                var horizontalLinesToMove = (_currentHorizontalZoomLine - horizontalLinesWithCurrentZoom);
                var verticalLineToMove = (_currentVerticalZoomLine - verticalLineWithCurrentZoom);

                var xToMove = horizontalLinesToMove * EditorGrid.LineSpacing;
                var yToMove = verticalLineToMove * EditorGrid.LineSpacing;
            
                CurrentViewPortCenter += new Vector2(-xToMove / 2, -yToMove / 2);
            
                _zoomFactor = clampedValue;

                _currentHorizontalZoomLine = horizontalLinesWithCurrentZoom;
                _currentVerticalZoomLine = verticalLineWithCurrentZoom;
            }
        }

        private const float MinZoomFactor = 0.5f;
        private const float MaxZoomFactor = 1.5f;

        private const float ScrollWheelSpeed = 0.1f;

        private float _zoomFactor;

        private void AddZoom(float zoom)
        {
            ZoomFactor += zoom;
        }

        private void MoveViewPort(Vector2 amount) => CurrentViewPortCenter += amount * (2 - _zoomFactor);
    
        /// <summary>
        /// Resets the viewport and the ViewPortCenter
        /// </summary>
        public void Reset()
        {
            ZoomFactor = 1f;
            CurrentViewPortCenter = new Vector2(EditorGrid.GridWidth / 2 , EditorGrid.GridHeight / 2);
        }

        public void ProcessInput()
        {
            if (UnityEngine.Event.current == null)
            {
                return;
            }

            switch (UnityEngine.Event.current.type)
            {
                case EventType.MouseDown:
                    break;
                case EventType.ScrollWheel:
                    UpdateZoom();
                    break;
                case EventType.MouseDrag:
                    UpdateDrag();
                    break;
                default:
                    if (UnityEngine.Event.current.keyCode == KeyCode.C)
                        Reset();
                    break;
            
            }
        }

        private void UpdateZoom()
        {
            AddZoom(-UnityEngine.Event.current.delta.y * ScrollWheelSpeed);
        
        }

        private void UpdateDrag()
        {
            MoveViewPort(UnityEngine.Event.current.delta);
        }
    }
}