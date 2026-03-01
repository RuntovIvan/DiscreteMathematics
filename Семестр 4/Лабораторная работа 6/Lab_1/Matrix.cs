namespace Lab_1
{
    public class Matrix
    {
        // Матрица смежности
        public int[,] AdjacencyMatrix {  get; private set; }

        // Матрица достижимости
        public int[,] ReachabilityMatrix { get; private set; }

        private int[,] IdentityMatrix { get;  set; }

        // Размер матрицы
        public int Size {  get; private set; }

        // Конструктор
        public Matrix(string filename)
        {
            FillMatrix(filename);
            MakeUnoriented(AdjacencyMatrix);

            // Не знаю нужно ли
            IdentityMatrix = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                IdentityMatrix[i, i] = 1;
            }

            ReachabilityMatrix = BuildReachabilityMatrix(AdjacencyMatrix);
        }

        // Запись матрицы смежности из файла
        private void FillMatrix(string filename)
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
        }

        // Превращение в неориентированный граф
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

        // Умножение матриц смежности
        private int[,] Multiply(int[,] matrix1, int[,] matrix2)
        {
            int[,] newMatrix = new int[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    for (int i = 0; i < Size; i++)
                    {
                        if (matrix1[row,i] == 1 && matrix2[i, col] == 1)
                        {
                            newMatrix[row, col] = 1;
                            break;
                        }
                    }
                }
            }

            return newMatrix;
        }

        // Сравнение матриц на равенство
        private bool AreEqual(int[,] matrix1, int[,] matrix2)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Объединение матриц 
        private int[,] Union(int[,] matrix1, int[,] matrix2)
        {
            int[,] result = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (matrix1[i, j] == 1 || matrix2[i, j] == 1)
                    {
                        result[i, j] = 1;
                    }
                }
            }
            return result;
        }

        // Построение матрицы достижимости
        //private int[,] BuildReachabilityMatrix(int[,] matrix)
        //{
        //    int[,] prevMatrix;
        //    int[,] currentMatrix = (int[,])matrix.Clone();
        //    int[,] result;
        //    int[,] newResult = (int[,])matrix.Clone();

        //    newResult = Union(newResult, IdentityMatrix);
        //    //do
        //    //{
        //    //    prevMatrix = currentMatrix;
        //    //    result = newResult;

        //    //    currentMatrix = Multiply(prevMatrix, matrix);
        //    //    newResult = Union(result, currentMatrix);
        //    //} while (!AreEqual(result, newResult));

        //    return newResult;
        //}

        private int[,] BuildReachabilityMatrix(int[,] matrix)
        {
            int[,] result = (int[,])matrix.Clone();
            int[,] current = (int[,])matrix.Clone();

            //result = Union(result, current);
            for (int i = 2; i < Size; i++)
            {
                current = Multiply(current, matrix);
                result = Union(result, current);
            }
            return result;
        }

        // Вывод матрицы
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
                    if (matrix[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("1".PadRight(5));
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

        // Получение списка соседей вершины
        private List<int> GetNeighbours(int vert)
        {
            // Индексы вершин
            List<int> neighbours = new List<int>();

            for (int i = 0; i < Size; i++)
            {
                if (AdjacencyMatrix[vert, i] == 1)
                {
                    neighbours.Add(i);
                }
            }
            return neighbours;
        }

        // Обход графа в глубину
        private void DFS(int startVert, List<bool> visitedVerts, List<int> route)
        {
            route.Add(startVert);
            visitedVerts[startVert] = true;

            List<int> neighbours = GetNeighbours(startVert);
            for (int i = 0; i <  neighbours.Count; i++)
            {
                if (!visitedVerts[neighbours[i]])
                {
                    DFS(neighbours[i], visitedVerts, route);
                }
            }
        }

        // Нахождение первой не посещенной вершины
        private int FindFirstNotVisited(List<bool> visitedVerts)
        {
            for (int i = 0; i < visitedVerts.Count; i++)
            {
                if (!visitedVerts[i])
                {
                    return i;
                }
            }
            return -1;
        }

        // Вывод компонент графа
        public void GetComponents()
        {
            List<bool> visited = new List<bool>(Size) { };
            for (int i = 0; i < Size; i++)
            {
                visited.Add(false);
            }

            int count = 1;
            int startVert = FindFirstNotVisited(visited);
            while (startVert != -1)
            {
                List<int> component = new List<int>();
                DFS(startVert, visited, component);

                Console.Write($"Компонента {count}: ");
                for (int i = 0; i < component.Count; i++)
                {
                    Console.Write($"{component[i] + 1} ");
                }
                Console.WriteLine();

                count++;
                startVert = FindFirstNotVisited(visited);
            }
        }
    }
}