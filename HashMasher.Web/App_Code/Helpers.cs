using System.Web.Mvc;

namespace HashMasher.Web
{
    public static class Helpers
    {
        public static string Truncate(this HtmlHelper helper, string input, int length)
        {
            if (input == null || string.IsNullOrWhiteSpace(input))
                return string.Empty;

            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length) + "...";
            }
        }
    }
}