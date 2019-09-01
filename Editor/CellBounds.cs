namespace EasyGrid
{
    public struct CellBounds
    {
        public Cell TopLeft;
        public int Width;
        public int Height;

        public Cell TopRight => new Cell(TopLeft.Column + Width, TopLeft.Line);
        public Cell BottomLeft => new Cell(TopLeft.Column, TopLeft.Line + Height);
    
        public Cell BottomRight => new Cell(TopLeft.Column + Width, TopLeft.Line + Height);
    
        public CellBounds(Cell topLeft, int width, int height)
        {
            TopLeft = topLeft;
            Width = width;
            Height = height;
        }

        public bool Intersects(CellBounds other)
        {
            if (TopLeft.Column >= other.BottomRight.Column || other.TopLeft.Column >= BottomRight.Column)
            {
                return false;
            }

            if (TopLeft.Line >= other.BottomRight.Line || other.TopLeft.Line >= BottomRight.Line)
            {
            
                return false;
            }

            return true;
        }
    }
}