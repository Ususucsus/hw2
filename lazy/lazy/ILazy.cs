namespace lazy
{
    /// <summary>
    /// Represents interface for lazy calculations classes.
    /// </summary>
    /// <typeparam name="T">Calculated function return type.</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Returns result of supplier function.
        /// </summary>
        /// <returns>Result of supplier function.</returns>
        T Get();
    }
}