using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MatrixMultiplication
{
    /// <summary>
    /// Represents static class for loading, saving and generating matrices.
    /// </summary>
    public static class MatrixHelper
    {
        /// <summary>
        /// Generates matrix with random values from 0 to 9 with specified dimensions.
        /// </summary>
        /// <param name="height">Number of rows in matrix.</param>
        /// <param name="width">Number of columns in matrix.</param>
        public static int[,] GetRandomMatrix(int height, int width)
        {
            var random = new Random();

            var matrix = new int[height, width];

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    matrix[i, j] = random.Next(10);
                }
            }

            return matrix;
        }

        /// <summary>
        /// Loads matrix from file.
        /// </summary>
        /// <param name="path">Path to file with matrix.</param>
        /// <returns>Matrix list.</returns>
        /// <exception cref="FormatException">Input file is in invalid format.</exception>
        public static int[,] LoadMatrix(string path)
        {
            var lines = File.ReadAllLines(path);

            int height;
            int width;

            try
            {
                var dimensionsSplit = lines[0].Trim().Split();
                height = int.Parse(dimensionsSplit[0]);
                width = int.Parse(dimensionsSplit[1]);
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new FormatException("Bad dimensions line", exception);
            }
            catch (OverflowException exception)
            {
                throw new FormatException("Matrix too big", exception);
            }

            if (lines.Length != height + 1)
            {
                throw new FormatException("Too few lines");
            }

            var matrix = new int[height, width];

            for (var i = 0; i < height; i++)
            {
                var line = lines[i + 1];

                var integers = line.Trim().Split().Select(int.Parse).ToList();

                if (integers.Count != width)
                {
                    throw new FormatException("Too few integers in line");
                }

                for (var j = 0; j < width; j++)
                {
                    matrix[i, j] = integers[j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Saves matrix to file.
        /// </summary>
        /// <param name="matrix">Matrix to be saved.</param>
        /// <param name="path">Output file path. If not exist will be created.</param>
        public static void SaveMatrix(int[,] matrix, string path)
        {
            var height = matrix.GetLength(0);
            var width = matrix.GetLength(1);

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{height} {width}\n");

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    stringBuilder.Append($"{matrix[i, j]} ");
                }

                stringBuilder.Append(Environment.NewLine);
            }

            var output = stringBuilder.ToString();

            File.WriteAllText(path, output);
        }
    }
}