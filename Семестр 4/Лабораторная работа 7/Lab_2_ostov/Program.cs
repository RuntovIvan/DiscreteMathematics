using ForLabs;
using System.Diagnostics;
using System.Linq;

namespace Lab_2_ostov
{
    internal class Program
    {
        public static void Kruskal(Graph graph)
        {
            Console.WriteLine("\nМатрица смежности:");
            Graph.Show(graph.AdjacencyMatrix);

            List<Edge> edges = graph.Kruskal();

            Console.WriteLine("\nАлгоритм Краскала");

            Console.WriteLine("Ребра остова:");
            foreach (Edge edge in edges)
            {

                Console.WriteLine(edge);
            }

            Console.WriteLine("Вес остова: " + edges.Sum(edge => edge.Weight));
        }

        public static void Prim(Graph graph)
        {
            Console.WriteLine("\nМатрица смежности:");
            Graph.Show(graph.AdjacencyMatrix);

            List<Edge> edges = graph.Prim();

            Console.WriteLine("\nАлгоритм Прима");

            Console.WriteLine("Ребра остова:");
            foreach (Edge edge in edges)
            {
                Console.WriteLine(edge);
            }

            Console.WriteLine("Вес остова: " + edges.Sum(edge => edge.Weight));
        }

        static void Main(string[] args)
        {
            Graph graph = new Graph(@"D:\Study\2 курс\Дискретная математика\Семестр 4\Лабораторная работа 7\Lab_2_ostov\matrix.txt");
            bool isFinished = false;
            
            do
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Алгоритм Краскала");
                Console.WriteLine("2. Алгоритм Прима");
                Console.WriteLine("3. Выход");

                int command = ReadValues.ReadIntMinMax("Введите команду: ", 1, 3);

                switch (command)
                {
                    case 1:
                        Kruskal(graph);
                        break;
                    case 2:
                        Prim(graph);
                        break;
                    case 3:
                        isFinished = true;
                        break;
                }
            } while (!isFinished);
        }
    }
}
