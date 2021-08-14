using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public class YieldEnumerableAwaiter<T> : IEnumerableAwaiter<T>, IAsyncDisposable
    {


        private readonly CancellationToken _cancellationToken;

        private readonly CancellationTokenSource _masterCancellationSource;


        private readonly IAwaitable _function;

        private TaskCompletionSource<IAwaitable<T>>? _nextYield;
        private IAwaitable<IAwaitable<T>>? _yield;

        private Yielder? _current;

        private Yielder? _next;


        public YieldEnumerableAwaiter(Func<Func<IAwaitable<T>, IAwaitable>, CancellationToken, IAwaitable> function, CancellationToken cancellationToken)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            _cancellationToken = cancellationToken;
            _masterCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken);

            _nextYield = new TaskCompletionSource<IAwaitable<T>>();

            _function = function(Yield, cancellationToken) ?? throw new ArgumentException($"{nameof(function)} return no {nameof(IAwaitable)}.", nameof(function));
        }


        public async IAwaitable<bool> AwaitNextAsync(CancellationToken cancellationToken)
        {
            if (_nextYield is null)
                throw new InvalidOperationException("The end of awaiter has been reached.");

            _masterCancellationSource.Token.ThrowIfCancellationRequested();
            cancellationToken.ThrowIfCancellationRequested();

            if (_function.GetAwaiter().IsCompleted)
            {
                if (_current != _next)
                    throw new InvalidOperationException($"The end of awaiter has been reached before the last yield has proceeded. Ensure that every yield is awaiting.");

                await _function; // throw exception
            }

            // if GetNextAsync not called continue / don't wait for completing
            if (!_current?.Source.Task.IsCompleted ?? false)
            {
                _current!.Continue();
                await _current.ContinueAwaitable;
            }

            _yield = _nextYield!.Task.ToAwaitable();
            _nextYield = new TaskCompletionSource<IAwaitable<T>>();
            _current = _next;

            var done = await Awaitables.AwaitAnyAwaitable(_yield.NoResult(), _function);
            if (done == _function)
            {
                _yield = null;
                _nextYield = null;
                return false;
            }
            return true;
        }


        public async IAwaitable<T> GetNextAsync(CancellationToken cancellationToken)
        {
            if (_current is null)
                throw new InvalidOperationException($"The awaiter isn't started. Call before {nameof(AwaitNextAsync)}.");
            if (_yield is null)
                throw new InvalidOperationException($"The end of awaiter has been reached.");

            _masterCancellationSource.Token.ThrowIfCancellationRequested();
            cancellationToken.ThrowIfCancellationRequested();

            if (!_current!.IsCompleted)
                try
                {
                    _current.Source.SetResult(await await _yield!);
                }
                catch (Exception ex)
                {
                    _current.Source.SetException(ex);
                }

            return await _current.Awaitable;
        }


        private IAwaitable Yield(IAwaitable<T> value)
        {
            _masterCancellationSource.Token.ThrowIfCancellationRequested();

            if (_current != _next)
                throw new InvalidOperationException($"It was yield before the last yield has proceeded. Ensure that every yield is awaiting.");

            _next = new Yielder();
            _nextYield!.SetResult(value);

            return _next.ContinueAwaitable;
        }


        public ValueTask DisposeAsync()
        {
            _masterCancellationSource.Cancel();
            _masterCancellationSource.Dispose();
            _nextYield = null;
            _yield = null;
            _current = null;
            _next = null;
            return default;
        }


        private class Yielder
        {


            public bool IsCompleted => Source.Task.IsCompleted;


            public TaskCompletionSource<T> Source { get; }

            public IAwaitable<T> Awaitable { get; }

            public IAwaitable ContinueAwaitable { get; }


            private readonly TaskCompletionSource<bool> _continueSource;


            public Yielder()
            {
                Source = new TaskCompletionSource<T>();
                Awaitable = Source.Task.ToAwaitable();
                _continueSource = new TaskCompletionSource<bool>();
                ContinueAwaitable = Awaitables.AwaitAny(Awaitable.NoResult(), _continueSource.Task.ToAwaitable().NoResult())
                    .ConfigureAwait(false);
            }


            public void Continue()
            {
                _continueSource.SetResult(true);
            }


        }

    }
}