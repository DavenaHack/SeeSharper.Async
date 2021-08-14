using System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async.Abstraction
{
    public interface IAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {


        public bool IsCompleted { get; }


        public void GetResult();


    }


    public interface IAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
    {


        public bool IsCompleted { get; }


        public T GetResult();


    }
}
