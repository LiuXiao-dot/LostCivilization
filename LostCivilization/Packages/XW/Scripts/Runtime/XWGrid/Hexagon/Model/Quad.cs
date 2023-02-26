namespace XWGrid.Hexagon
{
    public struct Quad
    {
        public VertexCenter a;
        public VertexCenter b;
        public VertexCenter c;
        public VertexCenter d;

        public Quad(VertexCenter a, VertexCenter b, VertexCenter c, VertexCenter d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
    }
}