using Mimp.SeeSharper.Async.Abstraction;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public static class ConfiguredValueTaskAwaitableExtensions
    {


        public static IAwaitable ToAwaitable(this Awaiter.ConfiguredValueTaskAwaitable task) =>
            new ConfiguredValueTaskAwaitable(task);

        public static IAwaitable<T> ToAwaitable<T>(this Awaiter.ConfiguredValueTaskAwaitable<T> task) =>
            new ConfiguredValueTaskAwaitable<T>(task);


    }
}
