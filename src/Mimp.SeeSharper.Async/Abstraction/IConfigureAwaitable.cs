namespace Mimp.SeeSharper.Async.Abstraction
{
    public interface IConfigureAwaitable : IAwaitable
    {


        public IAwaitable ConfigureAwait(bool continueOnCapturedContext);


    }


    public interface IConfigureAwaitable<T> : IAwaitable<T>
    {


        public IAwaitable<T> ConfigureAwait(bool continueOnCapturedContext);


    }
}
