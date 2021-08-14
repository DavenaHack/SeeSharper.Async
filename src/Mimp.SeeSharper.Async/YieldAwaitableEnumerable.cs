using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public class YieldAwaitableEnumerable<T> : IAwaitableEnumerable<T>
    {


        private readonly Func<Func<IAwaitable<T>, IAwaitable>, CancellationToken, IAwaitable> _function;


        public YieldAwaitableEnumerable(Func<Func<IAwaitable<T>, IAwaitable>, CancellationToken, IAwaitable> function)
        {
            _function = function ?? throw new ArgumentNullException(nameof(function));
        }


        public IEnumerableAwaiter<T> GetAwaiter(CancellationToken cancellationToken) =>
            new YieldEnumerableAwaiter<T>(_function, cancellationToken);


    }
}
