using System;

namespace Recore.Functional
{
    /// <summary>
    /// Defines functions corresponding to C# operators.
    /// </summary>
    public static class Operator
    {
        /// <summary>
        /// Returns the logical negation of a boolean value.
        /// </summary>
        public static bool Not(bool value) => !value;

        /// <summary>
        /// Returns the logical negation of a predicate function.
        /// </summary>
        public static Func<T1, bool> Not<T1>(Func<T1, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return x => !predicate(x);
        }
    }
}