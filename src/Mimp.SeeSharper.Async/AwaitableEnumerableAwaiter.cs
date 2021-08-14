using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public class AwaitableEnumerableAwaiter<T> : IEnumerableAwaiter<T>, IAsyncDisposable, IAwaitableDisposable
    {


        private readonly IAsyncEnumerator<IAwaitable<T>> _enumerator;

        private readonly CancellationToken _cancellationToken;


        public AwaitableEnumerableAwaiter(IAsyncEnumerator<IAwaitable<T>> enumerator, CancellationToken cancellationToken)
        {
            _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            _cancellationToken = cancellationToken;
        }


        public IAwaitable<bool> AwaitNextAsync(CancellationToken cancellationToken)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            cancellationToken.ThrowIfCancellationRequested();

            return _enumerator.MoveNextAsync().ToAwaitable();
        }

        public IAwaitable<T> GetNextAsync(CancellationToken cancellationToken)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            cancellationToken.ThrowIfCancellationRequested();

            return _enumerator.Current;
        }


        public IAwaitable DisposeAsync() =>
            ((IAsyncDisposable)this).DisposeAsync().ToAwaitable();

        ValueTask IAsyncDisposable.DisposeAsync() =>
            _enumerator.DisposeAsync();


    }
}