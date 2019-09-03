using UnityEngine;

namespace EasyGrid
{
    public class GridTexture : GridGraphic
    {
        private Texture _texture;

        private Vector2 _currentGridPosition => Transform.Position.GetPosition();
        
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
            var xCoordinate = (1 - rectInViewport.width / (Transform.ZoomedBounds.Width));
            var width = 1 - xCoordinate;
            var height = rectInViewport.height / (Transform.ZoomedBounds.Height);

            var yCoordinate = 0f;
            
            if((_currentGridPosition.x + Transform.ZoomedBounds.Width) > (EditorGrid.GridPosition.x + EditorGrid.GridWidth))
            {
                xCoordinate = 0;
            }

            if (_currentGridPosition.y + Transform.ZoomedBounds.Height >
                EditorGrid.GridPosition.y + EditorGrid.GridHeight)
            {
                yCoordinate = (1 - rectInViewport.height / (Transform.ZoomedBounds.Height));
            }
            
            return new Rect(xCoordinate, yCoordinate, width, height);
        }
    
        public override void Draw(float currentZoom)
        {
            var rectInViewport = Transform.GetRectInViewport();
            
            GUI.DrawTextureWithTexCoords(rectInViewport, _texture, GetTextureCoordinates(rectInViewport));
        }
    }
}