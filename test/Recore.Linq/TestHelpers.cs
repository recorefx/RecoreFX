using System;
using System.Collections.Generic;

namespace Recore.Linq.Tests
{
    internal static class TestHelpers
    {
        /// <summary>
        /// A method that uses <c>yield return</c> won't evaluate its body until its result is enumerated.
        /// </summary>
        public static void ForceExecution<T>(Func<IEnumerable<T>> generator)
        {
            foreach (var x in generator())
            {
            }
        }
    }
}