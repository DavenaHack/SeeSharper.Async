using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public static class Awaitables
    {


        #region Run


        public static IAwaitable Run(Action action, CancellationToken cancellationToken)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            return Task.Run(action, cancellationToken)
                .ConfigureAwait(false).ToAwaitable();
        }

        public static IAwaitable Run(Action action) =>
            Run(action, CancellationToken.None);


        public static IAwaitable Run(Func<IAwaitable> awaitable, CancellationToken cancellationToken)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            return Task.Run(() => awaitable().ToTask(), cancellationToken).ToAwaitable();
        }

        public static IAwaitable Run(Func<IAwaitable> awaitable) =>
            Run(awaitable, CancellationToken.None);


        public static IAwaitable<T> Run<T>(Func<T> function, CancellationToken cancellationToken)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            return Task.Run(function, cancellationToken)
                .ConfigureAwait(false).ToAwaitable();
        }

        public static IAwaitable<T> Run<T>(Func<T> function) =>
            Run(function, CancellationToken.None);

        public static IAwaitable<T> Run<T>(Func<IAwaitable<T>> awaitable, CancellationToken cancellationToken)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            return Task.Run(() => awaitable().ToTask(), cancellationToken).ToAwaitable();
        }

        public static IAwaitable<T> Run<T>(Func<IAwaitable<T>> awaitable) =>
            Run(awaitable, CancellationToken.None);


        #endregion Run


        #region Result


        public static IAwaitable Empty { get; } =
#if !NET
            Task.CompletedTask.ToAwaitable();
#else
            ValueTask.CompletedTask.ToAwaitable();
#endif


        public static IAwaitable<T> Result<T>(T result)
        {
#if !NET
            return Task.FromResult(result).ToAwaitable();
#else
            return ValueTask.FromResult(result).ToAwaitable();
#endif
        }


        #endregion Result


        #region Exception


        public static IAwaitable Exception(Exception exception)
        {
            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

#if !NET
            return Task.FromException(exception).ToAwaitable();
#else
            return ValueTask.FromException(exception).ToAwaitable();
#endif
        }

        public static IAwaitable<T> Exception<T>(Exception exception)
        {
            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

#if !NET
            return Task.FromException<T>(exception).ToAwaitable();
#else
            return ValueTask.FromException<T>(exception).ToAwaitable();
#endif
        }


        #endregion Exception


        #region Canceled


        public static IAwaitable Canceled(CancellationToken cancellationToken)
        {
#if !NET
            return Task.FromCanceled(cancellationToken).ToAwaitable();
#else
            return ValueTask.FromCanceled(cancellationToken).ToAwaitable();
#endif
        }

        public static IAwaitable<T> Canceled<T>(CancellationToken cancellationToken)
        {
#if !NET
            return Task.FromCanceled<T>(cancellationToken).ToAwaitable();
#else
            return ValueTask.FromCanceled<T>(cancellationToken).ToAwaitable();
#endif
        }


        #endregion Canceled


        #region Delay


        public static IAwaitable Delay(TimeSpan delay, CancellationToken cancellationToken)
        {
            return Task.Delay(delay, cancellationToken).ToAwaitable();
        }

        public static IAwaitable Delay(TimeSpan delay) =>
            Delay(delay, CancellationToken.None);


        public static IAwaitable Delay(int milliseconds, CancellationToken cancellationToken)
        {
            return Task.Delay(milliseconds, cancellationToken).ToAwaitable();
        }

        public static IAwaitable Delay(int milliseconds) =>
            Delay(milliseconds, CancellationToken.None);


        #endregion Delay


        #region AwaitAny


        public static IAwaitable<IAwaitable> AwaitAnyAwaitable(params IAwaitable[] awaitables) =>
            awaitables.AwaitAnyAwaitable();

        public static IAwaitable AwaitAny(params IAwaitable[] awaitables) =>
            awaitables.AwaitAny();


        public static async IAwaitable<IAwaitable> AwaitAnyAwaitable(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await (createAwaitables(cancelSource.Token)?.AwaitAnyAwaitable()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
            return result;
        }

        public static IAwaitable<IAwaitable> AwaitAnyAwaitable(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables) =>
            AwaitAnyAwaitable(createAwaitables, CancellationToken.None);


        public static async IAwaitable AwaitAny(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await (createAwaitables(cancelSource.Token)?.AwaitAny()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
        }

        public static IAwaitable AwaitAny(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables) =>
            AwaitAny(createAwaitables, CancellationToken.None);


        public static IAwaitable<IAwaitable> AwaitAnySuccessAwaitable(params IAwaitable[] awaitables) =>
            awaitables.AwaitAnySuccessAwaitable();

        public static IAwaitable AwaitAnySuccess(params IAwaitable[] awaitables) =>
            awaitables.AwaitAnySuccess();


        public static async IAwaitable<IAwaitable> AwaitAnySuccessAwaitable(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await (createAwaitables(cancelSource.Token)?.AwaitAnySuccessAwaitable()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
            return result;
        }

        public static IAwaitable<IAwaitable> AwaitAnySuccessAwaitable(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables) =>
            AwaitAnySuccessAwaitable(createAwaitables, CancellationToken.None);


        public static async IAwaitable AwaitAnySuccess(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await (createAwaitables(cancelSource.Token)?.AwaitAnySuccess()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
        }

        public static IAwaitable AwaitAnySuccess(Func<CancellationToken, IEnumerable<IAwaitable>> createAwaitables) =>
            AwaitAnySuccess(createAwaitables, CancellationToken.None);



        public static IAwaitable<IAwaitable<T>> AwaitAnyAwaitable<T>(params IAwaitable<T>[] awaitables) =>
            awaitables.AwaitAnyAwaitable();


        public static IAwaitable<T> AwaitAny<T>(params IAwaitable<T>[] awaitables) =>
            awaitables.AwaitAny();


        public static async IAwaitable<IAwaitable<T>> AwaitAnyAwaitable<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await (createAwaitables(cancelSource.Token)?.AwaitAnyAwaitable()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
            return result;
        }

        public static IAwaitable<IAwaitable<T>> AwaitAnyAwaitable<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables) =>
            AwaitAnyAwaitable(createAwaitables, CancellationToken.None);


        public static async IAwaitable<T> AwaitAny<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await (createAwaitables(cancelSource.Token)?.AwaitAny()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
            return result;
        }

        public static IAwaitable<T> AwaitAny<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables) =>
            AwaitAny(createAwaitables, CancellationToken.None);


        public static IAwaitable<IAwaitable<T>> AwaitAnySuccessAwaitable<T>(params IAwaitable<T>[] awaitables) =>
            awaitables.AwaitAnySuccessAwaitable();

        public static IAwaitable<T> AwaitAnySuccess<T>(params IAwaitable<T>[] awaitables) =>
            awaitables.AwaitAnySuccess();


        public static async IAwaitable<IAwaitable<T>> AwaitAnySuccessAwaitable<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await (createAwaitables(cancelSource.Token)?.AwaitAnySuccessAwaitable()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
            return result;
        }

        public static IAwaitable<IAwaitable<T>> AwaitAnySuccessAwaitable<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables) =>
            AwaitAnySuccessAwaitable(createAwaitables, CancellationToken.None);


        public static async IAwaitable<T> AwaitAnySuccess<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables, CancellationToken cancellationToken)
        {
            if (createAwaitables is null)
                throw new ArgumentNullException(nameof(createAwaitables));

            using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await (createAwaitables(cancelSource.Token)?.AwaitAnySuccess()
                ?? throw new ArgumentException($"{nameof(createAwaitables)} return null.", nameof(createAwaitables)));

            cancelSource.Cancel(); // clear all awaitables
            return result;
        }

        public static IAwaitable<T> AwaitAnySuccess<T>(Func<CancellationToken, IEnumerable<IAwaitable<T>>> createAwaitables) =>
            AwaitAnySuccess(createAwaitables, CancellationToken.None);


        #endregion AwaitAny


        #region AwaitAll


        public static IAwaitable AwaitAll(params IAwaitable[] awaitables) =>
            awaitables.AwaitAll();


        public static IAwaitable<IEnumerable<T>> AwaitAll<T>(params IAwaitable<T>[] awaitables) =>
            awaitables.AwaitAll();


        #endregion AwaitAll


        #region AwaitableEnumerable


        public static IAwaitableEnumerable<T> Yield<T>(Func<Func<IAwaitable<T>, IAwaitable>, CancellationToken, IAwaitable> function)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            return new YieldAwaitableEnumerable<T>(function);
        }

        public static IAwaitableEnumerable<T> Yield<T>(Func<Func<IAwaitable<T>, IAwaitable>, IAwaitable> function)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            return Yield((Func<IAwaitable<T>, IAwaitable> yield, CancellationToken cancellationToken) => function(yield));
        }

        public static IAwaitableEnumerable<T> Yield<T>(Func<Func<T, IAwaitable>, CancellationToken, IAwaitable> function)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            return Yield((Func<IAwaitable<T>, IAwaitable> yield, CancellationToken cancellationToken) =>
                function(t => yield(Result(t)), cancellationToken));
        }

        public static IAwaitableEnumerable<T> Yield<T>(Func<Func<T, IAwaitable>, IAwaitable> function)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            return Yield((Func<IAwaitable<T>, IAwaitable> yield, CancellationToken cancellationToken) => function(t => yield(Result(t))));
        }


        #endregion AwaitableEnumerable


    }
}
