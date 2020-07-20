using System.Linq;

namespace Recore.Text
{
    /// <summary>
    /// Represents a text pattern with <c>*</c> and <c>?</c> wildcard characters.
    /// </summary>
    /// <remarks>
    /// In evaluating matches for the pattern, <c>*</c> will match zero or more characters
    /// while <c>?</c> will match a single character.
    /// </remarks>
    public sealed class GlobExpression
    {
        /// <summary>
        /// Gets the pattern that was passed to the <see cref="GlobExpression"/> constructor.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobExpression"/> type with the specified pattern.
        /// </summary>
        public GlobExpression(string pattern)
        {
            Pattern = pattern;
        }

        /// <summary>
        /// Determines whether a string matches the wildcard pattern.
        /// </summary>
        public bool IsMatch(string text)
        {
            bool expandingStar = false;

            int patternIndex = 0;
            int textIndex = 0;
            while (patternIndex < Pattern.Length)
            {
                if (expandingStar)
                {
                    // Skip ahead to all possible positions in `text` and start matching again.
                    var subpattern = new GlobExpression(Pattern.Substring(patternIndex));
                    return Enumerable.Range(textIndex, count: text.Length - textIndex)
                        .Select(x => text.Substring(x))
                        .Any(subpattern.IsMatch);
                }
                else
                {
                    char patternChar = Pattern[patternIndex];

                    bool isEscape = false;
                    if (patternChar == '\\')
                    {
                        // If the next pattern character is a wildcard, skip to it and escape it.
                        // Otherwise, fall through to see if it matches the current text character.
                        if (patternIndex + 1 < Pattern.Length)
                        {
                            char nextPatternChar = Pattern[patternIndex + 1];
                            if (nextPatternChar == '*' || nextPatternChar == '?')
                            {
                                patternIndex++;
                                patternChar = nextPatternChar;
                                isEscape = true;
                            }
                        }
                    }

                    if (patternChar == '*' && !isEscape)
                    {
                        // Enter into the `expandingStar` state.
                        expandingStar = true;
                        patternIndex++;
                    }
                    else if (textIndex < text.Length && (patternChar == text[textIndex] || patternChar == '?' && !isEscape))
                    {
                        // The current pattern character and current text character match.
                        patternIndex++;
                        textIndex++;
                        isEscape = false;
                    }
                    else
                    {
                        // The pattern did not match.
                        return false;
                    }
                }
            }

            // Reached the end of the pattern
            // Match iff we have evaluated all of the input text
            // or the last character in the pattern is '*'
            return textIndex == text.Length || expandingStar;
        }
    }
}
