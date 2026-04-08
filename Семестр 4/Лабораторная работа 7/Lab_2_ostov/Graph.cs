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
                        if (matrix[i, j] > 0)
                        {
                            matrix[j, i] = matrix[i, j];
                        }
                    }
                }
            }
        }

        public static void Show(int[,] matrix)
        {
            Console.Write("".PadRight(5));
            for (int i = 1; i <= matrix.GetLength(0); i++)
            {
                Console.Write($"{i}".PadRight(5));
            }
            Console.WriteLine();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write($"{i + 1}".PadRight(5));
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{matrix[i,j]}".PadRight(5));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0".PadRight(5));
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
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

        public List<Edge> GetEdgesList(int vert)
        {
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < Size; i++)
            {
                if (AdjacencyMatrix[vert, i] != 0)
                {
                    if (vert < i)
                    {
                        edges.Add(new Edge(vert, i, AdjacencyMatrix[vert, i]));
                    }
                    else
                    {
                        edges.Add(new Edge(i, vert, AdjacencyMatrix[vert, i]));
                    }
                }
            }

            return edges;
        }

        public List<Edge> Kruskal()
        {
            List<Edge> edges = GetEdgesList().OrderBy(edge => edge.Weight).ToList();
            Set sets = new Set(Size);

            List<Edge> result = new List<Edge>();

            foreach (Edge edge in edges)
            {
                if (sets.FindSet(edge.Start) != sets.FindSet(edge.End))
                {
                    result.Add(edge);
                    sets.Union(edge.Start, edge.End);
                }
            }

            return result;
        }

        public List<Edge> Prim()
        {
            Set sets = new Set(Size);
            bool[] visited = new bool[Size];
            visited[0] = true;

            List<Edge> result = new List<Edge>();

            while (!visited.All(vert => vert))
            {
                List<Edge> edges = new List<Edge>();

                for (int i = 0; i < Size; i++)
                {
                    if (visited[i])
                    {
                        edges = edges.Union(GetEdgesList(i)).Distinct().ToList();
                    }
                }
                edges = edges.OrderBy(edge => edge.Weight).ToList();

                foreach (Edge edge in edges)
                {
                    if (sets.FindSet(edge.Start) != sets.FindSet(edge.End))
                    {
                        result.Add(edge);
                        sets.Union(edge.Start, edge.End);

                        visited[edge.Start] = true;
                        visited[edge.End] = true;
                        break;
                    }
                }
            }

            return result;
        }

        //public List<Edge> Prim()
        //{
        //    bool[] visited = new bool[Size];
        //    visited[0] = true;

        //    List<Edge> result = new List<Edge>();

        //    while (!visited.All(vert => vert))
        //    {
        //        int minWeight = int.MaxValue;
        //        Edge minEdge = null;

        //        for (int i = 0; i < Size; i++)
        //        {
        //            if (visited[i])
        //            {
        //                for (int j = 0; j < Size; j++)
        //                {
        //                    if (!visited[j] && AdjacencyMatrix[i, j] > 0 && AdjacencyMatrix[i, j] < minWeight)
        //                    {
        //                        minWeight = AdjacencyMatrix[i, j];
        //                        minEdge = new Edge(i, j, AdjacencyMatrix[i, j]);
        //                    }
        //                }
        //            }
        //        }

        //        if (minEdge != null)
        //        {
        //            result.Add(minEdge);
        //            visited[minEdge.Start] = true;
        //            visited[minEdge.End] = true;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    return result;
        //}
    }
}