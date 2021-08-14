using Mimp.SeeSharper.Async.Abstraction;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public static class ValueTaskExtensions
    {


        public static IAwaitable ToAwaitable(this ValueTask task) =>
            new ValueTaskAwaitable(task);

        public static IAwaitable<T> ToAwaitable<T>(this ValueTask<T> task) =>
            new ValueTaskAwaitable<T>(task);


    }
}
