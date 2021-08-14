using Mimp.SeeSharper.Async.Abstraction;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    [AsyncMethodBuilder(typeof(ValueTaskAwaitableAsyncMethodBuilder))]
    public readonly struct ValueTaskAwaitable : IAwaitable
    {


        public ValueTask Task { get; }


        public ValueTaskAwaitable(ValueTask task)
        {
            Task = task;
        }


        public IAwaiter GetAwaiter() =>
            new ValueTaskAwaiter(Task.GetAwaiter());


        public IAwaitable ConfigureAwait(bool continueOnCapturedContext) =>
            Task.ConfigureAwait(continueOnCapturedContext).ToAwaitable();


        public static implicit operator ValueTask(ValueTaskAwaitable task) => task.Task;


    }


    [AsyncMethodBuilder(typeof(ValueTaskAwaitableAsyncMethodBuilder<>))]
    public readonly struct ValueTaskAwaitable<T> : IAwaitable<T>
    {


        public ValueTask<T> Task { get; }


        public ValueTaskAwaitable(ValueTask<T> task)
        {
            Task = task;
        }


        public IAwaiter<T> GetAwaiter() =>
            new ValueTaskAwaiter<T>(Task.GetAwaiter());


        public static implicit operator ValueTask<T>(ValueTaskAwaitable<T> task) => task.Task;


    }
}