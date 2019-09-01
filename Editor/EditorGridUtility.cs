using UnityEngine;

namespace EasyGrid
{
    public static class EditorGridUtility
    {
        public static TextAnchor DirectionToAlignment(Vector2 direction)
        {
            var left = direction.x < 0;
            var up = direction.y < 0;

            if (left && up)
                return TextAnchor.UpperLeft;
        
            else if (left)
            {
                return TextAnchor.LowerLeft;
            }
        
            else if (up)
            {
                return TextAnchor.UpperRight;
            }
        
            else
            {
                return TextAnchor.LowerRight;
            }
        }
    
    }
}