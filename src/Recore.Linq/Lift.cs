using System;
using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Converts an action operating on a scalar value to an action operating on a sequence of values.
        /// </summary>
        public static Action<IEnumerable<T>> Lift<T>(Action<T> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return enumerable => enumerable.ForEach(action);
        }

        /// <summary>
        /// Converts a function operating on a scalar value to a function operating on a sequence of values.
        /// </summary>
        public static Func<IEnumerable<T>, IEnumerable<TResult>> Lift<T, TResult>(Func<T, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return enumerable => enumerable.Select(func);
        }
    }
}
