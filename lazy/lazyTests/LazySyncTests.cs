using lazy;
using NUnit.Framework;

namespace lazyTests
{
    public class LazySyncTests
    {
        [Test]
        public void LazySyncReturnsSupplierResult()
        {
            var lazy = LazyFactory.CreateSyncLazy(() => 5);

            Assert.AreEqual(5, lazy.Get());
        }

        [Test]
        public void LazySyncReturnsSupplierResultIfResultIsNull()
        {
            var lazy = LazyFactory.CreateSyncLazy<string>(() => null);

            Assert.Null(lazy.Get());
        }

        [Test]
        public void LazySyncRunsSupplierFunctionOnlyOnce()
        {
            var callNumber = 0;
            var lazy = LazyFactory.CreateSyncLazy(() =>
            {
                callNumber++;
                return "test";
            });

            lazy.Get();
            Assert.AreEqual(1, callNumber);

            lazy.Get();
            Assert.AreEqual(1, callNumber);
        }

        [Test]
        public void LazySyncRunsSupplierFunctionOnlyOnceIfResultIsNull()
        {
            var callNumber = 0;
            var lazy = LazyFactory.CreateSyncLazy<string>(() =>
            {
                callNumber++;
                return null;
            });

            lazy.Get();
            Assert.AreEqual(1, callNumber);

            lazy.Get();
            Assert.AreEqual(1, callNumber);
        }
    }
}