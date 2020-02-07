using System.Threading.Tasks;

namespace Recore
{
    /// <summary>
    /// Encapsulates an asynchronous method that has no parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<TResult>();

    /// <summary>
    /// Encapsulates an asynchronous method that has one parameter and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T, TResult>(T obj);

    /// <summary>
    /// Encapsulates an asynchronous method that has two parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);

    /// <summary>
    /// Encapsulates an asynchronous method that has three parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, TResult>(T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Encapsulates an asynchronous method that has four parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    /// <summary>
    /// Encapsulates an asynchronous method that has five parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>
    /// Encapsulates an asynchronous method that has six parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    /// <summary>
    /// Encapsulates an asynchronous method that has seven parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    /// <summary>
    /// Encapsulates an asynchronous method that has eight parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    /// <summary>
    /// Encapsulates an asynchronous method that has nine parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    /// <summary>
    /// Encapsulates an asynchronous method that has 10 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

    /// <summary>
    /// Encapsulates an asynchronous method that has 11 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

    /// <summary>
    /// Encapsulates an asynchronous method that has 12 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

    /// <summary>
    /// Encapsulates an asynchronous method that has 13 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

    /// <summary>
    /// Encapsulates an asynchronous method that has 14 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

    /// <summary>
    /// Encapsulates an asynchronous method that has 15 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

    /// <summary>
    /// Encapsulates an asynchronous method that has 16 parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// Equivalent to <c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
}
