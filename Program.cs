using System.Globalization;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

try
{
    while (true)
    {
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

        string? choice = Console.ReadLine();
        if (choice is null or "0")
            break;

        try
        {
            switch (choice)
            {
                case "1":
                    ReverseNumber();
                    break;
                case "2":
                    RemoveWords();
                    break;
                case "3":
                    CalculateArea();
                    break;
                case "4":
                    ReplaceCharacters();
                    break;
                default:
                    Console.WriteLine("Оберіть число від 0 до 4.");
                    break;
            }
        }
        catch (OverflowException)
        {
            Console.WriteLine("Значення завелике для обчислення.");
        }
    }
}
catch (EndOfStreamException)
{
    // Коректне завершення програми, якщо потік введення закрито.
}

static string ReadText(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? throw new EndOfStreamException();
}

static void ReverseNumber()
{
    while (true)
    {
        string input = ReadText("Введіть ціле число будь-якої довжини: ").Trim();

        if (!TryReadIntegerText(input, out char sign, out string digits))
        {
            Console.WriteLine("Помилка: введіть ціле число без пробілів та інших символів.");
            continue;
        }

        if (digits.Length == 1)
        {
            Console.WriteLine("Попередження: введено лише одну цифру — змінювати порядок цифр немає сенсу.");
            Console.WriteLine($"Результат: {FormatSignedNumber(sign, digits)}");
            return;
        }

        char[] reversedCharacters = digits.ToCharArray();
        Array.Reverse(reversedCharacters);

        string reversedDigits = new string(reversedCharacters).TrimStart('0');
        if (reversedDigits.Length == 0)
            reversedDigits = "0";

        Console.WriteLine($"Результат: {FormatSignedNumber(sign, reversedDigits)}");
        return;
    }
}

static bool TryReadIntegerText(string input, out char sign, out string digits)
{
    sign = '\0';
    digits = "";

    if (input.Length == 0)
        return false;

    int start = 0;

    if (input[0] == '-' || input[0] == '+')
    {
        sign = input[0];
        start = 1;

        if (input.Length == 1)
            return false;
    }

    for (int i = start; i < input.Length; i++)
    {
        if (input[i] < '0' || input[i] > '9')
            return false;
    }

    digits = input.Substring(start).TrimStart('0');

    if (digits.Length == 0)
    {
        digits = "0";
        sign = '\0';
    }

    return true;
}

static string FormatSignedNumber(char sign, string digits)
{
    if (digits == "0" || sign == '\0')
        return digits;

    return sign + digits;
}

static void RemoveWords()
{
    string text = ReadText("Введіть текст: ");

    AnalyzeTextElements(text);

    int count;
    while (!int.TryParse(
               ReadText("Скільки перших слів/числових значень видалити? "),
               out count) ||
           count < 0)
    {
        Console.WriteLine("Введіть невід'ємне ціле число.");
    }

    int index = 0;

    for (int removed = 0; removed < count && index < text.Length; removed++)
    {
        while (index < text.Length && char.IsWhiteSpace(text[index]))
            index++;

        while (index < text.Length && !char.IsWhiteSpace(text[index]))
            index++;
    }

    if (count > 0)
    {
        while (index < text.Length && char.IsWhiteSpace(text[index]))
            index++;
    }

    Console.WriteLine($"Результат: {text.Substring(index)}");
}

static void AnalyzeTextElements(string text)
{
    int wordCount = 0;
    int numericCount = 0;
    int index = 0;

    while (index < text.Length)
    {
        while (index < text.Length && char.IsWhiteSpace(text[index]))
            index++;

        if (index >= text.Length)
            break;

        int start = index;

        while (index < text.Length && !char.IsWhiteSpace(text[index]))
            index++;

        string token = text.Substring(start, index - start);

        if (IsNumericToken(token))
        {
            numericCount++;
            Console.WriteLine(
                $"Попередження: \"{token}\" — це не слово, а числове значення. " +
                "Воно буде враховано як окремий елемент.");
        }
        else
        {
            wordCount++;
        }
    }

    Console.WriteLine(
        $"Розпізнано: слів — {wordCount}, числових значень — {numericCount}.");
}

static bool IsNumericToken(string token)
{
    string value = token.Trim(
        '(', ')', '[', ']', '{', '}', '"', '\'', ';', '!', '?', ':', '.', ',');

    if (value.Length == 0)
        return false;

    int start = 0;

    if (value[0] == '+' || value[0] == '-')
    {
        start = 1;

        if (value.Length == 1)
            return false;
    }

    bool hasDigit = false;
    bool hasSeparator = false;

    for (int i = start; i < value.Length; i++)
    {
        if (value[i] >= '0' && value[i] <= '9')
        {
            hasDigit = true;
            continue;
        }

        if ((value[i] == '.' || value[i] == ',') && !hasSeparator)
        {
            hasSeparator = true;
            continue;
        }

        return false;
    }

    return hasDigit;
}

static decimal ReadPositiveNumber(string prompt)
{
    while (true)
    {
        string input = ReadText(prompt).Trim().Replace(',', '.');

        if (decimal.TryParse(
                input,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out decimal value) &&
            value > 0)
        {
            return value;
        }

        Console.WriteLine("Введіть додатне число. Десятковий роздільник — крапка або кома.");
    }
}

static void CalculateArea()
{
    Console.WriteLine("1 — Квадрат");
    Console.WriteLine("2 — Круг");
    Console.WriteLine("3 — Прямокутник");
    Console.WriteLine("4 — Трикутник");

    string figure = ReadText("Оберіть фігуру: ");
    decimal area;

    switch (figure)
    {
        case "1":
            decimal side = ReadPositiveNumber("Введіть сторону квадрата: ");
            area = side * side;
            break;

        case "2":
            decimal radius = ReadPositiveNumber("Введіть радіус круга: ");
            area = (decimal)Math.PI * radius * radius;
            break;

        case "3":
            decimal width = ReadPositiveNumber("Введіть першу сторону: ");
            decimal length = ReadPositiveNumber("Введіть другу сторону: ");
            area = width * length;
            break;

        case "4":
            decimal basis = ReadPositiveNumber("Введіть основу трикутника: ");
            decimal height = ReadPositiveNumber("Введіть висоту трикутника: ");
            area = basis * height / 2;
            break;

        default:
            Console.WriteLine("Невідома фігура. Оберіть число від 1 до 4.");
            return;
    }

    Console.WriteLine(
        $"Площа: {area.ToString("0.############################", CultureInfo.InvariantCulture)}");
}

static void ReplaceCharacters()
{
    char[] characters = ReadText("Введіть текст: ").ToCharArray();

    for (int i = 0; i < characters.Length; i++)
    {
        int position = i + 1;

        if (position % 15 == 0)
            characters[i] = '?';
        else if (position % 3 == 0)
            characters[i] = 'X';
        else if (position % 5 == 0)
            characters[i] = '9';
    }

    Console.WriteLine($"Результат: {new string(characters)}");
}
