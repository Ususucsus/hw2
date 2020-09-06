using System;
using MatrixMultiplication;
using NUnit.Framework;

namespace MatrixMultiplicationsTests
{
    public class SyncMultiplicationTests
    {
        private SyncSolver solver;

        [SetUp]
        public void Setup()
        {
            this.solver = new SyncSolver();
        }

        [Test]
        public void MultiplicationShouldWorkCorrectWithSquareMatrices()
        {
            var leftMatrix = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };

            var rightMatrix = new[,]
            {
                {9, 8, 7},
                {6, 5, 4},
                {3, 2, 1}
            };

            var result = solver.Multiply(leftMatrix, rightMatrix);

            var expectedResult = new[,]
            {
                {30, 24, 18},
                {84, 69, 54},
                {138, 114, 90}
            };

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MultiplicationShouldWorkCorrectWithNonSquareMatrices()
        {
            var leftMatrix = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
                {10, 11, 12}
            };

            var rightMatrix = new[,]
            {
                {1, 2, 3, 4, 5},
                {6, 7, 8, 9, 10},
                {11, 12, 13, 14, 15}
            };

            var result = solver.Multiply(leftMatrix, rightMatrix);

            var expectedResult = new[,]
            {
                {46, 52, 58, 64, 70},
                {100, 115, 130, 145, 160},
                {154, 178, 202, 226, 250},
                {208, 241, 274, 307, 340}
            };

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MultiplicationWithNullMatricesShouldThrowException()
        {
            var leftMatrix = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
                {10, 11, 12}
            };

            var rightMatrix = new[,]
            {
                {1, 2, 3, 4, 5},
                {6, 7, 8, 9, 10},
                {11, 12, 13, 14, 15}
            };

            Assert.Throws<ArgumentNullException>(() => solver.Multiply(leftMatrix, null));
            Assert.Throws<ArgumentNullException>(() => solver.Multiply(null, rightMatrix));
            Assert.Throws<ArgumentNullException>(() => solver.Multiply(null, null));
        }

        [Test]
        public void MultiplicationWithNotSuitableSizesShouldThrowException()
        {
            var leftMatrix = new[,]
            {
                {1, 2, 3, 4},
                {4, 5, 6, 1},
                {7, 8, 9, 1},
                {10, 11, 12, 1},
            };

            var rightMatrix = new[,]
            {
                {1, 2, 3, 4, 5},
                {6, 7, 8, 9, 10},
                {11, 12, 13, 14, 15}
            };

            Assert.Throws<MatrixMultiplicationException>(() => solver.Multiply(leftMatrix, rightMatrix));
        }

        [Test]
        public void MultiplicationWithEmptyMatricesShouldThrowException()
        {
            var leftMatrix = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
                {10, 11, 12}
            };

            var rightMatrix = new[,]
            {
                {1, 2, 3, 4, 5},
                {6, 7, 8, 9, 10},
                {11, 12, 13, 14, 15}
            };

            Assert.Throws<MatrixMultiplicationException>(() => solver.Multiply(new int[,] { }, rightMatrix));
            Assert.Throws<MatrixMultiplicationException>(() => solver.Multiply(leftMatrix, new int[,] { }));
            Assert.Throws<MatrixMultiplicationException>(() => solver.Multiply(new int[,] { }, new int[,] { }));
        }
    }
}