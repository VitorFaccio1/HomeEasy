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
}