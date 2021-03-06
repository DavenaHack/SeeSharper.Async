using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public static class AwaitableEnumerableExtensions
    {


        public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(this IAwaitableEnumerable<T> awaitable, CancellationToken cancellationToken)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            return new EnumerableAwaiterAsyncEnumerator<T>(awaitable.GetAwaiter(cancellationToken));
        }

        public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(this IAwaitableEnumerable<T> awaitable) =>
            awaitable.GetAsyncEnumerator(CancellationToken.None);


        public static IAwaitableEnumerable<T> ToAwaitable<T>(this IEnumerable<IAwaitable<T>> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            return new AwaitableEnumerable<T>(enumerable);
        }

        public static IAwaitableEnumerable<T> ToAwaitable<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            return enumerable.Select(Awaitables.Result).ToAwaitable();
        }


        public static IAwaitableEnumerable<T> ToAwaitable<T>(this IAsyncEnumerable<T> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            return new AwaitableAsyncEnumerable<T>(enumerable);
        }

        public static IAwaitableEnumerable<T> ToAwaitable<T>(this IAsyncEnumerable<IAwaitable<T>> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            return new AsyncAwaitableEnumerable<T>(enumerable);
        }


        public static IEnumerableAwaiter<T> GetAwaiter<T>(this IAwaitableEnumerable<T> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            return enumerable.GetAwaiter(CancellationToken.None);
        }


        public static IEnumerable<T> Await<T>(this IAwaitableEnumerable<T> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            var awaiter = enumerable.GetAwaiter();
            while (awaiter.AwaitNextAsync().Await())
                yield return awaiter.GetNextAsync().Await();
        }


    }
}
