namespace Apps.Zendesk.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> WhereEquals<T>(
        this IEnumerable<T> source, 
        string? value, 
        Func<T, string> selector)
    {
        if (string.IsNullOrWhiteSpace(value))
            return source;

        return source.Where(a => string.Equals(selector(a), value, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<T> WhereEquals<T>(
        this IEnumerable<T> source,
        bool? value,
        Func<T, bool> selector)
    {
        if (!value.HasValue)
            return source;

        return source.Where(a => selector(a) == value.Value);
    }

    public static IEnumerable<T> WhereContains<T>(
        this IEnumerable<T> source,
        IEnumerable<string>? values,
        Func<T, string> selector)
    {
        if (values is null || !values.Any())
            return source;

        return source.Where(a => values.Contains(selector(a)));
    }

    public static IEnumerable<T> WhereAnyContains<T>(
        this IEnumerable<T> source,
        string? query,
        params Func<T, string?>[] stringSelectors)
    {
        if (string.IsNullOrWhiteSpace(query))
            return source;

        return source.Where(item =>
            stringSelectors.Any(selector =>
            {
                var value = selector(item);
                return !string.IsNullOrEmpty(value) && value.Contains(query, StringComparison.InvariantCultureIgnoreCase);
            })
        );
    }

    public static IEnumerable<T> WhereIntersects<T, TValue>(
        this IEnumerable<T> source,
        IEnumerable<TValue>? targetValues,
        Func<T, IEnumerable<TValue>?> selector)
    {
        if (targetValues == null || !targetValues.Any())
            return source;

        return source.Where(a =>
        {
            var itemValues = selector(a);
            return itemValues != null && itemValues.Intersect(targetValues).Any();
        });
    }

    public static IEnumerable<T> WhereDateAfter<T>(
        this IEnumerable<T> source,
        DateTime? targetDate,
        Func<T, DateTime> dateSelector)
    {
        if (!targetDate.HasValue) 
            return source;

        return source.Where(a => dateSelector(a) >= targetDate.Value);
    }

    public static IEnumerable<T> WhereDateBefore<T>(
        this IEnumerable<T> source,
        DateTime? targetDate,
        Func<T, DateTime> dateSelector)
    {
        if (!targetDate.HasValue) 
            return source;

        return source.Where(a => dateSelector(a) <= targetDate.Value);
    }

    public static IEnumerable<T> WhereDateIs<T>(
        this IEnumerable<T> source,
        DateTime? targetDate,
        Func<T, DateTime> dateSelector)
    {
        if (!targetDate.HasValue) 
            return source;

        return source.Where(a => dateSelector(a).Date == targetDate.Value.Date);
    }
}
