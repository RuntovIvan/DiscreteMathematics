namespace Lab_2_ostov
{
    public class Edge
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Weight { get; set; }
        public bool IsAdded { get; set; }

        public Edge(int start, int end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
            IsAdded = false;
        }
    }
}