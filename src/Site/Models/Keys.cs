using System;

namespace Site.Keys
{
    public static class ImageDataKeys
    {
        public static readonly string TakenAt = nameof(TakenAt);
    }

    public static class PaginationKeys
    {
        public static readonly string TotalItems = nameof(TotalItems);
    }

    public static class RazorKeys
    {
        public static readonly string CssBuildId = Guid
            .NewGuid()
            .ToString()
            .Replace("-", string.Empty);
    }
}