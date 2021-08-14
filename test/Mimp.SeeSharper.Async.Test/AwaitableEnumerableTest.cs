using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.Async.Test
{
    [TestClass]
    public class AwaitableEnumerableTest
    {


        [TestMethod]
        public void TestAwaitableEnumerable()
        {
            TestAwaitableEnumerableAsync().Await();
        }

        private async IAwaitable TestAwaitableEnumerableAsync()
        {
            var result = new List<int>();

            await foreach (var x in new IAwaitable<int>[] {
                Awaitables.Result(1),
                Awaitables.Result(2),
                Awaitables.Result(3),
            }.ToAwaitable())
                result.Add(x);

            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }));
        }


        [TestMethod]
        public void TestYield()
        {

            var isWaiting = false;

            var awaiter = Awaitables.Yield(async (Func<int, IAwaitable> yield) =>
            {
                await yield(1);
                isWaiting = true;

                await yield(2);

                return;

#pragma warning disable CS0162 // Unreachable code detected
                await yield(-1);
#pragma warning restore CS0162 // Unreachable code detected
            }).GetAwaiter();

            Assert.ThrowsException<InvalidOperationException>(() => awaiter.GetNextAsync().Await());

            Assert.IsTrue(awaiter.AwaitNextAsync().Await());
            //Assert.IsFalse(isWaiting);
            Assert.AreEqual(1, awaiter.GetNextAsync().Await());
            Assert.AreEqual(1, awaiter.GetNextAsync().Await());

            Assert.IsTrue(awaiter.AwaitNextAsync().Await());
            Assert.AreEqual(2, awaiter.GetNextAsync().Await());

            Assert.IsFalse(awaiter.AwaitNextAsync().Await());
            Assert.ThrowsException<InvalidOperationException>(() => awaiter.GetNextAsync().Await());


            // test continue
            var run = false;
            var awaitableAwaiter = Awaitables.Yield(async (Func<IAwaitable<int>, IAwaitable> yield) =>
            {
                await yield(Awaitables.Run(async () =>
                {
                    await Awaitables.Delay(1000);
                    run = true;
                    return 1;
                }));
                await yield(Awaitables.Result(2));
            }).GetAwaiter();

            awaitableAwaiter.AwaitNextAsync().Await();
            Assert.IsFalse(run);

            awaitableAwaiter.AwaitNextAsync().Await();
            awaitableAwaiter.GetNextAsync().Await();

            Awaitables.Delay(1500).Await();
            Assert.IsTrue(run);

        }


    }
}
