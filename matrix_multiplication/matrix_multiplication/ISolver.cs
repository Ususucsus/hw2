namespace MatrixMultiplication
{
    /// <summary>
    /// Represents interface for operations on matrices.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Multiplies matrices.
        /// </summary>
        /// <param name="leftMatrix">Left matrix.</param>
        /// <param name="rightMatrix">Right matrix.</param>
        /// <returns>Result matrix.</returns>
        int[,] Multiply(int[,] leftMatrix, int[,] rightMatrix);
    }
}