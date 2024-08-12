namespace HomeEasy.Extensions;

public static class StringExtensions
{
    public static string NormalizePhoneNumber(this string phoneNumber) =>
        string.IsNullOrEmpty(phoneNumber)
            ? phoneNumber
            : phoneNumber.Replace("+", "")
                          .Replace(" ", "")
                          .Replace("-", "")
                          .Replace("(", "")
                          .Replace(")", "");

    public static string FormatPhoneNumber(this string phoneNumber) =>
        string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 13
            ? phoneNumber
            : $"+{phoneNumber[..2]} ({phoneNumber.Substring(2, 2)}) {phoneNumber.Substring(4, 5)}-{phoneNumber[9..]}";
}