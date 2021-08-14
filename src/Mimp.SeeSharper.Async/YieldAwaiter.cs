using Mimp.SeeSharper.Async.Abstraction;
using System;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct YieldAwaiter : IAwaiter
    {


        public Awaiter.YieldAwaitable.YieldAwaiter Awaiter { get; }


        public bool IsCompleted => Awaiter.IsCompleted;


        public YieldAwaiter(Awaiter.YieldAwaitable.YieldAwaiter awaiter)
        {
            Awaiter = awaiter;
        }


        public void UnsafeOnCompleted(Action continuation) => Awaiter.UnsafeOnCompleted(continuation);

        public void OnCompleted(Action continuation) => Awaiter.OnCompleted(continuation);


        public void GetResult() => Awaiter.GetResult();


        public static implicit operator Awaiter.YieldAwaitable.YieldAwaiter(YieldAwaiter awaiter) => awaiter.Awaiter;


    }
}