using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimp.SeeSharper.Async
{
    public static class AwaitableExtensions
    {


        #region ConfigureAwait


        public static IAwaitable ConfigureAwait(this IAwaitable awaitable, bool continueOnCapturedContext)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            if (awaitable is IConfigureAwaitable config)
                return config.ConfigureAwait(continueOnCapturedContext);

            return awaitable;
        }


        public static IAwaitable<T> ConfigureAwait<T>(this IAwaitable<T> awaitable, bool continueOnCapturedContext)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            if (awaitable is IConfigureAwaitable<T> config)
                return config.ConfigureAwait(continueOnCapturedContext);

            return awaitable;
        }


        #endregion ConfigureAwait


        #region ToTask


        public static Task ToTask(this IAwaitable awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            if (awaitable is TaskAwaitable task)
                return task.Task;

            return Task.Run(async () => await awaitable);
        }

        public static Task<T> ToTask<T>(this IAwaitable<T> awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            if (awaitable is TaskAwaitable<T> task)
                return task.Task;

            return Task.Run(async () => await awaitable);
        }


        #endregion ToTask


        #region ToValueTask


        public static ValueTask ToValueTask(this IAwaitable awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            if (awaitable is ValueTaskAwaitable task)
                return task.Task;

            return new ValueTask(Task.Run(async () => await awaitable));
        }

        public static ValueTask<T> ToValueTask<T>(this IAwaitable<T> awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            if (awaitable is ValueTaskAwaitable<T> task)
                return task.Task;

            return new ValueTask<T>(Task.Run(async () => await awaitable));
        }


        #endregion ToValueTask


        public static async IAwaitable NoResult<T>(this IAwaitable<T> awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            await awaitable;
        }


        #region Await


        public static void Await(this IAwaitable awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            awaitable.GetAwaiter().GetResult();
        }

        public static T Await<T>(this IAwaitable<T> awaitable)
        {
            if (awaitable is null)
                throw new ArgumentNullException(nameof(awaitable));

            return awaitable.GetAwaiter().GetResult();
        }


        #endregion Await


        #region AwaitAny


        public static async IAwaitable<IAwaitable> AwaitAnyAwaitable(this IEnumerable<IAwaitable> awaitables)
        {
            if (awaitables is null)
                throw new ArgumentNullException(nameof(awaitables));
            if (!awaitables.Any())
                throw new ArgumentException("No awaitables.", nameof(awaitables));

            var tasks = awaitables.ToDictionary(AwaitableExtensions.ToTask);
            return tasks[await Task.WhenAny(tasks.Keys.ToArray()).ConfigureAwait(false)];
        }

        public static async IAwaitable AwaitAny(this IEnumerable<IAwaitable> awaitables) =>
            await await awaitables.AwaitAnyAwaitable();


        public static async IAwaitable<IAwaitable<T>> AwaitAnyAwaitable<T>(this IEnumerable<IAwaitable<T>> awaitables)
        {
            if (awaitables is null)
                throw new ArgumentNullException(nameof(awaitables));
            if (!awaitables.Any())
                throw new ArgumentException("No awaitables.", nameof(awaitables));

            var tasks = awaitables.Select(a => a ?? throw new ArgumentNullException(nameof(awaitables), "At least one awaitable is null."))
                .ToDictionary(AwaitableExtensions.ToTask);
            return tasks[await Task.WhenAny(tasks.Keys.ToArray()).ConfigureAwait(false)];
        }

        public static async IAwaitable<T> AwaitAny<T>(this IEnumerable<IAwaitable<T>> awaitables) =>
            await await awaitables.AwaitAnyAwaitable();


        public static async IAwaitable<IAwaitable> AwaitAnySuccessAwaitable(this IEnumerable<IAwaitable> awaitables)
        {
            if (awaitables is null)
                throw new ArgumentNullException(nameof(awaitables));
            if (!awaitables.Any())
                throw new ArgumentException("No awaitables.", nameof(awaitables));

            var awaits = awaitables.ToList();
            var exceptions = new List<Exception>();

            while (awaits.Count > 0)
                try
                {
                    var awaitable = await awaits.AwaitAnyAwaitable();
                    awaits.Remove(awaitable);
                    await awaitable;
                    return awaitable;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    var aws = new List<IAwaitable>();
                    foreach (var a in awaits)
                        if (a.GetAwaiter().IsCompleted)
                            try
                            {
                                await a;
                                return a;
                            }
                            catch (Exception aex)
                            {
                                aws.Add(a);
                                exceptions.Add(aex);
                            }
                    if (aws.Count > 0)
                        awaits.RemoveAll(a => aws.Contains(a));
                }

            throw new AggregateException(exceptions);
        }

        public static async IAwaitable AwaitAnySuccess(this IEnumerable<IAwaitable> awaitables) =>
            await await awaitables.AwaitAnySuccessAwaitable();


        public static async IAwaitable<IAwaitable<T>> AwaitAnySuccessAwaitable<T>(this IEnumerable<IAwaitable<T>> awaitables)
        {
            if (awaitables is null)
                throw new ArgumentNullException(nameof(awaitables));
            if (!awaitables.Any())
                throw new ArgumentException("No awaitables.", nameof(awaitables));

            var awaits = awaitables.ToList();
            var exceptions = new List<Exception>();

            while (awaits.Count > 0)
                try
                {
                    var awaitable = await awaits.AwaitAnyAwaitable();
                    awaits.Remove(awaitable);
                    await awaitable;
                    return awaitable;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    var aws = new List<IAwaitable<T>>();
                    foreach (var a in awaits)
                        if (a.GetAwaiter().IsCompleted)
                            try
                            {
                                await a;
                                return a;
                            }
                            catch (Exception aex)
                            {
                                aws.Add(a);
                                exceptions.Add(aex);
                            }
                    if (aws.Count > 0)
                        awaits.RemoveAll(a => aws.Contains(a));
                }

            throw new AggregateException(exceptions);
        }

        public static async IAwaitable<T> AwaitAnySuccess<T>(this IEnumerable<IAwaitable<T>> awaitables) =>
            await await awaitables.AwaitAnySuccessAwaitable();


        #endregion AwaitAny


        #region AwaitAll


        public static async IAwaitable AwaitAll(this IEnumerable<IAwaitable> awaitables)
        {
            if (awaitables is null)
                throw new ArgumentNullException(nameof(awaitables));

            if (!awaitables.Any())
                return;

            await Task.WhenAll(awaitables.Select(a => a?.ToTask() ?? throw new ArgumentNullException(nameof(awaitables), "At least one awaitable is null.")).ToArray())
                .ConfigureAwait(false);
        }


        public static async IAwaitable<IEnumerable<T>> AwaitAll<T>(this IEnumerable<IAwaitable<T>> awaitables)
        {
            if (awaitables is null)
                throw new ArgumentNullException(nameof(awaitables));

            if (!awaitables.Any())
                return Array.Empty<T>();

            return await Task.WhenAll(awaitables.Select(a => a?.ToTask() ?? throw new ArgumentNullException(nameof(awaitables), "At least one awaitable is null.")).ToArray())
                .ConfigureAwait(false);
        }


        #endregion


    }
}
