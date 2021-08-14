using Mimp.SeeSharper.Async.Abstraction;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public readonly struct ConfiguredTaskAwaitable : IAwaitable
    {


        public Awaiter.ConfiguredTaskAwaitable Awaitable { get; }


        public ConfiguredTaskAwaitable(Awaiter.ConfiguredTaskAwaitable awaitable)
        {
            Awaitable = awaitable;
        }


        public IAwaiter GetAwaiter() =>
            new ConfiguredTaskAwaiter(Awaitable.GetAwaiter());


        public static implicit operator Awaiter.ConfiguredTaskAwaitable(ConfiguredTaskAwaitable task) => task.Awaitable;


    }


    public readonly struct ConfiguredTaskAwaitable<T> : IAwaitable<T>
    {


        public Awaiter.ConfiguredTaskAwaitable<T> Awaitable { get; }


        public ConfiguredTaskAwaitable(Awaiter.ConfiguredTaskAwaitable<T> awaitable)
        {
            Awaitable = awaitable;
        }


        public IAwaiter<T> GetAwaiter() =>
            new ConfiguredTaskAwaiter<T>(Awaitable.GetAwaiter());


        public static implicit operator Awaiter.ConfiguredTaskAwaitable<T>(ConfiguredTaskAwaitable<T> task) => task.Awaitable;


    }
}
