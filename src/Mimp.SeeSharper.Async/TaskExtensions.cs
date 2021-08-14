using Mimp.SeeSharper.Async.Abstraction;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public static class TaskExtensions
    {


        public static IAwaitable ToAwaitable(this Task task) =>
            new TaskAwaitable(task);

        public static IAwaitable<T> ToAwaitable<T>(this Task<T> task) =>
            new TaskAwaitable<T>(task);


    }
}
