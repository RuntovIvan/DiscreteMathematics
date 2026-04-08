namespace Lab_2_ostov
{
    public class Edge
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Weight { get; set; }

        public Edge(int start, int end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"({Start + 1}, {End + 1}): {Weight}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Edge edge)
            {
                return Start == edge.Start && End == edge.End && Weight == edge.Weight;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Start + End + Weight;
        }
    }
}