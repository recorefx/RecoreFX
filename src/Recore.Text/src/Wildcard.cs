using System.Linq;

namespace Recore.Text
{
    /// <summary>
    /// Represents a text pattern with <c>*</c> and <c>?<c> wildcard characters.
    /// </summary>
    /// <remarks>
    /// In evaluating matches for the pattern, <c>*</c> will match zero or more characters
    /// while <c>?</c> will match a single character.
    /// </remarks>
    public sealed class Wildcard
    {
        /// <summary>
        /// Gets the pattern that was passed to the <c cref="Wildcard">Wildcard</c> constructor.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Initializes a new instance of the <c cref="Wildcard">Wildcard</c> type with the specified pattern.
        /// </summary>
        public Wildcard(string pattern)
        {
            Pattern = pattern;
        }

        /// <summary>
        /// Determines whether a string matches the wildcard pattern.
        /// </summary>
        public bool IsMatch(string text)
        {
            bool expandingStar = false;

            int i = 0;
            int j = 0;
            while (i < Pattern.Length)
            {
                if (expandingStar)
                {
                    // Skip ahead to all possible positions and start matching again
                    var subpattern = new Wildcard(Pattern.Substring(i));
                    return Enumerable.Range(j, text.Length - 1)
                        .Select(x => text.Substring(x))
                        .Any(subpattern.IsMatch);
                }
                else
                {
                    char patternChar = Pattern[i];

                    if (patternChar == '*')
                    {
                        expandingStar = true;
                        i++;
                    }
                    else if (j < text.Length && (patternChar == '?' || patternChar == text[j]))
                    {
                        i++;
                        j++;
                    }
                    else
                    {
                        // Pattern did not match
                        return false;
                    }
                }
            }

            // Reached the end of the pattern
            // Match iff we have evaluated all of the input text
            // or the last character in the pattern is '*'
            return j == text.Length || expandingStar;
        }
    }
}
