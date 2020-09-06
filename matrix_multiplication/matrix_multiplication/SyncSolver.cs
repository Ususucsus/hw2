using System;

namespace MatrixMultiplication
{
    /// <summary>
    /// Represents class with operations on matrices using only one thread.
    /// </summary>
    public class SyncSolver : ISolver
    {
        /// <summary>
        /// Multiplies matrices.
        /// </summary>
        /// <param name="leftMatrix">Left matrix.</param>
        /// <param name="rightMatrix">Right matrix.</param>
        /// <returns>Result matrix.</returns>
        /// <exception cref="ArgumentNullException">Input matrices are null.</exception>
        /// <exception cref="MatrixMultiplicationException"></exception>
        public int[,] Multiply(int[,] leftMatrix, int[,] rightMatrix)
        {
            if (leftMatrix == null || rightMatrix == null)
            {
                throw new ArgumentNullException();
            }

            if (leftMatrix.GetLength(0) == 0 || leftMatrix.GetLength(1) == 0 ||
                rightMatrix.GetLength(0) == 0 || rightMatrix.GetLength(1) == 0)
            {
                throw new MatrixMultiplicationException("Matrices are with zero dimensions.");
            }

            if (leftMatrix.GetLength(1) != rightMatrix.GetLength(0))
            {
                throw new MatrixMultiplicationException("Not suitable sizes of matrices");
            }

            var resultHeight = leftMatrix.GetLength(0);
            var resultWidth = rightMatrix.GetLength(1);
            var n = leftMatrix.GetLength(1);

            var result = new int[resultHeight, resultWidth];

            for (var i = 0; i < resultHeight; i++)
            {
                for (var j = 0; j < resultWidth; j++)
                {
                    var sum = 0;
                    for (var k = 0; k < n; k++)
                    {
                        sum += (leftMatrix[i, k] * rightMatrix[k, j]);
                    }

                    result[i, j] = sum;
                }

            }

            return result;
        }

        /// <summary>
        /// Returns name of the solver.
        /// </summary>
        /// <returns>Name of the solver.</returns>
        public override string ToString() => "sync";
    }
}