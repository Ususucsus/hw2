using System;

namespace lazy
{
    /// <summary>
    /// Represents class for single thread lazy calculations.
    /// </summary>
    /// <typeparam name="T">Type of supplier function return value.</typeparam>
    public class SyncLazy<T> : ILazy<T>
    {
        private readonly Func<T> supplier;

        private bool isCached;
        private T cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncLazy{T}"/> class.
        /// </summary>
        /// <param name="supplier">Function to be lazy calculated.</param>
        public SyncLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }
        
        /// <summary>
        /// Returns result of supplier function.
        /// </summary>
        /// <returns>Result of supplier function.</returns>
        public T Get()
        {
            if (!this.isCached)
            {
                this.cache = this.supplier();
                this.isCached = true;
            }

            return this.cache;
        }
    }
}