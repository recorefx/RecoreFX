using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    public sealed class AnonymousEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> getHashCode;

        public AnonymousEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals;
            this.getHashCode = getHashCode;
        }

        public bool Equals(T x, T y) => equals(x, y);

        public int GetHashCode(T obj) => getHashCode(obj);
}
