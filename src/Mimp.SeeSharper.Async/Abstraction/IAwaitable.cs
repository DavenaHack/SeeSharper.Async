using System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async.Abstraction
{
    [AsyncMethodBuilder(typeof(AwaitableAsyncMethodBuilder))]
    public interface IAwaitable
    {


        public IAwaiter GetAwaiter();


    }

    [AsyncMethodBuilder(typeof(AwaitableAsyncMethodBuilder<>))]
    public interface IAwaitable<T>
    {


        public IAwaiter<T> GetAwaiter();


    }
}
