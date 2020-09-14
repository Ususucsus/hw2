using lazy;
using NUnit.Framework;

namespace lazyTests
{
    public class LazyFactoryTests
    {
        [Test]
        public void CreateSyncLazyShouldReturnLazy()
        {
            Assert.IsInstanceOf<SyncLazy<int>>(LazyFactory.CreateSyncLazy(() => 5));
        }

        [Test]
        public void CreateSyncLazyReturnsLazyWithObjects()
        {
            Assert.IsInstanceOf<SyncLazy<string>>(LazyFactory.CreateSyncLazy(() => "test"));
        }

        [Test]
        public void CreateConcurrentLazyShouldReturnLazy()
        {
            Assert.IsInstanceOf<ConcurrentLazy<int>>(LazyFactory.CreateConcurrentLazy(() => 5));
        }

        [Test]
        public void CreateConcurrentLazyReturnsLazyWithObjects()
        {
            Assert.IsInstanceOf<ConcurrentLazy<string>>(LazyFactory.CreateConcurrentLazy(() => "test"));
        }
    }
}