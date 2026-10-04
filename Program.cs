using System.Globalization;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

try
{
    while (true)
    {
        Console.WriteLine("\n1 — Змінити порядок цифр числа");
        Console.WriteLine("2 — Видалити перші X слів");
        Console.WriteLine("3 — Обчислити площу фігури");
        Console.WriteLine("4 — Замінити кожен 3-й та 5-й символ");
        Console.WriteLine("0 — Вийти");
        Console.Write("Оберіть завдання: ");
        string? choice = Console.ReadLine();
        if (choice is null or "0") break;

        try
        {
            switch (choice)
            {
                case "1": ReverseNumber(); break;
                case "2": RemoveWords(); break;
                case "3": CalculateArea(); break;
                case "4": ReplaceCharacters(); break;
                default: Console.WriteLine("Оберіть число від 0 до 4."); break;
            }
        }
        catch (OverflowException)
        {
            Console.WriteLine("Значення завелике для обчислення площі.");
        }
    }
}
catch (EndOfStreamException)
{
    // Завершуємо програму, якщо потік введення закрито.
}

static string ReadText(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? throw new EndOfStreamException();
}

static void ReverseNumber()
{
    long number;
    while (true)
    {
        string input = ReadText("Введіть ціле число (щонайменше 5 цифр): ");
        if (long.TryParse(input, out number) && (number <= -10000 || number >= 10000)) break;
        Console.WriteLine("Потрібне ціле число щонайменше з 5 цифр у діапазоні Int64.");
    }

    // Decimal вміщує обернені 19 цифр навіть при перевищенні діапазону Int64.
    // Від'ємні остачі зберігають знак, зокрема для long.MinValue.
    decimal reversed = 0;
    while (number != 0)
    {
        reversed = reversed * 10 + number % 10;
        number /= 10;
    }
    Console.WriteLine($"Результат: {reversed.ToString(CultureInfo.InvariantCulture)}");
}

static void RemoveWords()
{
    string text = ReadText("Введіть текст: ");
    int count;
    while (!int.TryParse(ReadText("Скільки перших слів видалити? "), out count) || count < 0)
        Console.WriteLine("Введіть невід'ємне ціле число.");

    int index = 0;
    for (int removed = 0; removed < count && index < text.Length; removed++)
    {
        while (index < text.Length && char.IsWhiteSpace(text[index])) index++;
        while (index < text.Length && !char.IsWhiteSpace(text[index])) index++;
    }
    if (count > 0)
        while (index < text.Length && char.IsWhiteSpace(text[index])) index++;

    Console.WriteLine($"Результат: {text.Substring(index)}");
}

static decimal ReadPositiveNumber(string prompt)
{
    while (true)
    {
        string input = ReadText(prompt).Trim().Replace(',', '.');
        if (decimal.TryParse(input, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out decimal value) && value > 0) return value;
        Console.WriteLine("Введіть додатне число. Десятковий роздільник — крапка або кома.");
    }
}

static void CalculateArea()
{
    Console.WriteLine("1 — Квадрат; 2 — Круг; 3 — Прямокутник; 4 — Трикутник");
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
    Console.WriteLine($"Площа: {area.ToString("0.############################", CultureInfo.InvariantCulture)}");
}

static void ReplaceCharacters()
{
    char[] characters = ReadText("Введіть текст: ").ToCharArray();
    for (int i = 0; i < characters.Length; i++)
    {
        int position = i + 1;
        if (position % 15 == 0) characters[i] = '?';
        else if (position % 3 == 0) characters[i] = 'X';
        else if (position % 5 == 0) characters[i] = '9';
    }
    Console.WriteLine($"Результат: {new string(characters)}");
}
