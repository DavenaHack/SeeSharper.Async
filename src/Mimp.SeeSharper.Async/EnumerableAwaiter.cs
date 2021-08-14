using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public class EnumerableAwaiter<T> : IEnumerableAwaiter<T>, IDisposable
    {


        private readonly CancellationToken _cancellationToken;

        private readonly IEnumerator<IAwaitable<T>> _enumerator;


        public EnumerableAwaiter(IEnumerator<IAwaitable<T>> enumerator, CancellationToken cancellationToken)
        {
            _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            _cancellationToken = cancellationToken;
        }


        public IAwaitable<bool> AwaitNextAsync(CancellationToken cancellationToken)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            cancellationToken.ThrowIfCancellationRequested();

            return Awaitables.Result(_enumerator.MoveNext());
        }

        public IAwaitable<T> GetNextAsync(CancellationToken cancellationToken)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            cancellationToken.ThrowIfCancellationRequested();

            return _enumerator.Current ?? throw new InvalidOperationException();
        }


        public void Dispose()
        {
            _enumerator.Dispose();
            GC.SuppressFinalize(this);
        }


    }
}
