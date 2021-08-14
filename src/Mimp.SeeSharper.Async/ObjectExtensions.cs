using Mimp.SeeSharper.Async.Abstraction;
using System;

namespace Mimp.SeeSharper.Async
{
    public static class ObjectExtensions
    {


        #region DisposeAsync


        public static IAwaitable DisposeAsync(this object disposable)
        {
            if (disposable is null)
                throw new ArgumentNullException(nameof(disposable));

            switch (disposable)
            {
                case IAsyncDisposable async: return async.DisposeAsync().ToAwaitable();
                case IAwaitableDisposable await: return await.DisposeAsync();
                case IDisposable d:
                    d.Dispose();
                    return Awaitables.Empty;
                default:
                    return Awaitables.Empty;
                    //return Awaitables.Run(async () =>
                    //{
                    //    foreach (var i in disposable.GetType().GetInterfaces())
                    //        if (i.FullName == "System.IAsyncDisposable")
                    //        {
                    //            var method = i.GetMethod("DisposeAsync");
                    //            if (method is not null)
                    //            {
                    //                var result = method.Invoke(disposable, Array.Empty<object>());
                    //                if (result is ValueTask vt)
                    //                {
                    //                    await vt;
                    //                    return;
                    //                }
                    //            }
                    //        }
                    //});
            };
        }


        public static async IAwaitable<R> UsingAsync<T, R>(this T disposable, Func<T, IAwaitable<R>> func)
        {
            if (disposable is null)
                throw new ArgumentNullException(nameof(disposable));
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                return await func(disposable);
            }
            finally
            {
                await disposable.DisposeAsync();
            }
        }

        public static async IAwaitable<R> UsingAsync<T, R>(this T disposable, Func<T, R> func)
        {
            if (disposable is null)
                throw new ArgumentNullException(nameof(disposable));
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                return func(disposable);
            }
            finally
            {
                await disposable.DisposeAsync();
            }
        }

        public static async IAwaitable UsingAsync<T>(this T disposable, Func<T, IAwaitable> action)
        {
            if (disposable is null)
                throw new ArgumentNullException(nameof(disposable));
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                await action(disposable);
            }
            finally
            {
                await disposable.DisposeAsync();
            }
        }

        public static async IAwaitable UsingAsync<T, R>(this T disposable, Action<T> action)
        {
            if (disposable is null)
                throw new ArgumentNullException(nameof(disposable));
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                action(disposable);
            }
            finally
            {
                await disposable.DisposeAsync();
            }
        }


        #endregion


    }
}
