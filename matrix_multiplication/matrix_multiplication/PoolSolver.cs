using System;
using System.Collections.Generic;
using System.Threading;

namespace MatrixMultiplication
{
    /// <summary>
    /// Represents class with operations on matrices using multithreading.
    /// </summary>
    public class PoolSolver : ISolver
    {
        /// <summary>
        /// Number of threads to be used in calculations.
        /// </summary>
        private readonly int numberOfThreads = 4;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolSolver"/>
        /// </summary>
        public PoolSolver()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolSolver"/>
        /// </summary>
        /// <param name="poolSize">Number of threads to be used.</param>
        public PoolSolver(int poolSize)
        {
            this.numberOfThreads = poolSize;
        }

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

            var chunks = this.GetChunks(leftMatrix, numberOfThreads);
            var chunkResults = new List<int[,]>(numberOfThreads);
            var threadPool = new Thread[numberOfThreads];

            for (var i = 0; i < numberOfThreads; i++)
            {
                var index = i;
                chunkResults.Add(null);

                threadPool[i] = new Thread(() =>
                {
                    var syncSolver = new SyncSolver();
                    chunkResults[index] = syncSolver.Multiply(chunks[index], rightMatrix);
                });
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threadPool[i].Start();
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threadPool[i].Join();
            }

            return this.JoinChunks(chunkResults, resultHeight, resultWidth);
        }

        /// <summary>
        /// Returns name of the solver.
        /// </summary>
        /// <returns>Name of the solver.</returns>
        public override string ToString() => "pool";

        /// <summary>
        /// Splits matrix to chunks by rows.
        /// </summary>
        /// <param name="matrix">Matrix.</param>
        /// <param name="chunksAmount">Amount of chunks</param>
        /// <returns>List of matrices.</returns>
        private List<int[,]> GetChunks(int[,] matrix, int chunksAmount)
        {
            var height = matrix.GetLength(0);
            var width = matrix.GetLength(1);

            var chunks = new List<int[,]>();
            var chunkHeight = height / chunksAmount;

            for (var k = 0; k < chunksAmount; k++)
            {
                var startRow = k * chunkHeight;
                var endRow = (k == numberOfThreads - 1 ? height : (k + 1) * chunkHeight);

                chunks.Add(new int[endRow - startRow, width]);

                for (var i = startRow; i < endRow; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        chunks[k][i - startRow, j] = matrix[i, j];
                    }
                }
            }

            return chunks;
        }

        /// <summary>
        /// Joins chunks to one matrix.
        /// </summary>
        /// <param name="chunks">List of matrices.</param>
        /// <param name="height">Height of result matrix.</param>
        /// <param name="width">Width of result matrix.</param>
        /// <returns>Matrix.</returns>
        private int[,] JoinChunks(IReadOnlyList<int[,]> chunks, int height, int width)
        {
            var result = new int[height, width];

            var currentRow = 0;
            for (var i = 0; i < numberOfThreads; i++)
            {
                for (var j = 0; j < chunks[i].GetLength(0); j++)
                {
                    for (var k = 0; k < chunks[i].GetLength(1); k++)
                    {
                        result[currentRow, k] = chunks[i][j, k];
                    }

                    currentRow++;
                }
            }

            return result;
        }
    }
}