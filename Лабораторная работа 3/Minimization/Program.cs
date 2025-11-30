using System.Collections.Generic;
namespace Minimization
{
    public class Program
    {
        public static bool CheckZeroOne(string str)
        {
            foreach (char c in str)
                if (c != '0' && c != '1')
                    return false;
            return true;
        }

        public static bool CheckLength(string str)
        {
            return str.Length == 16;
        }

        // получить функцию в виде массива
        public static int[] GetFunction()
        {
            bool isParsed = false;
            string strFunc;
            do
            {
                Console.Write("Введите функцию: ");
                strFunc = Console.ReadLine();

                if (!CheckZeroOne(strFunc))
                {
                    Console.WriteLine("Неизвестный символ\n");
                }
                else if (!CheckLength(strFunc))
                {
                    Console.WriteLine("Функция должна быть 4 переменных\n");
                }
                else
                {
                    isParsed = true;
                }
            } while (!isParsed);

            int[] function = new int[strFunc.Length];
            for (int i = 0; i < function.Length; i++)
                function[i] = int.Parse(strFunc[i].ToString());

            return function;
        }

        public static int[] GetSet(int a, int varsCount)
        {
            int[] set = new int[varsCount];
            for (int i = 0; i < varsCount; i++)
                set[i] = (a >> (varsCount - 1 - i)) & 1;

            return set;
        }

        public static int[][] GetSDNF(int[] function)
        {
            int varsCount = (int)Math.Log(function.Length, 2);

            int oneCount = 0;
            foreach (int i in function)
                oneCount += i;

            int[][] sdnf = new int[oneCount][];
            int k = 0;
            for (int i = 0; i < function.Length; i++)
                if (function[i] == 1)
                    sdnf[k++] = GetSet(i, varsCount);

            return sdnf;
        }

        public static string GetSetString(int[] set)
        {
            int minusCount = 0;
            for (int i = 0; i < set.Length; i++)
                if (set[i] == -1)
                    minusCount++;

            if (minusCount == set.Length)
            {
                return "1";
            }

            string setString = "";
            
            for (int i = 0; i < set.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (set[i] == 0)
                            setString += "X";
                        else if (set[i] == 1)
                            setString += "x";
                        break;
                    case 1:
                        if (set[i] == 0)
                            setString += "Y";
                        else if (set[i] == 1)
                            setString += "y";
                        break;
                    case 2:
                        if (set[i] == 0)
                            setString += "Z";
                        else if (set[i] == 1)
                            setString += "z";
                        break;
                    case 3:
                        if (set[i] == 0)
                            setString += "W";
                        else if (set[i] == 1)
                            setString += "w";
                        break;
                }
            }
            return setString;
        }

        public static void ShowSDNF(int[][] sdnf)
        {
            if (sdnf.GetLength(0) == 0)
            {
                Console.WriteLine("Нет СДНФ");
                Console.WriteLine();
                return;
            }

            for (int i = 0; i < sdnf.GetLength(0); i++)
            {
                Console.Write(GetSetString(sdnf[i]));
                if (i != sdnf.GetLength(0) - 1)
                    Console.Write(" v ");
            }

            Console.WriteLine();
        }

        public static bool CanMerge(int[] a, int[] b)
        {
            int differCount = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    differCount++;
            }
            return differCount == 1;
        }

        public static int[] Merge(int[] a, int[] b)
        {
            int[] implicant = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                    implicant[i] = a[i];
                else
                    implicant[i] = -1;
            }

            return implicant;
        }

        public static bool AreImplicantsEqual(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }

        public static bool ContainsImplicant(List<int[]> implicants, int[] a)
        {
            foreach (int[] item in implicants)
                if (AreImplicantsEqual(a, item))
                    return true;
            return false;
        }

        // метод для нахождения простых импликант
        public static List<int[]> GetImplicants(int[][] sdnf)
        {
            // простые импликанты
            List<int[]> primeImplicants = new List<int[]>();

            // текущие импликанты
            List<int[]> currentImplicants = new List<int[]>();
            foreach (int[] item in sdnf)
                currentImplicants.Add(item);

            // склеивание
            int step = 1;
            while (currentImplicants.Count > 0)
            {
                Console.WriteLine($"\nШаг {step} склеивания");

                for (int i = 0; i <  currentImplicants.Count; i++)
                {
                    Console.Write(GetSetString(currentImplicants[i]));
                    if (i != currentImplicants.Count - 1)
                        Console.Write("; ");
                }
                Console.WriteLine();

                // Следующие импликанты
                List<int[]> nextImplicants = new List<int[]>();

                // Использовалась ли текущая импликанта
                bool[] used = new bool[currentImplicants.Count];

                for (int i = 0; i < currentImplicants.Count; i++)
                    for (int j = i + 1; j < currentImplicants.Count; j++)
                        if (CanMerge(currentImplicants[i], currentImplicants[j]))
                        {
                            used[i] = used[j] = true;
                            int[] merge = Merge(currentImplicants[i], currentImplicants[j]);
                            nextImplicants.Add(merge);
                            Console.WriteLine(GetSetString(currentImplicants[i]) + " - " +
                                GetSetString(currentImplicants[j]) + " : " + GetSetString(merge));
                        }

                // неиспользованные импликанты в простые
                for (int i = 0; i < currentImplicants.Count; i++)
                    if (!used[i] && !ContainsImplicant(primeImplicants, currentImplicants[i]))
                        primeImplicants.Add(currentImplicants[i]);

                currentImplicants = nextImplicants.Distinct().ToList();
                step++;

                if (currentImplicants.Count == 0)
                {
                    Console.WriteLine("Нет склеек");
                    break;
                }
            }

            return primeImplicants;
        }
        public static bool IsImplicantIn(int[] minterm, int[] implicant)
        {
            for (int i = 0; i < implicant.Length; i++)
            {
                if (implicant[i] != -1 && (minterm[i] != implicant[i]))
                    return false;
            }
            return true;
        }

        public static int[,] GetImplicantMatrix(int[][] sdnf, List<int[]> implicants)
        {
            int[,] matrix = new int[implicants.Count, sdnf.GetLength(0)];

            for (int i = 0; i < implicants.Count; i++)
            {
                for (int j = 0; j < sdnf.GetLength(0); j++)
                {
                    matrix[i, j] = (IsImplicantIn(sdnf[j], implicants[i]) ? 1 : 0);
                }
            }

            return matrix;
        }

        public static void ShowImplicantMatrix(int[,] matrix, int[][] sdnf, List<int[]> implicants)
        {
            Console.WriteLine("\nМатрица импликантности");
            Console.Write("".PadLeft(6));

            for (int i = 0; i < sdnf.GetLength(0); i++)
            {
                Console.Write(GetSetString(sdnf[i]).PadRight(6));
            }

            Console.WriteLine();

            for (int i = 0; i < implicants.Count; i++)
            {
                Console.Write(GetSetString(implicants[i]).PadRight(6));
                for (int j = 0; j < sdnf.GetLength(0); j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("+".PadRight(6));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("-".PadRight(6));
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            int[] function = GetFunction();

            Console.WriteLine("Функция: ");
            foreach (int i in function)
            {
                Console.Write(i);
            }
            Console.WriteLine();

            int[][] sdnf = GetSDNF(function);

            Console.WriteLine("\nСДНФ:");
            ShowSDNF(sdnf);

            List<int[]> implicants = GetImplicants(sdnf);

            Console.WriteLine("\nИмпликанты:");
            foreach (int[] item in implicants)
            {
                Console.WriteLine(GetSetString(item));
            }

            int[,] matrix = GetImplicantMatrix(sdnf, implicants);

            ShowImplicantMatrix(matrix, sdnf, implicants);

            Console.WriteLine("\nМДНФ:");
            ShowMDNF(matrix, sdnf, implicants);
        }

        // построение покрытия минтермов импликантами
        public static List<List<int>> BuildCoverage(int[,] matrix, int[][] sdnf, List<int[]> implicants)
        {
            List<List<int>> coverage = new List<List<int>>();

            for (int j = 0; j < sdnf.Length; j++)
            {
                List<int> coveringImplicants = new List<int>(); // импликанты минтерма
                for (int i = 0; i < implicants.Count; i++)
                    if (matrix[i,j] == 1)
                        coveringImplicants.Add(i); // индексы импликантов

                coverage.Add(coveringImplicants);
            }

            return coverage;
        }

        // построение функции Петрика
        public static List<List<int>> BuildPetrickFunction(List<List<int>> coverage)
        {
            // функция Петрика - произведение логических сумм импликант
            return new List<List<int>>(coverage);
        }

            // раскрытие скобок
            public static List<List<int>> ExpandPetrickFunction(List<List<int>> petrickFunction)
            {
                if (petrickFunction.Count == 0)
                    return new List<List<int>>();

                // начало с первого множителя
                List<List<int>> result = new List<List<int>>();
                foreach (int impl in petrickFunction[0])
                    result.Add(new List<int> { impl });

                // последовательное умножение на на остальные множители
                for (int i = 1; i < petrickFunction.Count; i++)
                {
                    result = MultiplyPetrickTerms(result, petrickFunction[i]);
                    result = RemoveDominatedTerms(result);
                }

                return result;
            }

            // умножение термов
            public static List<List<int>> MultiplyPetrickTerms(List<List<int>> current, List<int> next)
            {
                List<List<int>> newResult = new List<List<int>>();

                foreach (List<int> term in current)
                {
                    foreach (int imlp in next)
                    {
                        List<int> newTerm = new List<int>(term);
                        if (!newTerm.Contains(imlp))
                            newTerm.Add(imlp);
                        newTerm.Sort();

                        if (!newResult.Any(t => t.SequenceEqual(newTerm)))
                            newResult.Add(newTerm);
                    }
                }

                return newResult;
            }

            // удаление доминируемых термов
            public static List<List<int>> RemoveDominatedTerms(List<List<int>> terms)
            {
                List<List<int>> result = new List<List<int>>();

                for (int i = 0; i < terms.Count; i++)
                {
                    bool isDominated = false;

                    for (int j = 0; j < terms.Count; j++)
                    {
                        if (i != j && terms[j].All(impl => terms[i].Contains(impl)))
                        {
                            isDominated = true;
                            break;
                        }
                    }

                    if (!isDominated)
                        result.Add(terms[i]);
                }

                return result;
            }

        // поиск минимальных покрытий
        public static List<List<int>> FindMinimalCovers(List<List<int>> allCovers)
        {
            if (allCovers.Count == 0)
                return new List<List<int>>();

            int minSize = allCovers.Min(cover =>  cover.Count);
            List<List<int>> minimalCovers = allCovers.Where(cover => cover.Count == minSize).ToList();

            return minimalCovers;
        }

        // построение мднф
        public static void ShowMDNF(int[,] matrix, int[][] sdnf, List<int[]> implicants)
        {
            if (implicants.Count == 0)
            {
                Console.WriteLine("Нет МДНФ");
                return;
            }

            List<List<int>> coverage = BuildCoverage(matrix, sdnf, implicants);

            List<List<int>> petrickFunction = BuildPetrickFunction(coverage);

            List<List<int>> allCovers = ExpandPetrickFunction(petrickFunction);

            List<List<int>> minimalCovers = FindMinimalCovers(allCovers);

            foreach (List<int> cover in minimalCovers)
            {
                for (int i = 0; i < cover.Count; i++)
                {
                    Console.Write(GetSetString(implicants[cover[i]]));
                    if (i != cover.Count - 1)
                        Console.Write(" v ");
                }
                Console.WriteLine();
            }
        }
    }
}
