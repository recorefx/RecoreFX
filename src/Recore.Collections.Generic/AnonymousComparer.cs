using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    public sealed class AnonymousComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> compare;

        public AnonymousComparer(Func<T, T, int> compare)
        {
            this.compare = compare;
        }

        public int Compare(T x, T y) => compare(x, y);
    }
}
