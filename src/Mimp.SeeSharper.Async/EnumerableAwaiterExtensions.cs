using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public static class EnumerableAwaiterExtensions
    {


        public static IAwaitable<bool> AwaitNextAsync<T>(this IEnumerableAwaiter<T> awaiter)
        {
            if (awaiter is null)
                throw new ArgumentNullException(nameof(awaiter));

            return awaiter.AwaitNextAsync(CancellationToken.None);
        }


        public static IAwaitable<T> GetNextAsync<T>(this IEnumerableAwaiter<T> awaiter)
        {
            if (awaiter is null)
                throw new ArgumentNullException(nameof(awaiter));

            return awaiter.GetNextAsync(CancellationToken.None);
        }


        public static IEnumerable<T> Await<T>(this IEnumerableAwaiter<T> awaiter)
        {
            if (awaiter is null)
                throw new ArgumentNullException(nameof(awaiter));

            while (awaiter.AwaitNextAsync().Await())
                yield return awaiter.GetNextAsync().Await();
        }


    }
}
