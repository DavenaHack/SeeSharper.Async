using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.Async.Test
{
    [TestClass]
    public class AwaitablesTest
    {


        [TestMethod]
        public void TestAwaitAny()
        {
            Assert.ThrowsException<ArgumentException>(() => Awaitables.AwaitAnyAwaitable().Await());

            var first = Awaitables.Delay(1000);
            Assert.AreSame(first, Awaitables.AwaitAnyAwaitable(Awaitables.Delay(2000), first, Awaitables.Delay(2000)).Await());

            Assert.ThrowsException<Exception>(() => Awaitables.AwaitAny(Awaitables.Exception(new Exception())).Await());


            Assert.ThrowsException<ArgumentException>(() => Awaitables.AwaitAny<int>().Await());

            Assert.AreEqual(1, Awaitables.AwaitAny(Awaitables.Run(async () =>
            {
                await Awaitables.Delay(2000);
                return 2;
            }), Awaitables.Run(async () =>
            {
                await Awaitables.Delay(1000);
                return 1;
            }), Awaitables.Run(async () =>
            {
                await Awaitables.Delay(2000);
                return 2;
            })).Await());

            Assert.ThrowsException<Exception>(() => Awaitables.AwaitAny(Awaitables.Exception<int>(new Exception())).Await());
        }


        [TestMethod]
        public void TestAwaitAnyCancel()
        {
            IEnumerable<IAwaitable> awaitables = null;

            var success = Awaitables.AwaitAnyAwaitable(cancellationToken =>
                awaitables = new[] {
                    Awaitables.Delay(1000, cancellationToken),
                    Awaitables.Delay(2000, cancellationToken),
                    Awaitables.Delay(2000, cancellationToken),
                }
            ).Await();

            Assert.IsTrue(success.GetAwaiter().IsCompleted);

            foreach (var a in awaitables)
                if (a != success)
                    try
                    {
                        a.Await();
                        Assert.Fail();
                    }
                    catch (OperationCanceledException)
                    {

                    }
            IEnumerable<IAwaitable<int>> awaitablesT = null;

            var successT = Awaitables.AwaitAnyAwaitable(cancellationToken =>
                awaitablesT = new[] {
                    Awaitables.Run(async () => {
                        await Awaitables.Delay(1000, cancellationToken);
                        return 1;
                    }),
                    Awaitables.Run(async () => {
                        await Awaitables.Delay(2000, cancellationToken);
                        return 2;
                    }),
                    Awaitables.Run(async () => {
                        await Awaitables.Delay(2000, cancellationToken);
                        return 2;
                    }),
                }
            ).Await();

            Assert.IsTrue(successT.GetAwaiter().IsCompleted);

            foreach (var a in awaitablesT)
                if (a != successT)
                    try
                    {
                        a.Await();
                        Assert.Fail();
                    }
                    catch (OperationCanceledException)
                    {
                    }
        }


        [TestMethod]
        public void TestAwaitAnySuccess()
        {
            Assert.ThrowsException<ArgumentException>(() => Awaitables.AwaitAnySuccessAwaitable().Await());

            var success = Awaitables.Delay(2000);
            Assert.AreSame(success, Awaitables.AwaitAnySuccessAwaitable(Awaitables.Run(async () =>
            {
                await Awaitables.Delay(1000);
                throw new Exception();
            }), success, Awaitables.Delay(3000)).Await());

            Assert.ThrowsException<AggregateException>(() => Awaitables.AwaitAnySuccess(Awaitables.Exception(new Exception())).Await());


            Assert.ThrowsException<ArgumentException>(() => Awaitables.AwaitAnySuccess<int>().Await());

            Assert.AreEqual(2, Awaitables.AwaitAnySuccess(Awaitables.Run<int>(async () =>
            {
                await Awaitables.Delay(1000);
                throw new Exception();
            }), Awaitables.Run(async () =>
            {
                await Awaitables.Delay(2000);
                return 2;
            }), Awaitables.Run(async () =>
            {
                await Awaitables.Delay(3000);
                return 3;
            })).Await());

            Assert.ThrowsException<AggregateException>(() => Awaitables.AwaitAnySuccess(Awaitables.Exception<int>(new Exception())).Await());
        }


        [TestMethod]
        public void TestAwaitAll()
        {
            Awaitables.AwaitAll().Await();

            var awaits = new IAwaitable[] {
                Awaitables.Delay(1000),
                Awaitables.Delay(2000),
                Awaitables.Delay(3000)
            };

            Awaitables.AwaitAll(awaits).Await();

            Assert.IsTrue(awaits.All(a => a.GetAwaiter().IsCompleted));

            Assert.ThrowsException<Exception>(() => Awaitables.AwaitAll(Awaitables.Exception(new Exception())).Await());

            var result = Awaitables.AwaitAll(new IAwaitable<int>[] {
                Awaitables.Run(async () => {
                    await Awaitables.Delay(1000);
                    return 1;
                }), Awaitables.Run(async () => {
                    await Awaitables.Delay(2000);
                    return 2;
                }), Awaitables.Run(async () => {
                    await Awaitables.Delay(3000);
                    return 3;
                }),
            }).Await();

            var excpect = new[] { 1, 2, 3 };
            Assert.IsTrue(excpect.All(x => result.Contains(x)) && result.All(x => excpect.Contains(x)));
        }


    }
}
