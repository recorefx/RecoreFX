using System;
using System.Collections.Generic;

namespace Recore
{
    /// <summary>
    /// Contains helper methods for working with strings.
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// Concatenates a sequence of strings into a single string
        /// where each input string is separated by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string JoinLines(IEnumerable<string> lines)
        {
            if (lines is null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Concatenates a variable number of string arguments into a single string
        /// where each input string is separated by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string JoinLines(params string[] lines)
            => JoinLines((IEnumerable<string>)lines);
    }
}
