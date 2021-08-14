using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public class EnumerableAwaiterAsyncEnumerator<T> : IAsyncEnumerator<T>, IAsyncDisposable, IAwaitableDisposable, IDisposable
    {


        public IEnumerableAwaiter<T> Awaiter { get; }


        public T Current => _has ? _current! : throw new InvalidOperationException();

        private T? _current;
        private bool _has;


        public EnumerableAwaiterAsyncEnumerator(IEnumerableAwaiter<T> awaiter)
        {
            Awaiter = awaiter ?? throw new ArgumentNullException(nameof(awaiter));
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (!await Awaiter.AwaitNextAsync(CancellationToken.None))
            {
                _has = false;
                return false;
            }

            _current = await Awaiter.GetNextAsync(CancellationToken.None);
            _has = true;
            return true;
        }


        public IAwaitable DisposeAsync()
        {
            var await = Awaiter.DisposeAsync();
            GC.SuppressFinalize(this);
            return await;
        }

        ValueTask IAsyncDisposable.DisposeAsync() =>
            DisposeAsync().ToValueTask();


        public void Dispose() =>
            DisposeAsync().Await();


    }
}
