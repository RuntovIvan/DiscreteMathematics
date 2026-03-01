namespace Lab_2_ostov
{
    internal class SortByWeight : IComparer<Edge>
    {
        public int Compare(Edge e1, Edge e2)
        {
            if (e1.Weight < e2.Weight) return -1;
            if (e1.Weight > e2.Weight) return 1;
            return 0;
        }
    }
}
