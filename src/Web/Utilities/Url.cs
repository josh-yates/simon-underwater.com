namespace Web.Utilities
{
    public static class Url
    {
        public static string Combine(string section1, string section2, bool prefixSlash = true)
        {
            var url = $"{section1.Trim('/')}/{section2.Trim('/')}";

            return prefixSlash ? "/" + url : url;
        }
    }
}