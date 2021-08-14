using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public class AwaitableAsyncDisposable : IAsyncDisposable
    {


        private readonly IAwaitableDisposable _disposable;


        public AwaitableAsyncDisposable(IAwaitableDisposable disposable)
        {
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }


        public ValueTask DisposeAsync() =>
            _disposable.DisposeAsync().ToValueTask();


    }
}
