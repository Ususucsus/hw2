using System;

namespace lazy
{
    /// <summary>
    /// Represents thread-safe class for lazy calculations.
    /// </summary>
    /// <typeparam name="T">Type of supplier function return value.</typeparam>
    public class ConcurrentLazy<T> : ILazy<T>
    {
        private readonly Func<T> supplier;

        private bool isCached;
        private T cache;

        private readonly object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentLazy{T}"/> class.
        /// </summary>
        /// <param name="supplier">Function to be lazy calculated.</param>
        public ConcurrentLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

        /// <summary>
        /// Returns result of supplier function.
        /// </summary>
        /// <returns>Result of supplier function.</returns>
        public T Get()
        {
            if (!isCached)
            {
                lock (lockObject)
                {
                    if (!isCached)
                    {
                        this.cache = this.supplier();
                        this.isCached = true;
                    }
                }
            }

            return this.cache;
        }
    }
}