using System.Linq;
using System.Text;

namespace RemindMe.Core.Extensions
{
    public static class StringExtensions
    {
        // https://stackoverflow.com/questions/206717/how-do-i-replace-multiple-spaces-with-a-single-space-in-c/16776096#16776096

        public static string MergeWhiteSpaces(this string str)
        {
            if (str == null)
            {
                return null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(str.Length);

                int i = 0;
                foreach (char c in str)
                {
                    if (c != ' ' || i == 0 || str[i - 1] != ' ')
                        stringBuilder.Append(c);
                    i++;
                }
                return stringBuilder.ToString();
            }
        }

        public static string UpperFirstCharIfPossible(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.First().ToString().ToUpper() + input.Substring(1);
            }

            return input;
        }
    }
}
