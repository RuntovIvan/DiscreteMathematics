using ForLabs;

namespace Shimpbell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph(@"D:\Study\2 курс\Дискретная математика\Семестр 4\Лабораторная работа 8\Shimpbell\matrix.txt");

            Graph.Show(graph.WeightMatrix);
            
            int edgesCount = ReadValues.ReadInt("\nВведите количество дуг: ");
            int command = ReadValues.ReadIntMinMax("Введите направление поиска (min - 0, max - 1): ", 0, 1);

            int[,] result = graph.GetShimpbell(edgesCount, command);

            Console.WriteLine();
            Graph.Show(result);

            //Console.WriteLine("\nМинимальные веса:");
            //Graph.Show(graph.GetShimpbell(edgesCount, 0));

            //Console.WriteLine("\nМаксимальные веса:");
            //Graph.Show(graph.GetShimpbell(edgesCount, 1));
        }
    }
}
