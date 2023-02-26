namespace XWGrid.Hexagon
{
    /// <summary>
    /// 边
    /// </summary>
    public struct Edge
    {
        public VertexCenter a;
        public VertexCenter b;

        private Edge(VertexCenter a, VertexCenter b)
        {
            this.a = a;
            this.b = b;
        }

        public static Edge CreateEdge(VertexCenter a, VertexCenter b)
        {
            if (a.Equals(b)) return default;
            return new Edge(a,b);
        }

        public static bool operator ==(Edge a,Edge b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(Edge a,Edge b)
        {
            return !a.Equals(b);
        }
        
        public override bool Equals(object obj)
        {
            if(obj is Edge other) return (this.a == other.a && this.b == other.b) || (this.a == other.b && this.b == other.a);
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() + b.GetHashCode();
        }
    }
}