using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Recore.Text.Json.Serialization.Converters;

namespace Recore
{
    /// <summary>
    /// Represents the result of an operation that can be successful or failed.
    /// </summary>
    /// <remarks>
    /// When working with <c>System.Text.Json</c>, deserializing into <see cref="Result{TValue, TError}"/>
    /// can be ambiguous.
    /// The default deserialization behavior will try first to deserialize
    /// as <typeparamref name="TValue"/>, and then as <typeparamref name="TError"/>.
    /// But, this may return the unintended type if the value deserializer can successfully deserialize the JSON
    /// representation of the error.
    /// 
    /// A common case is when both <typeparamref name="TValue"/> and <typeparamref name="TError"/> are POCOs.
    /// In that case, <see cref="Result{TValue, TError}"/> will always deserialize as <typeparamref name="TValue"/>,
    /// filling in default values for any missing properties.
    /// For example:
    /// 
    /// <code>
    /// class Person
    /// {
    ///     public string Name { get; set; }
    ///     public int Age { get; set; }
    /// }
    /// 
    /// class Address
    /// {
    ///     public string Street { get; set; }
    ///     public string Zip { get; set; }
    /// }
    /// 
    /// // Deserializes as a `Person`!
    /// JsonSerializer.Deserialize&lt;Result&lt;Person, Address&gt;&gt;("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}")
    /// </code>
    /// 
    /// You can use <seealso cref="OverrideResultConverter{TValue, TError}"/> to specify
    /// how to choose between <typeparamref name="TValue"/> and <typeparamref name="TError"/>
    /// based on the properties in the JSON:
    /// 
    /// <code>
    /// // Look at the JSON to decide which type we have
    /// options.Converters.Add(new OverrideResultConverter&lt;Person, Address&gt;(
    ///     deserializeAsLeft: json =&gt; json.TryGetProperty("Street", out JsonElement _)));
    /// 
    /// // Deserializes correctly
    /// JsonSerializer.Deserialize&lt;Result&lt;Person, Address&gt;&gt;("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}", options)
    /// </code>
    /// </remarks>
    [JsonConverter(typeof(ResultConverter))]
    public sealed class Result<TValue, TError> : IEquatable<Result<TValue, TError>?>
    {
        private readonly Either<TValue, TError> either;

        /// <summary>
        /// Indicates whether the result is successful.
        /// </summary>
        public bool IsSuccessful => either.IsLeft;

        /// <summary>
        /// Constructs an instance of the type from a value of <typeparamref name="TValue"/>.
        /// </summary>
        public Result(TValue value)
        {
            either = value;
        }

        /// <summary>
        /// Constructs an instance of the type from a value of <typeparamref name="TError"/>.
        /// </summary>
        public Result(TError error)
        {
            either = error;
        }

        /// <summary>
        /// Calls one of two functions depending on whether the result is successful.
        /// </summary>
        public T Switch<T>(Func<TValue, T> onValue, Func<TError, T> onError)
            => either.Switch(onValue, onError);

        /// <summary>
        /// Takes one of two actions depending on whether the result is successful.
        /// </summary>
        public void Switch(Action<TValue> onValue, Action<TError> onError)
            => either.Switch(onValue, onError);

        /// <summary>
        /// Converts <see cref="Result{TValue, TError}"/>
        /// to <c cref="Optional{TValue}">Optional&lt;TValue&gt;</c>
        /// </summary>
        public Optional<TValue> GetValue()
            => either.GetLeft();

        /// <summary>
        /// Converts <see cref="Result{TValue, TError}"/>
        /// to <c cref="Optional{TError}">Optional&lt;TError&gt;</c>
        /// </summary>
        public Optional<TError> GetError()
            => either.GetRight();

        /// <summary>
        /// Maps a function over the <see cref="Result{TValue, TError}"/> only if the result is successful.
        /// </summary>
        public Result<TResult, TError> OnValue<TResult>(Func<TValue, TResult> onValue)
            => Switch(
                value => Result.Success<TResult, TError>(onValue(value)),
                Result.Failure<TResult, TError>);

        /// <summary>
        /// Maps a function over the <see cref="Result{TValue, TError}"/> only if the result is failed.
        /// </summary>
        public Result<TValue, TResult> OnError<TResult>(Func<TError, TResult> onError)
            => Switch(
                Result.Success<TValue, TResult>,
                error => Result.Failure<TValue, TResult>(onError(error)));

        /// <summary>
        /// Takes an action only if the result is successful.
        /// </summary>
        public void IfValue(Action<TValue> onValue)
            => either.IfLeft(onValue);

        /// <summary>
        /// Takes an action only if the the result is failed.
        /// </summary>
        public void IfError(Action<TError> onError)
            => either.IfRight(onError);

        /// <summary>
        /// Chains another <see cref="Result{TValue, TError}"/>-producing operation from another.
        /// </summary>
        /// <remarks>
        /// This is a monad bind operation.
        /// Conceptually, it is the same as passing <paramref name="f"/> to <see cref="OnValue{TResult}(Func{TValue, TResult})"/>
        /// and then "flattening" the result.
        /// </remarks>
        public Result<TResult, TError> Then<TResult>(Func<TValue, Result<TResult, TError>> f)
            => Switch(
                f,
                Result.Failure<TResult, TError>);

        /// <summary>
        /// Returns the string representation of the underlying value or error.
        /// </summary>
        public override string ToString()
            => either.ToString();

        /// <summary>
        /// Compares this <see cref="Result{TValue, TError}"/>
        /// to another object for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="Result{TValue, TError}"/>s are equal only if they have the same type parameters in the same order.
        /// For example, an <c>Result&lt;int, string&gt;</c> and an <c>Result&lt;string, int&gt;</c>
        /// will always be nonequal.
        /// </remarks>
        public override bool Equals(object? obj)
            => obj is Result<TValue, TError> result
            && this.Equals(result);

        /// <summary>
        /// Compares two instances of <see cref="Result{TValue, TError}"/>
        /// for equality.
        /// </summary>
        /// <remarks>
        /// Equality is defined as both objects' underlying values or errors being equal.
        /// </remarks>
        public bool Equals(Result<TValue, TError>? other)
            => !(other is null)
            && this.either == other.either;

        /// <summary>
        /// Compares two instances of <see cref="Result{TValue, TError}"/>
        /// for equality using the given <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public bool Equals(Result<TValue, TError>? other, IEqualityComparer<TValue?> valueComparer, IEqualityComparer<TError?> errorComparer)
            => !(other is null)
            && this.either.Equals(other.either, valueComparer, errorComparer);

        /// <summary>
        /// Returns the hash code of the underlying value.
        /// </summary>
        public override int GetHashCode()
            => either.GetHashCode();

        /// <summary>
        /// Determines whether two instances of <see cref="Result{TValue, TError}"/>
        /// have the same value.
        /// </summary>
        public static bool operator ==(Result<TValue, TError> lhs, Result<TValue, TError> rhs) => Equals(lhs, rhs);

        /// <summary>
        /// Determines whether two instances of <see cref="Result{TValue, TError}"/>
        /// have different values.
        /// </summary>
        public static bool operator !=(Result<TValue, TError> lhs, Result<TValue, TError> rhs) => !Equals(lhs, rhs);

        /// <summary>
        /// Converts an instance of a type to an <see cref="Result{TValue, TError}"/>.
        /// </summary>
        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        /// <summary>
        /// Converts an instance of a type to an <see cref="Result{TValue, TError}"/>.
        /// </summary>
        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);
    }

    /// <summary>
    /// Provides additional methods for <seealso cref="Result{TValue, TError}"/>.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static Result<TValue, TError> Success<TValue, TError>(TValue value)
            => new Result<TValue, TError>(value);

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        public static Result<TValue, TError> Failure<TValue, TError>(TError error)
            => new Result<TValue, TError>(error);

        /// <summary>
        /// Wraps a function to be executed and converted to <see cref="Result{TValue, TError}"/>.
        /// </summary>
        public sealed class Catcher<TValue>
        {
            private readonly Func<TValue> func;

            internal Catcher(Func<TValue> func)
            {
                this.func = func;
            }

            /// <summary>
            /// Executes the stored function and catches exceptions of the given type.
            /// </summary>
            public Result<TValue, TException> Catch<TException>() where TException : Exception
            {
                try
                {
                    return func();
                }
                catch (TException e)
                {
                    return e;
                }
            }

            /// <summary>
            /// Executes the stored function and catches exceptions of the given type matching the given predicate.
            /// </summary>
            public Result<TValue, TMapped> Catch<TException, TMapped>(Func<TException, TMapped> onException) where TException : Exception
            {
                try
                {
                    return func();
                }
                catch (TException e)
                {
                    return onException(e);
                }
            }
        }

        /// <summary>
        /// Suspends a function to be executed by <see cref="Catcher{TValue}.Catch{TException}()"/>.
        /// </summary>
        public static Catcher<TValue> Try<TValue>(Func<TValue> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return new Catcher<TValue>(func);
        }

        /// <summary>
        /// Wraps a function to be executed and converted to <see cref="Result{TValue, TError}"/>.
        /// </summary>
        public sealed class AsyncCatcher<TValue>
        {
            private readonly AsyncFunc<TValue> func;

            internal AsyncCatcher(AsyncFunc<TValue> func)
            {
                this.func = func;
            }

            /// <summary>
            /// Executes the stored function and catches exceptions of the given type.
            /// </summary>
            public async Task<Result<TValue, TException>> CatchAsync<TException>() where TException : Exception
            {
                try
                {
                    return await func();
                }
                catch (TException e)
                {
                    return e;
                }
            }

            /// <summary>
            /// Executes the stored function and catches exceptions of the given type matching the given predicate.
            /// </summary>
            public async Task<Result<TValue, TResult>> CatchAsync<TException, TResult>(AsyncFunc<TException, TResult> onException) where TException : Exception
            {
                try
                {
                    return await func();
                }
                catch (TException e)
                {
                    return await onException(e);
                }
            }
        }

        /// <summary>
        /// Suspends a function to be executed by <see cref="AsyncCatcher{TValue}.CatchAsync{TException}()"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="TryAsync{TValue}(AsyncFunc{TValue})"/> must be used instead of <see cref="Try{TValue}(Func{TValue})"/>
        /// to ensure exceptions are caught properly with async code.
        /// </remarks>
        public static AsyncCatcher<TValue> TryAsync<TValue>(AsyncFunc<TValue> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return new AsyncCatcher<TValue>(func);
        }

        /// <summary>
        /// Converts a <c cref="Result{TValue, TError}">Result&lt;Result&lt;TValue, TError&gt;, TError&gt;</c>
        /// to a <see cref="Result{TValue, TError}"/>.
        /// </summary>
        public static Result<TValue, TError> Flatten<TValue, TError>(this Result<Result<TValue, TError>, TError> resultResult)
            => resultResult.Then(x => x);

        /// <summary>
        /// Retrieves the value of a <see cref="Result{TValue, TError}"/> when <c>TValue</c> and <c>TError</c> are the same.
        /// </summary>
        public static T Collapse<T>(this Result<T, T> result)
            => result.Switch(
                v => v,
                e => e);

        /// <summary>
        /// Collects all the values of successful results from the sequence.
        /// </summary>
        public static IEnumerable<TValue> Successes<TValue, TError>(this IEnumerable<Result<TValue, TError>> source)
            => source
                .Where(x => x.IsSuccessful)
                .Select(x => x.Switch(
                    v => v,
                    e => throw new InvalidOperationException()));

        /// <summary>
        /// Collects all the errors from failed results from the sequence.
        /// </summary>
        public static IEnumerable<TError> Failures<TValue, TError>(this IEnumerable<Result<TValue, TError>> source)
            => source
                .Where(x => !x.IsSuccessful)
                .Select(x => x.Switch(
                    v => throw new InvalidOperationException(),
                    e => e));
    }
}