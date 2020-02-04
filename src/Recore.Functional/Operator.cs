using System;

namespace Recore.Functional
{
    /// <summary>
    /// Defines functions corresponding to C# operators.
    /// </summary>
    public static class Operator
    {
        Func<bool> Not(Func<bool> predicate)
            => () => !predicate();

        Func<T1, bool> Not<T1>(Func<T1, bool> predicate)
            => t1 => !predicate(t1);
    }
}