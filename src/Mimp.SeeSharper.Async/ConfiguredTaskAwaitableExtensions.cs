using Mimp.SeeSharper.Async.Abstraction;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public static class ConfiguredTaskAwaitableExtensions
    {


        public static IAwaitable ToAwaitable(this Awaiter.ConfiguredTaskAwaitable task) =>
            new ConfiguredTaskAwaitable(task);

        public static IAwaitable<T> ToAwaitable<T>(this Awaiter.ConfiguredTaskAwaitable<T> task) =>
            new ConfiguredTaskAwaitable<T>(task);


    }
}
