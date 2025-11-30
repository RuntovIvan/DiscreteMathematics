namespace SystemCompleteness
{
    internal class Program
    {
        public static int GetInt(string message, int min)
        {
            int result;
            bool isParsed = false;
            do
            {
                Console.Write(message);
                isParsed = int.TryParse(Console.ReadLine(), out result);
                if (!isParsed)
                    Console.WriteLine("Должно быть введено целое число\n");
                else if (result < min)
                {
                    Console.WriteLine($"Число не может быть меньше {min}\n");
                    isParsed = false;
                }
                else
                    isParsed = true;
            } while (!isParsed);

            return result;
        }

        public static bool CheckZeroOne(string str)
        {
            foreach (char c in str)
                if (c != '0' && c != '1')
                    return false;
            return true;
        }

        public static bool CheckLength(string str)
        {
            return str.Length == 1 || str.Length == 2 || str.Length == 4 || str.Length == 8;
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
                    Console.WriteLine("Функция должна быть 1, 2 или 3 переменных\n");
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

        public static void ShowFunction(int[] function)
        {
            for (int i = 0; i < function.Length; i++)
                Console.Write(function[i]);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            int count = GetInt("Введите количество функций: ", 1);

            // задание функций
            int[][] functions = new int[count][];
            for (int i = 0; i < count; i++)
                functions[i] = GetFunction();

            // определение принадлежности к классам
            int[,] classes = new int[count, 5];
            for (int i = 0; i < count; i++)
            {
                classes[i, 0] = IsZeroPreserving(functions[i]) ? 1 : 0;
                classes[i, 1] = IsUnitPreserving(functions[i]) ? 1 : 0;
                classes[i, 2] = IsSelfDual(functions[i]) ? 1 : 0;
                classes[i, 3] = IsMonotonic(functions[i]) ? 1 : 0;
                classes[i, 4] = IsLinear(functions[i]) ? 1 : 0;
            }

            Console.WriteLine("\nФункции");
            for (int i = 0; i < count; i++)
            {
                Console.Write(i + 1 + ". ");
                ShowFunction(functions[i]);
            }
            Console.WriteLine();

            // вывод матрицы принадлежности
            Console.WriteLine("T0   T1   S   M   L");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine((classes[i,0] == 1 ? "+    " : "-    ") +
                    (classes[i, 1] == 1 ? "+    " : "-    ") +
                    (classes[i, 2] == 1 ? "+   " : "-   ") +
                    (classes[i, 3] == 1 ? "+   " : "-   ") +
                    (classes[i, 4] == 1 ? "+   " : "-   "));
            }
            Console.WriteLine();

            if (IsComplete(classes))
                Console.WriteLine("Система полная");
            else
                Console.WriteLine("Система не полная");

            if (IsBasis(classes))
                Console.WriteLine("Система - базис");
            else
                Console.WriteLine("Система - не базис");
        }

        // сохраняющая ноль
        public static bool IsZeroPreserving(int[] function)
        {
            return function[0] == 0;
        }

        // сохраняющая единицу
        public static bool IsUnitPreserving(int[] function)
        {
            return function[function.Length - 1] == 1;
        }

        // самодвойственная
        public static bool IsSelfDual(int[] function)
        {
            if (function.Length == 1)
                return false;
            for (int i = 0; i < function.Length / 2; i++)
                if (function[i] == function[function.Length - 1 - i])
                    return false;
            return true;
        }

        // определение монотонности
        // определение, входит ли один набор в другой
        public static bool IsLessOrEqual(int[,] truthTable, int a, int b)
        {
            for (int k = 0; k < truthTable.GetLength(1); k++)
            {
                if (truthTable[a, k] > truthTable[b, k])
                    return false;
            }
            return true;
        }

        public static bool IsMonotonic(int[] function)
        {
            int n = function.Length;
            if (n == 1)
                return true;

            int varsCount = (int)Math.Log(n, 2);
            int[,] truthTable = new int[n, varsCount];

            // заполнение таблицы истинности
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < varsCount; j++)
                {
                    truthTable[i, j] = (i >> (varsCount - 1 - j)) & 1;
                }
            }

            // проверка монотонности
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j && IsLessOrEqual(truthTable, i, j))
                    {
                        if (function[i] > function[j])
                            return false;
                    }
                }
            }
            return true;
        }

        public static bool IsLinear(int[] function)
        {
            int n = function.Length;

            if (n == 1)
                return true;

            int[,] triangle = new int[n, n]; // треугольная матрица

            // 1 столбец - значения функции
            for (int row = 0; row < n; row++)
            {
                triangle[row, 0] = function[row];
            }

            // построение треугольной матрицы
            for (int col = 1; col < n; col++)
            {
                for (int row = 0; row < (n - col);  row++)
                {
                    triangle[row, col] = triangle[row, col - 1] ^ triangle[row + 1, col - 1];
                }
            }

            // коэффициенты в 1 строке
            int[] coeffs = new int[n];

            for (int col = 0; col < n; col++)
            {
                coeffs[col] = triangle[0, col];
            }

            // проверка на линейность полинома Жегалкина
            for (int i = 0; i < n; i++)
            {
                if (coeffs[i] != 0) // если коэффициент 1
                {
                    if ((i & (i - 1)) != 0) // если набор не 0 или степень двойки
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsComplete(int[,] classes)
        {
            for (int col = 0; col < classes.GetLength(1); col++)
            {
                bool colResult = false;
                for (int row = 0; row < classes.GetLength(0); row++)
                {
                    if (classes[row, col] == 0)
                    {
                        colResult = true;
                        break;
                    }
                }
                
                if (!colResult)
                    return false;
            }

            return true;
        }

        public static bool IsCompleteWithoutFunction(int[,] classes, int row)
        {
            int[,] tempClasses = new int[classes.GetLength(0) - 1, classes.GetLength(1)];

            int index = 0;

            for (int i = 0; i <  classes.GetLength(0); i++)
            {
                if (i == row)
                    continue;

                for (int j = 0; j < classes.GetLength(1); j++)
                {
                    tempClasses[index, j] = classes[i, j];
                }

                index++;
            }

            return IsComplete(tempClasses);
        }

        public static bool IsBasis(int[,] classes)
        {
            if (!IsComplete(classes))
                return false;
            
            if (classes.GetLength(0) == 1)
                return true;

            // проверка, что можно удалить какую-то функцию
            for (int i = 0; i < classes.GetLength(0); i++)
            {
                if (IsCompleteWithoutFunction(classes, i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
