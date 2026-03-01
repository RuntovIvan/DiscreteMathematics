namespace Lab_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Matrix matr = new Matrix("D:\\Study\\2 курс\\Дискретная математика\\Семестр 4\\Лабораторная работа 1\\Lab_1\\matrix.txt");

                int[,] adjacencyMatrix = matr.AdjacencyMatrix;
                int[,] reachibilityMatrix = matr.ReachabilityMatrix;

                Console.WriteLine("Матрица смежности:");
                Matrix.Show(adjacencyMatrix);
                Console.WriteLine();

                matr.GetComponents();

                Console.WriteLine();
                Console.WriteLine("Матрица достижимости:");
                Matrix.Show(reachibilityMatrix);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}