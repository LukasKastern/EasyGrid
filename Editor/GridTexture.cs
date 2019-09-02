using UnityEngine;

namespace EasyGrid
{
    public class GridTexture : GridGraphic
    {
        private Texture _texture;

        private Vector2 _currentGridPosition => Bounds.TopLeft.GetPosition();
        
        public GridTexture(Cell position, int width, int height, Texture texture) : base(position, width, height)
        {
            _texture = texture;
        }

        /// <summary>
        /// We use these coordinates so we can cutoff textures correctly if their not inside the grid viewport.
        /// </summary>
        /// <returns></returns>
        private Rect GetTextureCoordinates(Rect rectInViewport)
        {
            var xCoordinate = (1 - rectInViewport.width / (Bounds.Width * EditorGrid.ZoomedSpacing));
            var width = 1 - xCoordinate;
            var height = rectInViewport.height / (Bounds.Height * EditorGrid.ZoomedSpacing);

            var yCoordinate = 0f;
            
            if((_currentGridPosition.x + Bounds.Width * EditorGrid.ZoomedSpacing) > (EditorGrid.GridPosition.x + EditorGrid.GridWidth))
            {
                xCoordinate = 0;
            }

            if (_currentGridPosition.y + Bounds.Height * EditorGrid.ZoomedSpacing >
                EditorGrid.GridPosition.y + EditorGrid.GridHeight)
            {
                yCoordinate = (1 - rectInViewport.height / (Bounds.Height * EditorGrid.ZoomedSpacing));
            }
            
            return new Rect(xCoordinate, yCoordinate, width, height);
        }
    
        public override void Draw(float currentZoom)
        {
            var rectInViewport = EditorGrid.GetRectInViewport(new Vector2(_currentGridPosition.x, _currentGridPosition.y),
                Bounds.Width * EditorGrid.ZoomedSpacing, Bounds.Height * EditorGrid.ZoomedSpacing);
            
            GUI.DrawTextureWithTexCoords(rectInViewport, _texture, GetTextureCoordinates(rectInViewport));
        }
    }
}