using Mimp.SeeSharper.Async.Abstraction;
using System;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct TaskAwaiter : IAwaiter
    {


        public Awaiter.TaskAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public TaskAwaiter(Awaiter.TaskAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public void GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.TaskAwaiter(TaskAwaiter awaiter) => awaiter.Awaiter;


    }


    public readonly struct TaskAwaiter<T> : IAwaiter<T>
    {


        public Awaiter.TaskAwaiter<T> Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public TaskAwaiter(Awaiter.TaskAwaiter<T> awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public T GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.TaskAwaiter<T>(TaskAwaiter<T> awaiter) => awaiter.Awaiter;


    }
}
