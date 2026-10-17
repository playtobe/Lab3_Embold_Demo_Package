using System.Globalization;

namespace Project3.Library;

public static class Inputter
{
    private const string DateFormat = "dd/MM/yyyy";

    public static DateOnly? ToDateOnly(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return null;
        }

        if (DateOnly.TryParseExact(data.Trim(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }

        return null;
    }

    public static string FromDateOnly(DateOnly? date)
    {
        return date.HasValue ? date.Value.ToString(DateFormat) : string.Empty;
    }

    public static string DateToString(DateOnly? date) => FromDateOnly(date);

    public static string GetString(string mess)
    {
        Console.Write(mess);
        return Console.ReadLine() ?? string.Empty;
    }

    public static string InputString(string mess) => GetString(mess);

    public static string InputAndLoop(string mess, string pattern, bool isLoop)
    {
        string result;
        do
        {
            result = GetString(mess);
            bool isInvalid = !Acceptable.IsValid(result, pattern);
            if (isInvalid && result.Length > 0)
            {
                Console.WriteLine("Invalid! Please try again!");
                continue;
            }
            if (result.Length == 0 && isLoop)
            {
                Console.WriteLine("cannot blank");
                continue;
            }
            break;
        } while (true);

        return result.Trim();
    }

    public static DateOnly? InputDateOnly(string mess, bool isLoop)
    {
        while (true)
        {
            Console.Write($"{mess} (dd/MM/yyyy): ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(input))
            {
                if (!isLoop)
                {
                    return null;
                }
                Console.WriteLine("Input cannot be blank! Please try again");
                continue;
            }

            if (DateOnly.TryParseExact(input, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate;
            }

            Console.WriteLine("Invalid Date Format! Please follow dd/MM/yyyy");
        }
    }

    public static DateTime? GetDate(string mess, string pattern)
    {
        while (true)
        {
            string input = GetString(mess);
            if (DateTime.TryParseExact(input, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                return dt;
            }
            Console.WriteLine($"Invalid date! Please enter again (format: {pattern})");
        }
    }

    // Các hàm tương thích tên gốc từ Java
    public static DateOnly? toLocalDate(string? data) => ToDateOnly(data);
    public static string fromLocalDate(DateOnly? date) => FromDateOnly(date);
    public static string getString(string mess) => GetString(mess);
    public static string inputString(string mess) => InputString(mess);
    public static string inputAndLoop(string mess, string pattern, bool isLoop) => InputAndLoop(mess, pattern, isLoop);
    public static DateOnly? inputLocalDate(string mess, bool isLoop) => InputDateOnly(mess, isLoop);
    public static string dateToString(DateOnly? date) => DateToString(date);
    public static DateTime? getDate(string mess, string pattern) => GetDate(mess, pattern);
}
