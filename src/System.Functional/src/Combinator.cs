using System;

namespace Recore.Functional
{
    public static class Combinator
    {
        // Entry point for these functions
        // Why Curry?  You could just call this Func or Create
        // I started with a helper method called Func since it's a syntax error in C#
        // to invoke these extensions on a naked method (unfortunately0
        // Returning curried functions makes this a fluid API so you can chain calls
        // This *greatly* reduces the burden from having to type out all the return types (you can't use var on raw methods)
        // All of the functions here will take curried functions as arguments for consistency,
        // But we could overload them if we want
        public static Func<A, Func<B, C>> Curry<A, B, C>(Func<A, B, C> f) => a => b => f(a, b);

        // B combinator (aka function composition)
        public static Func<A, C> Then<A, B, C>(this Func<A, B> f, Func<B, C> g) => a => g(f(a));

        // T combinator
        public static Func<Func<A, B>, B> ApplyTo<A, B>(A a) => (Func<A, B> f) => f(a);

        /// <summary>
        /// C combinator
        /// </summary>
        public static Func<B, Func<A, C>> Flip<A, B, C>(this Func<A, Func<B, C>> f) => b => a => f(a)(b);

        /// <summary>
        /// Psi combinator
        /// Map a binary function's arguments to another type
        /// </summary>
        public static Func<Func<B, A>, Func<B, Func<B, C>>> On<A, B, C>(this Func<A, Func<A, C>> f) => g => x => y => f(g(x))(g(y));
    }
}