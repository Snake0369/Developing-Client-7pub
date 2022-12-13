using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Core.Extensions
{
    public static class StringExtensions
    {
        public static string GetCommentIds(this string input)
        {
            var fields = input.Split(";");
            {
                if (fields.Length >= 3)
                {
                    return string.Join(";", fields.Take(3));
                }
            }
            return input;
        }
    }
}
