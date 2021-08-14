using System.Threading;

namespace Mimp.SeeSharper.Async.Abstraction
{
    public interface IEnumerableAwaiter<T>
    {


        public IAwaitable<T> GetNextAsync(CancellationToken cancellationToken);

        public IAwaitable<bool> AwaitNextAsync(CancellationToken cancellationToken);


    }
}
