using Mimp.SeeSharper.Async.Abstraction;
using System;

namespace Mimp.SeeSharper.Async
{
    public static class AwaitableDisposableExtensions
    {


        public static IAsyncDisposable ToAsyncDisposable(this IAwaitableDisposable disposable)
        {
            if (disposable is null)
                throw new ArgumentNullException(nameof(disposable));

            return new AwaitableAsyncDisposable(disposable);
        }


    }
}
