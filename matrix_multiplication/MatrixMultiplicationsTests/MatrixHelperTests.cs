using System;
using System.Collections.Generic;
using System.IO;
using MatrixMultiplication;
using NUnit.Framework;

namespace MatrixMultiplicationsTests
{
    public class MatrixHelperTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void GetRandomMatrixShouldReturnMatrixWithSpecifiedDimensions()
        {
            var matrix = MatrixHelper.GetRandomMatrix(10, 20);
            Assert.AreEqual(10, matrix.GetLength(0));
            Assert.AreEqual(20, matrix.GetLength(1));
        }

        [Test]
        public void LoadFromFileShouldWorkCorrect()
        {
            File.WriteAllLines("input.txt", new List<string>
            {
                "2 3",
                "1 2 3",
                "4 5 6"
            });

            var matrix = MatrixHelper.LoadMatrix("input.txt");
            Assert.AreEqual(new[,]
            {
                {1, 2, 3},
                {4, 5, 6}
            }, matrix);
        }

        [Test]
        public void LoadWithoutDimensionsShouldThrowException()
        {
            File.WriteAllLines("input.txt", new List<string>
            {
                "1 2 3",
                "4 5 6"
            });

            Assert.Throws<FormatException>(() => MatrixHelper.LoadMatrix("input.txt"));
        }

        [Test]
        public void LoadWithInvalidDimensionsShouldThrowException()
        {
            File.WriteAllLines("input.txt", new List<string>
            {
                "100",
                "1 2 3",
                "4 5 6"
            });

            Assert.Throws<FormatException>(() => MatrixHelper.LoadMatrix("input.txt"));
        }

        [Test]
        public void LoadWithWrongHeightShouldThrowException()
        {
            File.WriteAllLines("input.txt", new List<string>
            {
                "100 3",
                "1 2 3",
                "4 5 6"
            });

            Assert.Throws<FormatException>(() => MatrixHelper.LoadMatrix("input.txt"));
        }

        [Test]
        public void LoadWithWrongWidthShouldThrowException()
        {
            File.WriteAllLines("input.txt", new List<string>
            {
                "2 3",
                "1 2 3",
                "4 5"
            });

            Assert.Throws<FormatException>(() => MatrixHelper.LoadMatrix("input.txt"));
        }

        [Test]
        public void LoadWithNonNumericCharactersShouldThrowException()
        {
            File.WriteAllLines("input.txt", new List<string>
            {
                "2 3",
                "1 a 3",
                "4 5 6"
            });

            Assert.Throws<FormatException>(() => MatrixHelper.LoadMatrix("input.txt"));
        }

        
    }
}