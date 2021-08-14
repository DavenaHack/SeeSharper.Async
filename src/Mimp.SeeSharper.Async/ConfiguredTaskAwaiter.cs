using Mimp.SeeSharper.Async.Abstraction;
using System;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct ConfiguredTaskAwaiter : IAwaiter
    {


        public Awaiter.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public ConfiguredTaskAwaiter(Awaiter.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public void GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter(ConfiguredTaskAwaiter awaiter) => awaiter.Awaiter;


    }


    public readonly struct ConfiguredTaskAwaiter<T> : IAwaiter<T>
    {


        public Awaiter.ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public ConfiguredTaskAwaiter(Awaiter.ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public T GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter(ConfiguredTaskAwaiter<T> awaiter) => awaiter.Awaiter;


    }
}
