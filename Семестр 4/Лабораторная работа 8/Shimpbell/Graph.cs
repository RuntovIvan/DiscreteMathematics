namespace Shimpbell
{
    public class Graph
    {
        public int[,] WeightMatrix { get; private set; }
        public int Size { get; private set; }

        public Graph(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            Size = int.Parse(lines[0].Trim());
            WeightMatrix = new int[Size, Size];

            for (int i = 1; i <= Size; i++)
            {
                string[] values = lines[i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < Size; j++)
                {
                    WeightMatrix[i - 1, j] = int.Parse(values[j]);
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
                        Console.Write($"{matrix[i, j]}".PadRight(5));
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

        private int GetWeight(int w1, int w2)
        {
            if (w1 == 0 || w2 == 0)
                return 0;

            return w1 + w2;
        }

        private int GetResWeight(List<int> weights, int command)
        {
            weights = weights.Where(weight => weight > 0).ToList();

            if (weights.Count == 0)
                return 0;

            if (command == 0)
                return weights.Min();
            else
                return weights.Max();
        }

        public int[,] GetMatrixPow(int[,] matrix1, int[,] matrix2, int command)
        {
            int[,] result = new int[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    List<int> weights = new List<int>();
                    for (int i = 0; i < Size; i++)
                    {
                        weights.Add(GetWeight(matrix1[row, i], matrix2[i, col]));
                    }
                    result[row, col] = GetResWeight(weights, command);
                }
            }

            return result;
        }

        public int[,] GetShimpbell(int edgedCount,  int command)
        {
            int[,] result = (int[,])WeightMatrix.Clone();

            for (int i = 1; i < edgedCount; i++)
            {
                result = GetMatrixPow(result, WeightMatrix, command);
            }

            return result;
        }
    }
}
