using System.Text.RegularExpressions;

namespace Project3.Library;

public static class Acceptable
{
    public const string PROJECT_ID_VALID = @"^PROJ\d{2}$";
    public const string DEVELOPER_ID_VALID = @"^DEV\d{3}$";
    public const string NAME_VALID = @"^[a-zA-Z]+(\s+[a-zA-Z]+)+$";
    public const string INTEGER_VALID = @"\d+";
    public const string POSITIVE_INT_VALID = @"^[1-9]\d*$";
    public const string PROJECT_NAME_VALID = @"^[a-zA-Z0-9][a-zA-Z0-9\s\-]*$";
    public const string FILE = @"([^,]+),\s*([^,]+),\s*\[(.*?)\]\,\s*(\d+)";

    // Hỗ trợ cả tên hoa và thường để tương thích
    public const string file = FILE;

    public static bool IsValid(string? data, string? pattern)
    {
        if (data == null || pattern == null)
        {
            return false;
        }

        // Java matches() so khớp toàn bộ chuỗi
        string anchoredPattern = pattern;
        if (!pattern.StartsWith("^"))
        {
            anchoredPattern = "^" + anchoredPattern;
        }
        if (!pattern.EndsWith("$"))
        {
            anchoredPattern = anchoredPattern + "$";
        }

        try
        {
            return Regex.IsMatch(data, anchoredPattern);
        }
        catch
        {
            return false;
        }
    }

    public static bool isValid(string? data, string? pattern) => IsValid(data, pattern);
}
