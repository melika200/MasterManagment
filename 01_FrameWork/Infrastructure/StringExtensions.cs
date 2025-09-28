using System.Text.RegularExpressions;

namespace _01_FrameWork.Infrastructure;

public static class StringExtensions
{
    public static string RemoveParentheses(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return text.Replace("(", "").Replace(")", "");
    }

    public static string RemoveParenthesesWithTextInside(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return Regex.Replace(text, @"\s*\([^)]*\)", "");
    }
    public static string RemoveCommas(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return Regex.Replace(text, @"[,]", "");
    }
    public static string RemoveDecimals(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return Regex.Replace(text, @"\.\d*", "");
    }

    public static string? Normalize_FullPersianTextAndNumbers(this string? text)
    {
        text = NormalizePersianNumbers(text);
        return NormalizePersianText(text);
    }


    public static string? Normalize_FullPersianTextAndNumbersAndRemoveHA(this string? text)
    {
        text = NormalizePersianNumbers(text);
        text = text?.Replace(" های", "").Replace(" ها", "");
        return NormalizePersianText(text);
    }
    public static string? Normalize_PersianNumbers(this string? text)
    {
        return NormalizePersianNumbers(text);
    }

    private static string? NormalizePersianNumbers(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        text = text.Replace("\u200c", " ")
            .Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4")
            .Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        text = new string(text.Where(c => !char.IsControl(c)).ToArray());
        return text;
    }
    private static string? NormalizePersianText(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        text = text.Replace("\u200c", " ").Replace('ي', 'ی').Replace('ك', 'ک').Replace('أ', 'ا').Replace("ء", "");
        text = new string(text.Where(c => !char.IsControl(c)).ToArray());
        return text;
    }

    private static string NormalizeDoubleSpace(string text)
    {
        while (text.Contains("  "))
            text = text.Replace("  ", " ");
        if (!string.IsNullOrEmpty(text))
            text = text.Trim();
        return text;
    }

    public static string TrimToNumber(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var match = Regex.Match(text, @"-?\d+(\.\d+)?");

        return match.Success ? match.Value : "";
    }

    public static string ToEnglish(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        text = text
            .Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4")
            .Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        return text;
    }
    public static string ToPersian(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        text = text
            .Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴")
            .Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
        return text;
    }

    public static string? ToPureTitle(this string? text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        if (!text.Contains('-'))
            return text;

        var index = text.IndexOf("-") + 1;
        var res = text.Remove(0, index);
        return res;
    }

    public static bool IsValidPhone(this string? text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        const string pattern = @"^09[0|1|2|3|9][0-9]{8}$";
        Regex reg = new Regex(pattern);
        return reg.IsMatch(text);
    }
}
