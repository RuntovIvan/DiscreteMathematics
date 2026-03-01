namespace Lab_2_ostov
{
    public class Graph
    {
        public int[,] AdjacencyMatrix { get; private set; }
        public int Size { get; private set; }

        public Graph(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            Size = int.Parse(lines[0].Trim());
            AdjacencyMatrix = new int[Size, Size];

            for (int i = 1; i <= Size; i++)
            {
                string[] values = lines[i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < Size; j++)
                {
                    AdjacencyMatrix[i - 1, j] = int.Parse(values[j]);
                }
            }

            MakeUnoriented(AdjacencyMatrix);
        }

        private void MakeUnoriented(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = i; j < matrix.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                    }
                    else
                    {
                        if (matrix[i, j] == 1 || matrix[j, i] == 1)
                        {
                            matrix[i, j] = 1;
                            matrix[j, i] = 1;
                        }
                    }
                }
            }
        }

        public List<Edge> GetEdgesList()
        {
            List<Edge> edges = new List<Edge>();
            
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    if (AdjacencyMatrix[i, j] != 0)
                    {
                        edges.Add(new Edge(i, j, AdjacencyMatrix[i, j]));
                    }
                }
            }

            return edges;
        }
    }
}
