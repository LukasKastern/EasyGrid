namespace EasyGrid
{
    public struct ViewPort
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        /// <summary>
        /// The values of the Viewport are in a range from 0..1 where 0,0 is the Top Left corner of the editor window
        /// </summary>
        public ViewPort(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}