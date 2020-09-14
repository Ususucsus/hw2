using System.Threading;
using lazy;
using NUnit.Framework;

namespace lazyTests
{
    public class LazyConcurrentTests
    {
        [Test]
        public void RaceTest()
        {
            const int threadNumber = 4;

            for (var i = 0; i < 1000; i++)
            {
                var callNumber = 0;
                var lazy = LazyFactory.CreateConcurrentLazy(() =>
                {
                    callNumber++;
                    return "test";
                });


                var threadPool = new Thread[threadNumber];
                var results = new string[threadNumber];

                for (var j = 0; j < threadNumber; j++)
                {
                    var index = j;
                    threadPool[j] = new Thread(() => { results[index] = lazy.Get(); });
                }

                for (var j = 0; j < threadNumber; j++)
                {
                    threadPool[j].Start();
                }

                for (var j = 0; j < threadNumber; j++)
                {
                    threadPool[j].Join();
                }

                for (var j = 0; j < threadNumber; j++)
                {
                    Assert.AreEqual("test", results[j]);
                }

                Assert.AreEqual(1, callNumber);
            }
        }
    }
}