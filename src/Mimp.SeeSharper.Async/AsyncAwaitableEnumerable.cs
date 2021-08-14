using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public class AsyncAwaitableEnumerable<T> : IAwaitableEnumerable<T>, IAsyncEnumerable<IAwaitable<T>>
    {


        private readonly IAsyncEnumerable<IAwaitable<T>> _enumerable;


        public AsyncAwaitableEnumerable(IAsyncEnumerable<IAwaitable<T>> enumerable)
        {
            _enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }


        public IEnumerableAwaiter<T> GetAwaiter(CancellationToken cancellationToken) =>
            new AwaitableEnumerableAwaiter<T>(_enumerable.GetAsyncEnumerator(cancellationToken), cancellationToken);


        public IAsyncEnumerator<IAwaitable<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            _enumerable.GetAsyncEnumerator(cancellationToken);


    }
}