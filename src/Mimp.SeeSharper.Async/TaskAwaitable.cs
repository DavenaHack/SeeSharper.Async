using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    [AsyncMethodBuilder(typeof(TaskAwaitableAsyncMethodBuilder))]
    public readonly struct TaskAwaitable : IConfigureAwaitable, IAwaitable, IDisposable
    {


        public Task Task { get; }


        public TaskAwaitable(Task task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
        }


        public IAwaiter GetAwaiter() =>
            new TaskAwaiter(Task.GetAwaiter());


        public IAwaitable ConfigureAwait(bool continueOnCapturedContext) =>
            Task.ConfigureAwait(continueOnCapturedContext).ToAwaitable();


        public void Dispose() => Task.Dispose();



        public static implicit operator Task(TaskAwaitable task) => task.Task;


    }


    [AsyncMethodBuilder(typeof(TaskAwaitableAsyncMethodBuilder<>))]
    public readonly struct TaskAwaitable<T> : IAwaitable<T>
    {


        public Task<T> Task { get; }


        public TaskAwaitable(Task<T> task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
        }


        public IAwaiter<T> GetAwaiter() =>
            new TaskAwaiter<T>(Task.GetAwaiter());


        public static implicit operator Task<T>(TaskAwaitable<T> task) => task.Task;


    }
}
