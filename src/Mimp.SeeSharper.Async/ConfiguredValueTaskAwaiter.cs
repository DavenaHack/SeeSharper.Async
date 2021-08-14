using Mimp.SeeSharper.Async.Abstraction;
using System;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct ConfiguredValueTaskAwaiter : IAwaiter
    {


        public Awaiter.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public ConfiguredValueTaskAwaiter(Awaiter.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public void GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter(ConfiguredValueTaskAwaiter awaiter) => awaiter.Awaiter;


    }


    public readonly struct ConfiguredValueTaskAwaiter<T> : IAwaiter<T>
    {


        public Awaiter.ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public ConfiguredValueTaskAwaiter(Awaiter.ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public T GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter(ConfiguredValueTaskAwaiter<T> awaiter) => awaiter.Awaiter;


    }
}
