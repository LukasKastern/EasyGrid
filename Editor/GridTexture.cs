using UnityEngine;

namespace EasyGrid
{
    public class GridTexture : GridGraphic
    {
        private Texture _texture;
    
        public GridTexture(Cell position, int width, int height, Texture texture) : base(position, width, height)
        {
            _texture = texture;
        }
    
        public override void Draw(float currentZoom)
        {
            var gridPosition = Bounds.TopLeft.GetPosition();

            var rectInViewport = EditorGrid.GetRectInViewport(new Vector2(gridPosition.x, gridPosition.y),
                Bounds.Width * EditorGrid.ZoomedSpacing, Bounds.Height * EditorGrid.ZoomedSpacing);
        
            GUI.DrawTexture(rectInViewport, _texture);
        }
    }
}