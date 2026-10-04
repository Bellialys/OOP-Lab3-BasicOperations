namespace Basics {
    internal class Program {
        static void Main(string[] args) {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool programIsRunning = true;

            while (programIsRunning) {
                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("              ФІТ-2-15");
                Console.WriteLine("      Лабораторна робота №3");
                Console.WriteLine("========================================");
                Console.WriteLine("1 — Змінити порядок цифр числа");
                Console.WriteLine("2 — Видалити перші X слів");
                Console.WriteLine("3 — Обчислити площу фігури");
                Console.WriteLine("4 — Замінити кожен 3-й та 5-й символ");
                Console.WriteLine("0 — Вийти");
                Console.Write("Оберіть завдання: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice) {
                    case "1": {
                            ReverseNumber();
                            break;
                        }

                    case "2": {
                            RemoveWordsOrDigits();
                            break;
                        }

                    case "3": {
                            CalculateArea();
                            break;
                        }

                    case "4": {
                            ReplaceCharacters();
                            break;
                        }

                    case "0": {
                            programIsRunning = false;
                            break;
                        }

                    default: {
                            Console.WriteLine("Помилка: оберіть число від 0 до 4.");
                            break;
                        }
                }
            }
        }


        #region TASK 1

        static void ReverseNumber() {
            Console.Write("Введіть ціле число будь-якої довжини: ");
            string input = (Console.ReadLine() ?? "").Trim();

            char sign;
            string digits;

            if (!TrySplitInteger(input, out sign, out digits)) {
                Console.WriteLine("Помилка: потрібно ввести ціле число.");
                return;
            }

            // Прибираємо початкові нулі, тому що працюємо саме з числом.
            int firstDigit = 0;

            while (firstDigit < digits.Length - 1 && digits[firstDigit] == '0') {
                firstDigit++;
            }

            digits = digits.Substring(firstDigit);

            if (digits.Length == 1) {
                Console.WriteLine("Попередження: введено лише одну цифру.");
                Console.WriteLine("Змінювати порядок цифр немає сенсу — значення не зміниться.");

                if (digits == "0") {
                    Console.WriteLine("Результат: 0");
                } else if (sign == '-' || sign == '+') {
                    Console.WriteLine("Результат: " + sign + digits);
                } else {
                    Console.WriteLine("Результат: " + digits);
                }

                return;
            }

            // Створюємо масив символів і записуємо цифри у зворотному порядку.
            char[] reversedCharacters = new char[digits.Length];

            for (int i = 0; i < digits.Length; i++) {
                reversedCharacters[i] = digits[digits.Length - 1 - i];
            }

            string reversed = new string(reversedCharacters);

            // 1000 -> 0001 -> 1
            int firstNonZero = 0;

            while (firstNonZero < reversed.Length - 1 && reversed[firstNonZero] == '0') {
                firstNonZero++;
            }

            reversed = reversed.Substring(firstNonZero);

            if (reversed == "0") {
                Console.WriteLine("Результат: 0");
            } else if (sign == '-' || sign == '+') {
                Console.WriteLine("Результат: " + sign + reversed);
            } else {
                Console.WriteLine("Результат: " + reversed);
            }
        }

        #endregion


        #region TASK 2

        static void RemoveWordsOrDigits() {
            Console.Write("Введіть текст: ");
            string text = Console.ReadLine() ?? "";
            string trimmedText = text.Trim();

            char sign;
            string numberDigits;

            // Якщо користувач ввів тільки число, працюємо з його цифрами.
            if (TrySplitInteger(trimmedText, out sign, out numberDigits)) {
                Console.WriteLine(
                    "Попередження: \"" + trimmedText +
                    "\" — це не слова, а числове значення.");
                Console.WriteLine("Для числового значення будуть видалятися перші X цифр.");

                int digitCount = ReadNonNegativeInteger("Скільки перших цифр видалити? ");

                if (digitCount >= numberDigits.Length) {
                    Console.WriteLine("Результат: ");
                    return;
                }

                string resultDigits = numberDigits.Substring(digitCount);

                if (sign == '-' || sign == '+') {
                    Console.WriteLine("Результат: " + sign + resultDigits);
                } else {
                    Console.WriteLine("Результат: " + resultDigits);
                }

                return;
            }

            // Для звичайного тексту числа теж вважаються окремими елементами,
            // але програма повідомляє, що це числові значення, а не слова.
            AnalyzeTextElements(text);

            int count = ReadNonNegativeInteger(
                "Скільки перших слів/числових значень видалити? ");

            int index = 0;

            for (int removed = 0; removed < count && index < text.Length; removed++) {
                while (index < text.Length && char.IsWhiteSpace(text[index])) {
                    index++;
                }

                while (index < text.Length && !char.IsWhiteSpace(text[index])) {
                    index++;
                }
            }

            if (count > 0) {
                while (index < text.Length && char.IsWhiteSpace(text[index])) {
                    index++;
                }
            }

            Console.WriteLine("Результат: " + text.Substring(index));
        }


        static void AnalyzeTextElements(string text) {
            int wordCount = 0;
            int numberCount = 0;
            int index = 0;

            while (index < text.Length) {
                while (index < text.Length && char.IsWhiteSpace(text[index])) {
                    index++;
                }

                if (index >= text.Length) {
                    break;
                }

                int start = index;

                while (index < text.Length && !char.IsWhiteSpace(text[index])) {
                    index++;
                }

                string element = text.Substring(start, index - start);
                string elementWithoutPunctuation = element.Trim(
                    '(', ')', '[', ']', '{', '}', '"', '\'', ';', '!', '?', ':', '.', ',');

                char sign;
                string digits;

                if (TrySplitInteger(elementWithoutPunctuation, out sign, out digits)) {
                    numberCount++;

                    Console.WriteLine(
                        "Попередження: \"" + element +
                        "\" — це не слово, а числове значення.");
                } else {
                    wordCount++;
                }
            }

            Console.WriteLine(
                "Розпізнано: слів — " + wordCount +
                ", числових значень — " + numberCount + ".");
        }

        #endregion


        #region TASK 3

        static void CalculateArea() {
            Console.WriteLine("1 — Квадрат");
            Console.WriteLine("2 — Круг");
            Console.WriteLine("3 — Прямокутник");
            Console.WriteLine("4 — Трикутник");
            Console.Write("Оберіть фігуру: ");

            string figure = Console.ReadLine() ?? "";

            switch (figure) {
                case "1": {
                        double side = ReadPositiveDouble("Введіть сторону квадрата: ");
                        double area = side * side;

                        Console.WriteLine("Площа квадрата: " + Math.Round(area, 10));
                        break;
                    }

                case "2": {
                        double radius = ReadPositiveDouble("Введіть радіус круга: ");
                        double area = Math.PI * radius * radius;

                        Console.WriteLine("Площа круга: " + Math.Round(area, 10));
                        break;
                    }

                case "3": {
                        double sideA = ReadPositiveDouble("Введіть першу сторону: ");
                        double sideB = ReadPositiveDouble("Введіть другу сторону: ");
                        double area = sideA * sideB;

                        Console.WriteLine("Площа прямокутника: " + Math.Round(area, 10));
                        break;
                    }

                case "4": {
                        double triangleBase = ReadPositiveDouble("Введіть основу трикутника: ");
                        double height = ReadPositiveDouble("Введіть висоту трикутника: ");
                        double area = triangleBase * height / 2;

                        Console.WriteLine("Площа трикутника: " + Math.Round(area, 10));
                        break;
                    }

                default: {
                        Console.WriteLine("Помилка: оберіть число від 1 до 4.");
                        break;
                    }
            }
        }


        static double ReadPositiveDouble(string message) {
            while (true) {
                Console.Write(message);
                string input = (Console.ReadLine() ?? "").Trim();

                double value;

                if (TryParsePositiveDouble(input, out value)) {
                    return value;
                }

                Console.WriteLine(
                    "Помилка: введіть додатне число. " +
                    "Можна використовувати крапку або кому.");
            }
        }


        // Просте перетворення десяткового числа без сторонніх бібліотек.
        // Підтримуються і 2.5, і 2,5.
        static bool TryParsePositiveDouble(string text, out double value) {
            value = 0;

            if (text.Length == 0) {
                return false;
            }

            bool separatorFound = false;
            bool digitFound = false;
            double divider = 10;

            for (int i = 0; i < text.Length; i++) {
                char current = text[i];

                if (current >= '0' && current <= '9') {
                    int digit = current - '0';
                    digitFound = true;

                    if (!separatorFound) {
                        value = value * 10 + digit;
                    } else {
                        value = value + digit / divider;
                        divider = divider * 10;
                    }
                } else if ((current == '.' || current == ',') && !separatorFound) {
                    separatorFound = true;
                } else {
                    return false;
                }
            }

            if (!digitFound || value <= 0 || double.IsInfinity(value)) {
                return false;
            }

            return true;
        }

        #endregion


        #region TASK 4

        static void ReplaceCharacters() {
            Console.Write("Введіть текст: ");
            string text = Console.ReadLine() ?? "";

            char[] characters = text.ToCharArray();

            for (int i = 0; i < characters.Length; i++) {
                int position = i + 1;

                // Кожен 15-й символ одночасно кратний 3 і 5,
                // тому перевіряємо 15 першим.
                if (position % 15 == 0) {
                    characters[i] = '?';
                } else if (position % 3 == 0) {
                    characters[i] = 'X';
                } else if (position % 5 == 0) {
                    characters[i] = '9';
                }
            }

            Console.WriteLine("Результат: " + new string(characters));
        }

        #endregion


        #region HELPERS

        // Перевіряє ціле число будь-якої довжини.
        // Саме тому завдання 1 не обмежується типами int або long.
        static bool TrySplitInteger(string text, out char sign, out string digits) {
            sign = '\0';
            digits = "";

            if (text.Length == 0) {
                return false;
            }

            int start = 0;

            if (text[0] == '-' || text[0] == '+') {
                sign = text[0];
                start = 1;

                if (text.Length == 1) {
                    return false;
                }
            }

            for (int i = start; i < text.Length; i++) {
                if (text[i] < '0' || text[i] > '9') {
                    return false;
                }
            }

            digits = text.Substring(start);
            return true;
        }


        static int ReadNonNegativeInteger(string message) {
            while (true) {
                Console.Write(message);
                string input = (Console.ReadLine() ?? "").Trim();

                int value;

                if (int.TryParse(input, out value) && value >= 0) {
                    return value;
                }

                Console.WriteLine("Помилка: введіть невід'ємне ціле число.");
            }
        }

        #endregion
    }
}
