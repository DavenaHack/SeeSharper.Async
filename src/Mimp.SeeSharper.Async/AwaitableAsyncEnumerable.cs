using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public class AwaitableAsyncEnumerable<T> : IAwaitableEnumerable<T>, IAsyncEnumerable<T>
    {


        private readonly IAsyncEnumerable<T> _enumerable;


        public AwaitableAsyncEnumerable(IAsyncEnumerable<T> enumerable)
        {
            _enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }


        public IEnumerableAwaiter<T> GetAwaiter(CancellationToken cancellationToken) =>
            new AsyncEnumerableAwaiter<T>(_enumerable.GetAsyncEnumerator(cancellationToken), cancellationToken);


        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            _enumerable.GetAsyncEnumerator(cancellationToken);


    }
}