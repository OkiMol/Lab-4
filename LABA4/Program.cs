namespace Laba4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var isRunning = true;
            var option = 0;
            var stringArray = "";
            var extra = "";
            var isPrinted = false;
            const int limit = 100;
            var arrayLength = InputLimited("Введите количество элементов массива: ", 1, limit);
            var fillOptions = new string[] { "0. Ввести элементы вручную.", "1. Сгенерировать случайные элементы." };
            var array = Fill(Menu(0, fillOptions) == 1, arrayLength);
            var isSorted = IsSorted(array);
            var isHighlighted = false;
            while (isRunning)
            {
                var options = new string[]
                {
                    "0. Выход.",
                    isPrinted
                        ? "1. Скрыть массив" 
                        : "1. Распечатать массив",
                    "2. Удалить элемент с заданным номером",
                    "3. Добавить N элементов начиная с K-го элемента",
                    "4. Добавить элемент на K-ую позицию",
                    "5. Четные элементы переставить в начало массива, нечетные - в конец",
                    isHighlighted
                        ? "6. Скрыть элементы равные среднему арифметическому элементов массива"
                        : "6. Показать элементы равные среднему арифметическому элементов массива",
                    "7. Отсортировать массив простым включением (сортировка вставками)",
                    "8. Бинарный поиск заданного элемента",
                    "9. Отсортировать массив быстрой сортировкой Ломуто",
                    "10. Пересоздать массив"
                };
                option = Menu(option,
                    options,
                    $"Длина массива: {arrayLength}\n" +
                    (!isSorted
                        ? "^Массив не отсортирован^\n"
                        : "#Массив отсортирован#\n") +
                    extra,
                    isPrinted
                        ? $"\n{stringArray}\n"
                        : "");
                switch (option)
                {
                    case 0:
                        isRunning = false;
                        break;
                    case 1:
                        if (!isPrinted)
                        {
                            if (isHighlighted)
                                (stringArray, extra) = FindEqualAverage(array);
                            else
                                stringArray = WriteArray(array);
                        }
                        isPrinted = !isPrinted;
                        break;
                    case 2:
                        array = DeleteElement(array);
                        if (isHighlighted)
                            (stringArray, extra) = FindEqualAverage(array);
                        else
                            stringArray = WriteArray(array);
                        isSorted = IsSorted(array);
                        arrayLength = array.Length;
                        break;
                    case 3:
                        array = AddElements(array);
                        if (isHighlighted)
                            (stringArray, extra) = FindEqualAverage(array);
                        else
                            stringArray = WriteArray(array);
                        isSorted = IsSorted(array);
                        arrayLength = array.Length;
                        break;
                    case 4:
                        array = AddElement(array);
                        if (isHighlighted)
                            (stringArray, extra) = FindEqualAverage(array);
                        else
                            stringArray = WriteArray(array);
                        isSorted = IsSorted(array);
                        arrayLength = array.Length;
                        break;
                    case 5:
                        array = BeginEvenEndOdd(array);
                        if (isHighlighted)
                            (stringArray, extra) = FindEqualAverage(array);
                        else
                            stringArray = WriteArray(array);
                        isSorted = IsSorted(array);
                        break;
                    case 6:
                        if (!isHighlighted)
                        {
                            (stringArray, extra) = FindEqualAverage(array);
                            isHighlighted = !isHighlighted;
                        }
                        else
                        {
                            isHighlighted = !isHighlighted;
                            stringArray = WriteArray(array);
                            extra = "";
                        }
                        break;
                    case 7:
                        array = InsertionSort(array);
                        isSorted = true;
                        if (isHighlighted)
                            (stringArray, extra) = FindEqualAverage(array);
                        else
                            stringArray = WriteArray(array);
                        break;
                    case 8:
                        if (isSorted)
                        {
                            (var elementPosition, extra) = BinarySearch(array);
                            if (elementPosition != -1)
                                stringArray = WriteArray(array, [elementPosition]);
                            else
                            {
                                extra += $"\n^Элемент не найден^";
                                stringArray = WriteArray(array);
                            }
                        }
                        else
                            extra = "Отсортируйте массив!";
                        break;
                    case 9:
                        quickSort(array);
                        if (isHighlighted)
                            (stringArray, extra) = FindEqualAverage(array);
                        else
                            stringArray = WriteArray(array);
                        isSorted = IsSorted(array);
                        break;
                    case 10:
                        Console.CursorVisible = true;
                        arrayLength = InputLimited("Введите количество элементов массива: ", 1, limit);
                        fillOptions = new string[] { "0. Ввести элементы вручную.", "1. Сгенерировать случайные элементы." };
                        array = Fill(Menu(0, fillOptions) == 1, arrayLength);
                        isSorted = IsSorted(array);
                        stringArray = WriteArray(array);
                        break;
                }
            }
            Console.CursorVisible = true;
        }

        public static int Input(string message)
        {
            int number;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out number))
                Console.Write($"Ошибка! Введите целое число.\n{message}");
            return number;
        }

        public static int InputLimited(string message, int lowerBound, int upperBound = int.MaxValue)
        {
            int input;
            do
            {
                input = Input(message);
                if (input < lowerBound || input > upperBound)
                    Console.WriteLine($"Ошибка! Число должно быть не меньше {lowerBound} и не больше {upperBound}!");
            } while (input < lowerBound || input > upperBound);
            return input;
        }

        public static void PrintHighlighted(string message)
        {
            var highlight = 0;
            foreach (var sign in message)
            {
                if (sign == '~')
                    highlight = highlight == 1 ? 0 : 1;
                else if (sign == '#')
                    highlight = highlight == 2 ? 0 : 2;
                else if (sign == '^')
                    highlight = highlight == 3 ? 0 : 3;
                else if (highlight == 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(sign);
                    Console.ResetColor();
                }
                else if (highlight == 2)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(sign);
                    Console.ResetColor();
                }
                else if (highlight == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(sign);
                    Console.ResetColor();
                }
                else
                    Console.Write(sign);

            }
        }

        public static int Menu(int option, string[] options, string highlightedMessage = "", string array = "")
        {
            Console.CursorVisible = false;
            var isRunning = true;
            var length = options.Length;
            while (isRunning)
            {
                Console.Clear();
                for (var i = 0; i < length; i++)
                {
                    Console.ResetColor();
                    if (i == option)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(options[i]);
                        Console.ResetColor();
                    }
                    else
                        Console.WriteLine(options[i]);
                }

                if (array != "")
                {
                    var isHighlighting = false;
                    foreach (var sign in array)
                    {
                        if (sign == '~') isHighlighting = !isHighlighting;
                        else if (isHighlighting)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(sign);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else Console.Write(sign);
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }

                if (highlightedMessage != "")
                    PrintHighlighted(highlightedMessage);

                var key = Console.ReadKey();
                ConsoleKey[] keys = [ConsoleKey.D0, ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9];
                if (key.Key == ConsoleKey.UpArrow)
                    option = (option - 1 + length) % length;
                else if (key.Key == ConsoleKey.DownArrow)
                    option = (option + 1) % length;
                else if (key.Key == ConsoleKey.Enter)
                    isRunning = false;
                else
                {
                    for (var i = 0; i < (length < keys.Length ? length : keys.Length); i++)
                    {
                        if (key.Key == keys[i])
                        {
                            option = i;
                            break;
                        }
                    }
                }
                Console.ResetColor();
                Console.Clear();
            }


            return option;
        }

        public static int[] Fill(bool isRandom, int length)
        {
            Console.CursorVisible = true;
            var array = new int[length];
            if (isRandom)
            {
                var lowerBound = Input("Введите нижнюю границу: ");
                var upperBound = InputLimited("Введите верхнюю границу: ", lowerBound);
                var random = new Random();
                for (var i = 0; i < length; i++)
                    array[i] = random.Next(lowerBound, upperBound + 1);
            }
            else
            {
                Console.WriteLine("Введите элементы массива:");
                for (var i = 0; i < length; i++)
                    array[i] = Input($"{i + 1} -> ");
            }
            return array;
        }

        public static string WriteArray(int[] array, int[]? highlights = null)
        {
            var stringArray = "";
            if (highlights != null)
            {
                for (var i = 0; i < array.Length; i++)
                    stringArray += highlights.Contains(i) ? $"~{array[i]}~ " : $"{array[i]} ";
            }
            else
            {
                foreach (var num in array)
                    stringArray += $"{num} ";
            }
            return stringArray;
        }

        public static bool IsSorted(int[] array)
        {
            int i;
            for (i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                    break;
            }
            return array.Length == 1 || i == array.Length;
        }

        public static int[] DeleteElement(int[] array)
        {
            int position;
            bool isRight;
            do
            {
                position = Input("Введите номер удаляемого элемента: ");
                isRight = position > 0 && position <= array.Length;
                Console.Write(isRight ? "" : "Номер не может быть меньше 1 или больше длины массива\n");
            } while (!isRight);

            int[] arrayTemporal = new int[array.Length - 1];
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (i == position - 1)
                    continue;
                arrayTemporal[j++] = array[i];
            }
            return arrayTemporal;
        }

        public static int[] AddElements(int[] array)
        {
            Console.CursorVisible = true;
            var amount = InputLimited("Введите количество добавляемых элементов: ", 1);
            int position;
            bool isRight;
            do
            {
                Console.WriteLine("Введите позицию, после которой добавятся новые элементы: ");
                position = Input("") - 1;
                isRight = position >= 0 && position <= array.Length;
                Console.Write(isRight ? "" : "Номер не может быть меньше 1 или больше длины массива.");
            } while (!isRight);

            Console.CursorVisible = false;
            var fillOptions = new string[] {
                "0. Ввести элементы вручную.",
                "1. Сгенерировать случайные элементы."
            };
            var additionalArray = Fill(Menu(0, fillOptions) == 1, amount);
            var temporalArray = new int[array.Length + amount];
            for (int i = 0; i < position; i++)
                temporalArray[i] = array[i];
            for (int i = 0; i < amount; i++)
                temporalArray[i + position] = additionalArray[i];
            for (int i = position; i < temporalArray.Length - amount; i++)
                temporalArray[i + amount] = array[i];
            return temporalArray;
        }

        public static int[] AddElement(int[] array)
        {
            int element;
            int position;
            bool isRight;
            do
            {
                Console.Write("Введите позицию нового элемента: ");
                position = Input("") - 1;
                isRight = position >= 0 && position <= array.Length;
                Console.Write(isRight ? "" : "Номер не может быть меньше 1 или больше длины массива.");
            } while (!isRight);
            Console.Clear();

            var fillOptions = new string[] { "0. Ввести элементы вручную.", "1. Сгенерировать случайные элементы." };
            if (Menu(0, fillOptions) == 0)
                element = Input("Введите элемент: ");
            else
            {
                var lowerBound = Input("Введите нижнюю границу: ");
                var upperBound = InputLimited("Введите верхнюю границу: ", lowerBound);
                var random = new Random();
                element = random.Next(lowerBound, upperBound + 1);
            }

            var temporalArray = new int[array.Length + 1];
            for (int i = 0; i < position; i++)
                temporalArray[i] = array[i];
            temporalArray[position] = element;
            for (int i = position + 1; i < temporalArray.Length; i++)
                temporalArray[i] = array[i - 1];

            return temporalArray;
        }

        public static int[] BeginEvenEndOdd(int[] array)
        {
            var count = 0;
            var temporalArray = new int[array.Length];
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (array[i] % 2 == 0)
                {
                    temporalArray[j] = array[i];
                    j++;
                    count++;
                }
            }
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (array[i] % 2 != 0)
                {
                    temporalArray[count + j] = array[i];
                    j++;
                }
            }
            return temporalArray;
        }

        public static Tuple<string, string> FindEqualAverage(int[] array)
        {
            var sum = 0.0;
            foreach (var num in array)
            {
                sum += num;
            }
            var average = sum / array.Length;
            var highlights = new int[array.Length];
            var count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                highlights[i] = array[i] == average ? i : -1;
                count++;
            }
            var arrayString = WriteArray(array, highlights);
            return Tuple.Create(arrayString, $"Количество сравнений: {count}");
        }

        public static int[] InsertionSort(int[] array)
        {
            for (var i = 1; i < array.Length; i++)
            {
                for (var j = i - 1; j >= 0 && array[j + 1] < array[j]; j--)
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
            }
            return array;
        }

        public static Tuple<int, string> BinarySearch(int[] array)
        {
            Console.CursorVisible = true;
            var element = Input("Введите искомый элемент: ");
            var start = 0;
            var end = array.Length - 1;
            var count = 1;
            var isFound = false;
            while (start <= end)
            {
                if (array[(start + end) / 2] == element)
                {
                    isFound = true;
                    break;
                }

                if (array[(start + end) / 2] > element) end = (start + end) / 2 - 1;
                else start = (start + end) / 2 + 1;
                count++;
            }

            return Tuple.Create(isFound ? (start + end) / 2 : -1, $"Количество сравнений: {count}");
        }

        private static int partOfSort(int[] array, int start, int end)
        {
            var left = start;
            for (int current = start; current < end; current++)
            {
                if (array[current] <= array[end])
                {
                    (array[left], array[current]) = (array[current], array[left]);
                    left++;
                }
            }
            (array[left], array[end]) = (array[end], array[left]);
            return left;
        }

        private static void quickSort(int[] array, int start = 0, int end = -1)
        {
            if (end == -1)
                end = array.Length - 1;
            if (start >= end)
                return;
            int rightStart = partOfSort(array, start, end);
            quickSort(array, start, rightStart - 1);
            quickSort(array, rightStart + 1, end);
        }
    }
}
