using System;
using System.Collections.Generic;
using System.Linq;

namespace Recore
{
    /// <summary>
    /// Represents the result of an operation that can be successful or failed.
    /// </summary>
    public sealed class Result<TValue, TError> : IEquatable<Result<TValue, TError>>
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
        /// Converts <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>
        /// to <c cref="Optional{TValue}">Optional&lt;TValue&gt;</c>
        /// </summary>
        public Optional<TValue> GetValue()
            => either.GetLeft();

        /// <summary>
        /// Converts <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>
        /// to <c cref="Optional{TError}">Optional&lt;TError&gt;</c>
        /// </summary>
        public Optional<TError> GetError()
            => either.GetRight();

        /// <summary>
        /// Maps a function over the <c cref="Result{TValue, TError}">Result</c> only if the result is successful.
        /// </summary>
        public Result<TResult, TError> OnValue<TResult>(Func<TValue, TResult> onValue)
            => Switch(
                value => Result.Success<TResult, TError>(onValue(value)),
                Result.Failure<TResult, TError>);

        /// <summary>
        /// Maps a function over the <c cref="Result{TValue, TError}">Result</c> only if the result is failed.
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
        /// Chains another <c cref="Result{TValue, TError}">Result</c>-producing operation from another.
        /// </summary>
        /// <remarks>
        /// This is a monad bind operation.
        /// Conceptually, it is the same as passing <paramref name="f"/> to <c cref="OnValue{TResult}(Func{TValue, TResult})">OnValue</c>
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
        /// Compares this <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>
        /// to another object for equality.
        /// </summary>
        /// <remarks>
        /// Two <c>Result</c>s are equal only if they have the same type parameters in the same order.
        /// For example, an <c>Result&lt;int, string&gt;</c> and an <c>Result&lt;string, int&gt;</c>
        /// will always be nonequal.
        /// </remarks>
        public override bool Equals(object obj)
            => obj is Result<TValue, TError>
            && this.Equals((Result<TValue, TError>)obj);

        /// <summary>
        /// Compares two instances of <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>
        /// for equality.
        /// </summary>
        /// <remarks>
        /// Equality is defined as both objects' underlying values or errors being equal.
        /// </remarks>
        public bool Equals(Result<TValue, TError> other)
            => other != null
            && this.either == other.either;

        /// <summary>
        /// Returns the hash code of the underlying value.
        /// </summary>
        public override int GetHashCode()
            => either.GetHashCode();

        /// <summary>
        /// Determines whether two instances of <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>
        /// have the same value.
        /// </summary>
        public static bool operator ==(Result<TValue, TError> lhs, Result<TValue, TError> rhs) => Equals(lhs, rhs);

        /// <summary>
        /// Determines whether two instances of <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>
        /// have the same value.
        /// </summary>
        public static bool operator !=(Result<TValue, TError> lhs, Result<TValue, TError> rhs) => !Equals(lhs, rhs);

        /// <summary>
        /// Converts an instance of a type to an <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>.
        /// </summary>
        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        /// <summary>
        /// Converts an instance of a type to an <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>.
        /// </summary>
        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);
    }

    /// <summary>
    /// Provides additional methods for <c>Result</c>.
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
        /// Converts a <c cref="Result{TValue, TError}">Result&lt;Result&lt;TValue, TError&gt;, TError&gt;</c>
        /// to a <c cref="Result{TValue, TError}">Result&lt;TValue, TError&gt;</c>.
        /// </summary>
        public static Result<TValue, TError> Flatten<TValue, TError>(this Result<Result<TValue, TError>, TError> resultResult)
            => resultResult.Then(x => x);

        /// <summary>
        /// Collects all the values of successful results from the sequence.
        /// </summary>
        public static IEnumerable<TValue> Values<TValue, TError>(this IEnumerable<Result<TValue, TError>> source)
            => source
            .Select(x => x.GetValue())
            .NonEmpty();

        /// <summary>
        /// Collects all the errors from failed results from the sequence.
        /// </summary>
        public static IEnumerable<TError> Errors<TValue, TError>(this IEnumerable<Result<TValue, TError>> source)
            => source
            .Select(x => x.GetError())
            .NonEmpty();
    }
}