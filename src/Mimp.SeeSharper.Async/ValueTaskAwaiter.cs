using Mimp.SeeSharper.Async.Abstraction;
using System;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct ValueTaskAwaiter : IAwaiter
    {


        public Awaiter.ValueTaskAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public ValueTaskAwaiter(Awaiter.ValueTaskAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public void GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.ValueTaskAwaiter(ValueTaskAwaiter awaiter) => awaiter.Awaiter;


    }


    public readonly struct ValueTaskAwaiter<T> : IAwaiter<T>
    {


        public Awaiter.ValueTaskAwaiter<T> Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public ValueTaskAwaiter(Awaiter.ValueTaskAwaiter<T> awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public T GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.ValueTaskAwaiter<T>(ValueTaskAwaiter<T> awaiter) => awaiter.Awaiter;


    }
}