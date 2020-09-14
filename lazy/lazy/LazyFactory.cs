using System;

namespace lazy
{
    /// <summary>
    /// Represents static class for creating lazy class.
    /// </summary>
    public static class LazyFactory
    {
        /// <summary>
        /// Creates not thread-safe lazy calculation class.
        /// </summary>
        /// <typeparam name="T">Supplier return type.</typeparam>
        /// <param name="supplier">Function to be calculated with lazy calculation class.</param>
        /// <returns></returns>
        public static ILazy<T> CreateSyncLazy<T>(Func<T> supplier)
        {
            return new SyncLazy<T>(supplier);
        }

        /// <summary>
        /// Creates thread-safe lazy calculation class.
        /// </summary>
        /// <typeparam name="T">Supplier return type.</typeparam>
        /// <param name="supplier">Function to be calculated with lazy calculation class.</param>
        /// <returns></returns>
        public static ILazy<T> CreateConcurrentLazy<T>(Func<T> supplier)
        {
            return new ConcurrentLazy<T>(supplier);
        }
    }
}