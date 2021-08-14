using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Mimp.SeeSharper.Async
{
    public class AwaitableEnumerable<T> : IAwaitableEnumerable<T>, IEnumerable<IAwaitable<T>>
    {


        private readonly IEnumerable<IAwaitable<T>> _awaitables;

        public AwaitableEnumerable(IEnumerable<IAwaitable<T>> awaitables)
        {
            _awaitables = awaitables ?? throw new ArgumentNullException(nameof(awaitables));
        }


        public IEnumerableAwaiter<T> GetAwaiter(CancellationToken cancellationToken) =>
            new EnumerableAwaiter<T>(_awaitables.GetEnumerator(), cancellationToken);


        IEnumerator<IAwaitable<T>> IEnumerable<IAwaitable<T>>.GetEnumerator() =>
            _awaitables.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            _awaitables.GetEnumerator();


    }
}
