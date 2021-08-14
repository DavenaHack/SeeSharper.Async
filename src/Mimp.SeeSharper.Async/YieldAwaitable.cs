using Mimp.SeeSharper.Async.Abstraction;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct YieldAwaitable : IAwaitable
    {


        public Awaiter.YieldAwaitable Awaitable { get; }


        public YieldAwaitable(Awaiter.YieldAwaitable awaitable)
        {
            Awaitable = awaitable;
        }


        public IAwaiter GetAwaiter() =>
            new YieldAwaiter(Awaitable.GetAwaiter());


        public static implicit operator Awaiter.YieldAwaitable(YieldAwaitable task) => task.Awaitable;


    }
}
