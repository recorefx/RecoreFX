using System.Linq;

namespace System.Text
{
    public class Wildcard
    {
        private readonly string pattern;

        public Wildcard(string pattern)
        {
            this.pattern = pattern;
        }

        public bool IsMatch(string text)
        {
            bool expandingStar = false;

            int i = 0;
            int j = 0;
            while (i < pattern.Length)
            {
                if (expandingStar)
                {
                    // Skip ahead to all possible positions and start matching again
                    var subpattern = new Wildcard(pattern.Substring(i));
                    return Enumerable.Range(j, text.Length - 1)
                        .Select(x => text.Substring(x))
                        .Any(subpattern.IsMatch);
                }
                else
                {
                    char patternChar = pattern[i];

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
