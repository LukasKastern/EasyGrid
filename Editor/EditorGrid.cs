using System;
using System.Linq;
using UnityEditor;
using UnityEditor.StyleSheets;
using UnityEngine;

namespace EasyGrid
{
    public static class EditorGrid
    {
        public static Vector2 MouseRelativeToActiveEditor
        {
            get
            {
                var mousePosition = GUIUtility.GUIToScreenPoint(UnityEngine.Event.current.mousePosition);

                return mousePosition - _editor.position.position - LabelSize * Vector2.up;
            }
        }

        public const float LineSpacing = 10;

        public static GridView ActiveView;

        /// <summary>
        /// Only set this to false if you repaint your editor by yourself.
        /// Otherwise the Editor will repaint in the Update method of the grid
        /// </summary>
        public static bool DoRepaint { get; private set; } = true;

        public static float ZoomedSpacing => LineSpacing * ZoomFactor;

        public static Color BackGroundGridLineColor { get; set; } =
            new Color(Color.grey.r, Color.grey.g, Color.grey.b, 0.4f);

        public static Color MainGridLineColor { get; set; } = Color.black;

        /// <summary>
        /// The color of the GridOutline if its enabled
        /// </summary>
        public static Color GridOutlineColor;

        /// <summary>
        /// Should the grid have an Outline?
        /// </summary>
        public static bool IsOutlineActive;

        public static Color BackgroundColor;
        
        public static double XLineOffset =>
            (ActiveView.CurrentViewPortCenter.x / GridView.ViewPortUnit -
             Math.Truncate(ActiveView.CurrentViewPortCenter.x / GridView.ViewPortUnit)) * LineSpacing * ZoomFactor;

        public static double YLineOffset =>
            (ActiveView.CurrentViewPortCenter.y / GridView.ViewPortUnit -
             Math.Truncate(ActiveView.CurrentViewPortCenter.y / GridView.ViewPortUnit)) * LineSpacing * ZoomFactor;

        public static float GridWidth
        {
            get => _gridWidth;
            private set => _gridWidth = value;
        }

        public static float GridHeight
        {
            get => _gridHeight;
            private set => _gridHeight = value;
        }


        /// <summary>
        /// This is the position where the grid is going to be drawn,
        /// where 0,0 is the Top Left corner of the Editor Window
        /// </summary>
        public static Vector2 GridPosition;

        public static float HorizontalLines => Mathf.RoundToInt(GridHeight / (LineSpacing * ZoomFactor));
        public static float VerticalLines => Mathf.RoundToInt(GridWidth / (LineSpacing * ZoomFactor));

        private static float ZoomFactor => ActiveView.ZoomFactor;

        private static Texture2D _backgroundTexture;

        /// <summary>
        /// Every x lines comes a line in the main grid color
        /// </summary>
        private const int MainLineEvery = 10;

        private static EditorWindow _editor;

        /// <summary>
        /// This is the size of the name label of the editor window
        /// </summary>
        private const float LabelSize = 25;

        private static ViewPort _viewPort;
        private static float _gridWidth;
        private static float _gridHeight;

        /// <summary>
        /// Initializes the grid with the given Editor
        /// </summary>
        /// <param name="editor"></param>
        public static void Initialize(EditorWindow editor)
        {
            _editor = editor;
            _editor.wantsMouseMove = true;
            _editor.wantsMouseEnterLeaveWindow = true;

            ActiveView = new GridView();
            
            _backgroundTexture = new Texture2D(1, 1);
            _backgroundTexture.wrapMode = TextureWrapMode.Repeat;
            _backgroundTexture.SetPixel(0, 0, BackgroundColor);
            _backgroundTexture.Apply();
        }

        /// <summary>
        /// Draws the grid in the view port.
        /// </summary>
        private static void DrawGrid()
        {
            DrawBackground();

            var lineSpacing = LineSpacing * ZoomFactor;

            var relativeLineX = Math.Truncate(ActiveView.CurrentViewPortCenter.x) / GridView.ViewPortUnit;
            var relativeLineY = Math.Truncate(ActiveView.CurrentViewPortCenter.y) / GridView.ViewPortUnit;

            var relativeXLineOffset = XLineOffset;
            var relativeYLineOffset = YLineOffset;

            for (int horizontalLine = 0; horizontalLine <= HorizontalLines; ++horizontalLine)
            {
                var yOffset = horizontalLine * lineSpacing + (float) relativeYLineOffset;

                Color color = (horizontalLine - (int) relativeLineY) % MainLineEvery == 0
                    ? MainGridLineColor
                    : BackGroundGridLineColor;


                DrawLine(new Vector2(0, yOffset), new Vector2(GridWidth, yOffset), color);
            }

            for (int verticalLine = 0; verticalLine <= VerticalLines; ++verticalLine)
            {
                var xOffset = verticalLine * lineSpacing + (float) relativeXLineOffset;

                Color color = (verticalLine - (int) relativeLineX) % MainLineEvery == 0
                    ? MainGridLineColor
                    : BackGroundGridLineColor;

                DrawLine(new Vector2(xOffset, 0), new Vector2(xOffset, GridHeight), color);
            }

            if (IsOutlineActive)
                DrawGridOutline();
            
            
        }

        private static void DrawBackground()
        {
            var style = new GUIStyle();
            style.normal.background = _backgroundTexture;

            var viewPortRect = new Rect(GridPosition.x, GridPosition.y, _gridWidth, _gridHeight);

            GUI.Box(viewPortRect, GUIContent.none, style);
        }

        /// <summary>
        /// Calls the draw function on all Graphics which are active and registered in the Hierarchy
        /// </summary>
        private static void DrawGraphics()
        {
            foreach (var graphic in Hierarchy.GetGraphics().Where(i => i.IsActive))
            {
                graphic.Draw(ZoomFactor);
            }
        }


        /// <summary>
        /// Returns true if the object intersects with the viewport,
        /// </summary>
        /// <returns></returns>
        public static bool IsInViewport(Vector2 topLeft, Vector2 bottomRight)
        {
            var topleftViewportPoint = new Vector2(0, 0);
            var bottomRightViewportPoint = new Vector2(GridWidth, GridHeight);
            
            if (topLeft.x >= bottomRightViewportPoint.x || topleftViewportPoint.x >= bottomRight.x)
                return false;

            if (topLeft.y >= bottomRightViewportPoint.y || topleftViewportPoint.y >= bottomRight.y)
                return false;

            return true; 
        }

        public static bool IsInViewportEditorSpace(Vector2 topLeft, Vector2 bottomRight)
        {
            var topleftViewportPoint = new Vector2(GridPosition.x, GridPosition.y);
            var bottomRightViewportPoint = new Vector2(GridPosition.x + GridWidth, GridPosition.y + GridHeight);
            
            if (topLeft.x >= bottomRightViewportPoint.x || topleftViewportPoint.x >= bottomRight.x)
                return false;

            if (topLeft.y >= bottomRightViewportPoint.y || topleftViewportPoint.y >= bottomRight.y)
                return false;

            return true; 
        }
        
        /// <summary>
        /// Calculates a rect based upon the given parameters which gets cutoff by the border of the Viewport.
        /// </summary>
        /// <returns></returns>
        public static Rect GetRectInViewport(Vector2 bounds, float width, float height)
        {
            var rectWidth = width;
            var rectHeight = height;

            var x = bounds.x;
            var y = bounds.y;

            if (bounds.x < GridPosition.x)
            {
                var 
                   Right = bounds.x + width;

                //x = GridPosition.x;

                var amountToCutOff = Mathf.Clamp(width - (Right - GridPosition.x), 0, width);

                rectWidth -= amountToCutOff; //Mathf.Clamp(width - (Right - GridPosition.x), 0, width);

                x = bounds.x + amountToCutOff;
            }

            else if ((x + rectWidth) > (GridPosition.x + GridWidth))
            {
                var left = bounds.x + width;

                var cutOffWidth = width - (left - (GridPosition.x + GridWidth));

                rectWidth = Mathf.Clamp(cutOffWidth, 0, width);

                x = GridPosition.x + GridWidth - rectWidth;
            }

            if (bounds.y < GridPosition.y)
            {
                var bottom = bounds.y + height;

                y = GridPosition.y;

                rectHeight -= Mathf.Clamp(height - (bottom - GridPosition.y), 0, height);
            }

            else if (y + rectHeight > GridPosition.y + GridHeight)
            {
                var top = bounds.y + height;

                var cutOffHeight = height - (top - (GridPosition.y + GridHeight));

                rectHeight = Mathf.Clamp(cutOffHeight, 0, height);

                y = GridPosition.y + GridHeight - rectHeight;
            }

            return new Rect(x, y, rectWidth, rectHeight);
        }

        /// <summary>
        /// Draws a line in the given color from start to end.
        /// The positions are relative to the top left corner of the viewport.
        /// </summary>
        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            if (!IsInViewport(start, end))
                return;

            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawLine(start + GridPosition, end + GridPosition) ;
            Handles.EndGUI();
        }

        /// <summary>
        /// Draws a white line from start to end.
        /// The positions are relative to the top left corner of the viewport.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void DrawLine(Vector3 start, Vector3 end)
        {
            DrawLine(start, end, Color.white);
        }

        /// <summary>
        /// Draws a Line in the given color from start to end.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        private static void DrawLineEditorSpace(Vector2 start, Vector2 end, Color color)
        {
            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawLine(start, end);
            Handles.EndGUI();
        }

        /// <summary>
        /// Draws an outline around the grid.
        /// </summary>
        private static void DrawGridOutline()
        {
            DrawLineEditorSpace(new Vector2(GridPosition.x, GridPosition.y),
                new Vector2(GridPosition.x + GridWidth, GridPosition.y), GridOutlineColor);
            DrawLineEditorSpace(new Vector2(GridPosition.x, GridPosition.y),
                new Vector2(GridPosition.x, GridPosition.y + GridHeight), GridOutlineColor);
            DrawLineEditorSpace(new Vector2(GridPosition.x + GridWidth, GridPosition.y),
                new Vector2(GridPosition.x + GridWidth, GridPosition.y + GridHeight), GridOutlineColor);
            DrawLineEditorSpace(new Vector2(GridPosition.x, GridPosition.y + GridHeight),
                new Vector2(GridPosition.x + GridWidth, GridPosition.y + GridHeight), GridOutlineColor);

        }

        /// <summary>
        /// Set the viewport of the grid.
        /// X and Y are in a Range of 0..1
        /// </summary>
        public static void SetViewPort(ViewPort viewPort)
        {
            _viewPort = viewPort;
            UpdateViewport();
            ActiveView.Reset();

        }
        private static void UpdateViewport()
        {
            GridPosition = new Vector2(_editor.position.width * _viewPort.X, _editor.position.height * _viewPort.Y);

            var newViewPortWidth = _viewPort.Width * _editor.position.width;
            var newViewPortHeight = _viewPort.Height * _editor.position.height;
            
            GridWidth = newViewPortWidth;
            GridHeight = newViewPortHeight;
            
            
        }

        /// <summary>
        /// Updates the grid
        /// </summary>
        public static void Update()
        {
            UpdateViewport();

            InputManager.ProcessInput();

            DrawGrid();
            DrawGraphics();

            if (DoRepaint)
                _editor.Repaint();
        }
    }
}
