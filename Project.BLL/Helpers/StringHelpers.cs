using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Project.BLL.Helpers
{
    public static class StringHelpers
    {
        public static IHtmlContent GetInitials(this IHtmlHelper html, string fullname)
        {
            string[] words = fullname.Split();
            StringBuilder initials = new StringBuilder();

            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    initials.Append(char.ToUpper(word[0]));
                }
            }

            return new HtmlString(initials.ToString());
        }
    }
}