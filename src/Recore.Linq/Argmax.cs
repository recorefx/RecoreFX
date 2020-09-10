using System;
using System.Collections.Generic;
// TODO https://github.com/recorefx/RecoreFX/issues/24
//using System.Diagnostics.CodeAnalysis;

using Recore.Properties;

namespace Recore.Linq
{
    // Adapted from https://github.com/dotnet/runtime/blob/809a06f45161ae686a06b9e9ccc2f45097b91657/src/libraries/System.Linq/src/System/Linq/Max.cs
    public static partial class Renumerable
    {
        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of <see cref="int"/> values.
        /// </summary>
        public static (int Argmax, int Max) Argmax(this IEnumerable<int> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int value;
            using (IEnumerator<int> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    int x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of nullable <see cref="int"/> values.
        /// </summary>
        public static (int Argmax, int? Max) Argmax(this IEnumerable<int?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int? value = null;
            using (IEnumerator<int?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                int valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    // We can fast-path this case where we know HasValue will
                    // never affect the outcome, without constantly checking
                    // if we're in such a state. Similar fast-paths could
                    // be done for other cases, but as all-positive
                    // or mostly-positive integer values are quite common in real-world
                    // uses, it's only been done in this direction for int? and long?.
                    while (e.MoveNext())
                    {
                        int? cur = e.Current;
                        int x = cur.GetValueOrDefault();
                        if (x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                else
                {
                    while (e.MoveNext())
                    {
                        int? cur = e.Current;
                        int x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of <see cref="long"/> values.
        /// </summary>
        public static (int Argmax, long Max) Argmax(this IEnumerable<long> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long value;
            using (IEnumerator<long> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    long x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of nullable <see cref="long"/> values.
        /// </summary>
        public static (int Argmax, long? Max) Argmax(this IEnumerable<long?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long? value = null;
            using (IEnumerator<long?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                long valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    while (e.MoveNext())
                    {
                        long? cur = e.Current;
                        long x = cur.GetValueOrDefault();
                        if (x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                else
                {
                    while (e.MoveNext())
                    {
                        long? cur = e.Current;
                        long x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of <see cref="double"/> values.
        /// </summary>
        public static (int Argmax, double Max) Argmax(this IEnumerable<double> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double value;
            using (IEnumerator<double> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;

                // As described in a comment on Min(this IEnumerable<double>) NaN is ordered
                // less than all other values. We need to do explicit checks to ensure this, but
                // once we've found a value that is not NaN we need no longer worry about it,
                // so first loop until such a value is found (or not, as the case may be).
                while (double.IsNaN(value))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }

                while (e.MoveNext())
                {
                    double x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of nullable <see cref="double"/> values.
        /// </summary>
        public static (int Argmax, double? Max) Argmax(this IEnumerable<double?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double? value = null;
            using (IEnumerator<double?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                double valueVal = value.GetValueOrDefault();
                while (double.IsNaN(valueVal))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    double? cur = e.Current;
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (e.MoveNext())
                {
                    double? cur = e.Current;
                    double x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x > valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of <see cref="float"/> values.
        /// </summary>
        public static (int Argmax, float Max) Argmax(this IEnumerable<float> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            float value;
            using (IEnumerator<float> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (float.IsNaN(value))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }

                while (e.MoveNext())
                {
                    float x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of nullable <see cref="float"/> values.
        /// </summary>
        public static (int Argmax, float? Max) Argmax(this IEnumerable<float?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            float? value = null;
            using (IEnumerator<float?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                float valueVal = value.GetValueOrDefault();
                while (float.IsNaN(valueVal))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    float? cur = e.Current;
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (e.MoveNext())
                {
                    float? cur = e.Current;
                    float x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x > valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of <see cref="decimal"/> values.
        /// </summary>
        public static (int Argmax, decimal Max) Argmax(this IEnumerable<decimal> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            decimal value;
            using (IEnumerator<decimal> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    decimal x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of nullable <see cref="decimal"/> values.
        /// </summary>
        public static (int Argmax, decimal? Max) Argmax(this IEnumerable<decimal?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            decimal? value = null;
            using (IEnumerator<decimal?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                decimal valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    decimal? cur = e.Current;
                    decimal x = cur.GetValueOrDefault();
                    if (cur.HasValue && x > valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value from a sequence of values.
        /// </summary>
        // TODO https://github.com/recorefx/RecoreFX/issues/24
        //[return: MaybeNull]
        public static (TSource Argmax, (int Argmax, TSource Max) Max) Argmax<TSource>(this IEnumerable<TSource> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Comparer<TSource> comparer = Comparer<TSource>.Default;
            TSource value = default!;
            if (value == null)
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    do
                    {
                        if (!e.MoveNext())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (value == null);

                    while (e.MoveNext())
                    {
                        TSource x = e.Current;
                        if (x != null && comparer.Compare(x, value) > 0)
                        {
                            value = x;
                        }
                    }
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        throw new InvalidOperationException(Resources.NoElements);
                    }

                    value = e.Current;
                    while (e.MoveNext())
                    {
                        TSource x = e.Current;
                        if (comparer.Compare(x, value) > 0)
                        {
                            value = x;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning <see cref="int"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, int Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            int value;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = selector(e.Current);
                while (e.MoveNext())
                {
                    int x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning nullable <see cref="int"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, int? Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            int? value = null;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                int valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    // We can fast-path this case where we know HasValue will
                    // never affect the outcome, without constantly checking
                    // if we're in such a state. Similar fast-paths could
                    // be done for other cases, but as all-positive
                    // or mostly-positive integer values are quite common in real-world
                    // uses, it's only been done in this direction for int? and long?.
                    while (e.MoveNext())
                    {
                        int? cur = selector(e.Current);
                        int x = cur.GetValueOrDefault();
                        if (x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                else
                {
                    while (e.MoveNext())
                    {
                        int? cur = selector(e.Current);
                        int x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning <see cref="long"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, long Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            long value;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = selector(e.Current);
                while (e.MoveNext())
                {
                    long x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning nullable <see cref="long"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, long? Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            long? value = null;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                long valueVal = value.GetValueOrDefault();
                if (valueVal >= 0)
                {
                    while (e.MoveNext())
                    {
                        long? cur = selector(e.Current);
                        long x = cur.GetValueOrDefault();
                        if (x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
                else
                {
                    while (e.MoveNext())
                    {
                        long? cur = selector(e.Current);
                        long x = cur.GetValueOrDefault();

                        // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                        // unless nulls either never happen or always happen.
                        if (cur.HasValue & x > valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning <see cref="float"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, float Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            float value;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = selector(e.Current);
                while (float.IsNaN(value))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }

                while (e.MoveNext())
                {
                    float x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning nullable <see cref="float"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, float? Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            float? value = null;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                float valueVal = value.GetValueOrDefault();
                while (float.IsNaN(valueVal))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    float? cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (e.MoveNext())
                {
                    float? cur = selector(e.Current);
                    float x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x > valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning <see cref="double"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, double Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            double value;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = selector(e.Current);

                // As described in a comment on Min(this IEnumerable<double>) NaN is ordered
                // less than all other values. We need to do explicit checks to ensure this, but
                // once we've found a value that is not NaN we need no longer worry about it,
                // so first loop until such a value is found (or not, as the case may be).
                while (double.IsNaN(value))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }

                while (e.MoveNext())
                {
                    double x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning nullable <see cref="double"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, double? Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            double? value = null;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                double valueVal = value.GetValueOrDefault();
                while (double.IsNaN(valueVal))
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    double? cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        valueVal = (value = cur).GetValueOrDefault();
                    }
                }

                while (e.MoveNext())
                {
                    double? cur = selector(e.Current);
                    double x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x > valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning <see cref="decimal"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, decimal Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            decimal value;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = selector(e.Current);
                while (e.MoveNext())
                {
                    decimal x = selector(e.Current);
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function returning nullable <see cref="decimal"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmax, decimal? Max) Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            decimal? value = null;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = selector(e.Current);
                }
                while (!value.HasValue);

                decimal valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    decimal? cur = selector(e.Current);
                    decimal x = cur.GetValueOrDefault();
                    if (cur.HasValue && x > valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function on a sequence of values.
        /// </summary>
        // TODO https://github.com/recorefx/RecoreFX/issues/24
        //[return: MaybeNull]
        public static (TSource Argmax, TResult Max) Argmax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            Comparer<TResult> comparer = Comparer<TResult>.Default;
            TResult value = default!;
            if (value == null)
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    do
                    {
                        if (!e.MoveNext())
                        {
                            return value;
                        }

                        value = selector(e.Current);
                    }
                    while (value == null);

                    while (e.MoveNext())
                    {
                        TResult x = selector(e.Current);
                        if (x != null && comparer.Compare(x, value) > 0)
                        {
                            value = x;
                        }
                    }
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        throw new InvalidOperationException(Resources.NoElements);
                    }

                    value = selector(e.Current);
                    while (e.MoveNext())
                    {
                        TResult x = selector(e.Current);
                        if (comparer.Compare(x, value) > 0)
                        {
                            value = x;
                        }
                    }
                }
            }

            return value;
        }
    }
}