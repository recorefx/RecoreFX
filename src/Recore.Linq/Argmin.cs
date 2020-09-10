using System;
using System.Collections.Generic;
// TODO https://github.com/recorefx/RecoreFX/issues/24
//using System.Diagnostics.CodeAnalysis;

using Recore.Properties;

namespace Recore.Linq
{
    // https://github.com/dotnet/runtime/blob/809a06f45161ae686a06b9e9ccc2f45097b91657/src/libraries/System.Linq/src/System/Linq/Min.cs
    public static partial class Renumerable
    {
        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of <see cref="int"/> values.
        /// </summary>
        public static (int Argmin, int Min) Argmin(this IEnumerable<int> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, int Item) value;
            using (var e = source.Enumerate().GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    if (e.Current.Item < value.Item)
                    {
                        value = e.Current;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of nullable <see cref="int"/> values.
        /// </summary>
        public static (int Argmin, int? Min) Argmin(this IEnumerable<int?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, int? Item) value = (0, null);
            using (var e = source.Enumerate().GetEnumerator())
            {
                // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.Item.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                int valueVal = value.Item.GetValueOrDefault();
                while (e.MoveNext())
                {
                    var cur = e.Current;
                    int x = cur.Item.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.Item.HasValue & x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of <see cref="long"/> values.
        /// </summary>
        public static (int Argmin, long Min) Argmin(this IEnumerable<long> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, long Item) value;
            using (var e = source.Enumerate().GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    if (e.Current.Item < value.Item)
                    {
                        value = e.Current;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of nullable <see cref="long"/> values.
        /// </summary>
        public static (int Argmin, long? Min) Argmin(this IEnumerable<long?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, long? Item) value = (0, null);
            using (var e = source.Enumerate().GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.Item.HasValue);

                long valueVal = value.Item.GetValueOrDefault();
                while (e.MoveNext())
                {
                    var cur = e.Current;
                    long x = cur.Item.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.Item.HasValue & x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of <see cref="float"/> values.
        /// </summary>
        public static (int Argmin, float Min) Argmin(this IEnumerable<float> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, float Item) value;
            using (var e = source.Enumerate().GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                if (float.IsNaN(value.Item))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    if (e.Current.Item < value.Item)
                    {
                        value = e.Current;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (float.IsNaN(e.Current.Item))
                    {
                        return e.Current;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of nullable <see cref="float"/> values.
        /// </summary>
        public static (int Argmin, float? Min) Argmin(this IEnumerable<float?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, float? Item) value = (0, null);
            using (var e = source.Enumerate().GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.Item.HasValue);

                float valueVal = value.Item.GetValueOrDefault();
                if (float.IsNaN(valueVal))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    var cur = e.Current;
                    if (cur.Item.HasValue)
                    {
                        float x = cur.Item.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (float.IsNaN(x))
                        {
                            return cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of <see cref="double"/> values.
        /// </summary>
        public static (int Argmin, double Min) Argmin(this IEnumerable<double> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, double Item) value;
            using (var e = source.Enumerate().GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                if (double.IsNaN(value.Item))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    if (e.Current.Item < value.Item)
                    {
                        value = e.Current;
                    }
                    else if (double.IsNaN(e.Current.Item))
                    {
                        return e.Current;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of nullable <see cref="double"/> values.
        /// </summary>
        public static (int Argmin, double? Min) Argmin(this IEnumerable<double?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, double? Item) value = (0, null);
            using (var e = source.Enumerate().GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.Item.HasValue);

                double valueVal = value.Item.GetValueOrDefault();
                if (double.IsNaN(valueVal))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    var cur = e.Current;
                    if (cur.Item.HasValue)
                    {
                        double x = cur.Item.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (double.IsNaN(x))
                        {
                            return cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of <see cref="decimal"/> values.
        /// </summary>
        public static (int Argmin, decimal Min) Argmin(this IEnumerable<decimal> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, decimal Item) value;
            using (var e = source.Enumerate().GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    if (e.Current.Item < value.Item)
                    {
                        value = e.Current;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of nullable <see cref="decimal"/> values.
        /// </summary>
        public static (int Argmin, decimal? Min) Argmin(this IEnumerable<decimal?> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (int Index, decimal? Item) value = (0, null);
            using (var e = source.Enumerate().GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.Item.HasValue);

                decimal valueVal = value.Item.GetValueOrDefault();
                while (e.MoveNext())
                {
                    var cur = e.Current;
                    decimal x = cur.Item.GetValueOrDefault();
                    if (cur.Item.HasValue && x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value from a sequence of values.
        /// </summary>
        // TODO https://github.com/recorefx/RecoreFX/issues/24
        //[return: MaybeNull]
        public static (int Argmin, TSource Min) Argmin<TSource>(this IEnumerable<TSource> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Comparer<TSource> comparer = Comparer<TSource>.Default;
            // TODO https://github.com/recorefx/RecoreFX/issues/24
            //TSource value = default!;
            (int Index, TSource Item) value = (0, default);
            if (value.Item == null)
            {
                using (var e = source.Enumerate().GetEnumerator())
                {
                    do
                    {
                        if (!e.MoveNext())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (value.Item == null);

                    while (e.MoveNext())
                    {
                        if (e.Current.Item != null && comparer.Compare(e.Current.Item, value.Item) < 0)
                        {
                            value = e.Current;
                        }
                    }
                }
            }
            else
            {
                using (var e = source.Enumerate().GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        throw new InvalidOperationException(Resources.NoElements);
                    }

                    value = e.Current;
                    while (e.MoveNext())
                    {
                        if (comparer.Compare(e.Current.Item, value.Item) < 0)
                        {
                            value = e.Current;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning <see cref="int"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, int Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            int value;
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                argminCandidate = e.Current;
                value = selector(e.Current);
                while (e.MoveNext())
                {
                    int x = selector(e.Current);
                    if (x < value)
                    {
                        argminCandidate = e.Current;
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning nullable <see cref="int"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, int? Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            int? value = null;
            using (var e = source.GetEnumerator())
            {
                // Start off knowing that we've a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    argminCandidate = e.Current;
                    value = selector(e.Current);
                }
                while (!value.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                int valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    int? cur = selector(e.Current);
                    int x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning <see cref="long"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, long Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            long value;
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                argminCandidate = e.Current;
                value = selector(e.Current);
                while (e.MoveNext())
                {
                    long x = selector(e.Current);
                    if (x < value)
                    {
                        argminCandidate = e.Current;
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning nullable <see cref="long"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, long? Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            long? value = null;
            using (var e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    argminCandidate = e.Current;
                    value = selector(e.Current);
                }
                while (!value.HasValue);

                long valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    long? cur = selector(e.Current);
                    long x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x < valueVal)
                    {
                        valueVal = x;
                        argminCandidate = e.Current;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning <see cref="float"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, float Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            float value;
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                argminCandidate = e.Current;
                value = selector(e.Current);
                if (float.IsNaN(value))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    float x = selector(e.Current);
                    if (x < value)
                    {
                        argminCandidate = e.Current;
                        value = x;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (float.IsNaN(x))
                    {
                        return x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning nullable <see cref="float"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, float? Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            float? value = null;
            using (var e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    argminCandidate = e.Current;
                    value = selector(e.Current);
                }
                while (!value.HasValue);

                float valueVal = value.GetValueOrDefault();
                if (float.IsNaN(valueVal))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    float? cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        float x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            argminCandidate = e.Current;
                            value = cur;
                        }
                        else if (float.IsNaN(x))
                        {
                            return cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning <see cref="double"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, double Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            double value;
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                argminCandidate = e.Current;
                value = selector(e.Current);
                if (double.IsNaN(value.Item))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    double x = selector(e.Current);
                    if (x < value)
                    {
                        argminCandidate = e.Current;
                        value = x;
                    }
                    else if (double.IsNaN(x))
                    {
                        return x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning nullable <see cref="double"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, double? Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            double? value = null;
            using (var e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    argminCandidate = e.Current;
                    value = selector(e.Current);
                }
                while (!value.HasValue);

                double valueVal = value.GetValueOrDefault();
                if (double.IsNaN(valueVal))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    double? cur = selector(e.Current);
                    if (cur.HasValue)
                    {
                        double x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            argminCandidate = e.Current;
                            value = cur;
                        }
                        else if (double.IsNaN(x))
                        {
                            return cur;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning nullable <see cref="decimal"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, decimal Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            decimal value;
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException(Resources.NoElements);
                }

                argminCandidate = e.Current;
                value = selector(e.Current);
                while (e.MoveNext())
                {
                    decimal x = selector(e.Current);
                    if (x < value)
                    {
                        argminCandidate = e.Current;
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function returning nullable <see cref="decimal"/> on a sequence of values.
        /// </summary>
        public static (TSource Argmin, decimal? Min) Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            TSource argminCandidate;
            decimal? value = null;
            using (var e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    argminCandidate = e.Current;
                    value = selector(e.Current);
                }
                while (!value.HasValue);

                decimal valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    decimal? cur = selector(e.Current);
                    decimal x = cur.GetValueOrDefault();
                    if (cur.HasValue && x < valueVal)
                    {
                        valueVal = x;
                        argminCandidate = e.Current;
                        value = cur;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the minimum value and the minimizing value for a function on a sequence of values.
        /// </summary>
        // TODO https://github.com/recorefx/RecoreFX/issues/24
        //[return: MaybeNull]
        public static (TSource Argmin, TResult Min) Argmin<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
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
            // TODO https://github.com/recorefx/RecoreFX/issues/24
            //TResult value = default!;
            TSource argminCandidate;
            TResult value = default;
            if (value == null)
            {
                using (var e = source.GetEnumerator())
                {
                    do
                    {
                        if (!e.MoveNext())
                        {
                            return value;
                        }

                        argminCandidate = e.Current;
                        value = selector(e.Current);
                    }
                    while (value == null);

                    while (e.MoveNext())
                    {
                        TResult x = selector(e.Current);
                        if (x != null && comparer.Compare(x, value) < 0)
                        {
                            argminCandidate = e.Current;
                            value = x;
                        }
                    }
                }
            }
            else
            {
                using (var e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        throw new InvalidOperationException(Resources.NoElements);
                    }

                    argminCandidate = e.Current;
                    value = selector(e.Current);
                    while (e.MoveNext())
                    {
                        TResult x = selector(e.Current);
                        if (comparer.Compare(x, value) < 0)
                        {
                            argminCandidate = e.Current;
                            value = x;
                        }
                    }
                }
            }

            return value;
        }
    }
}