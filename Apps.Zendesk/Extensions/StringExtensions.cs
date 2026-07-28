namespace Apps.Zendesk.Extensions;

public static class StringExtensions
{
    public static bool IsMismatchWith(this string? filter, string? actual)
    {
        return !string.IsNullOrWhiteSpace(filter) && !string.Equals(filter, actual, StringComparison.OrdinalIgnoreCase);
    }
}