using Mimp.SeeSharper.Async.Abstraction;
using Awaiter = System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public static class YieldAwaitableExtensions
    {


        public static IAwaitable ToAwaitable(this Awaiter.YieldAwaitable task) =>
            new YieldAwaitable(task);


    }
}
