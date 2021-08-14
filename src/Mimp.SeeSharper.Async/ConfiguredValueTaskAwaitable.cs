using Mimp.SeeSharper.Async.Abstraction;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct ConfiguredValueTaskAwaitable : IAwaitable
    {


        public Awaiter.ConfiguredValueTaskAwaitable Awaitable { get; }


        public ConfiguredValueTaskAwaitable(Awaiter.ConfiguredValueTaskAwaitable awaitable)
        {
            Awaitable = awaitable;
        }


        public IAwaiter GetAwaiter() =>
            new ConfiguredValueTaskAwaiter(Awaitable.GetAwaiter());


        public static implicit operator Awaiter.ConfiguredValueTaskAwaitable(ConfiguredValueTaskAwaitable task) => task.Awaitable;


    }


    public readonly struct ConfiguredValueTaskAwaitable<T> : IAwaitable<T>
    {


        public Awaiter.ConfiguredValueTaskAwaitable<T> Awaitable { get; }


        public ConfiguredValueTaskAwaitable(Awaiter.ConfiguredValueTaskAwaitable<T> awaitable)
        {
            Awaitable = awaitable;
        }


        public IAwaiter<T> GetAwaiter() =>
            new ConfiguredValueTaskAwaiter<T>(Awaitable.GetAwaiter());


        public static implicit operator Awaiter.ConfiguredValueTaskAwaitable<T>(ConfiguredValueTaskAwaitable<T> task) => task.Awaitable;


    }
}
