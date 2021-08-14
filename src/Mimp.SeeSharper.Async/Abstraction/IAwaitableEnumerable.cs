using System.Threading;

namespace Mimp.SeeSharper.Async.Abstraction
{
    public interface IAwaitableEnumerable<T>
    {


        public IEnumerableAwaiter<T> GetAwaiter(CancellationToken cancellationToken);


    }
}
