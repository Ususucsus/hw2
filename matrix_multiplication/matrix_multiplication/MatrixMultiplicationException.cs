using System;
using System.Runtime.Serialization;

namespace MatrixMultiplication
{
    [Serializable]
    public class MatrixMultiplicationException : Exception
    {
        public MatrixMultiplicationException()
        {
        }

        public MatrixMultiplicationException(string message) : base(message)
        {
        }

        public MatrixMultiplicationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MatrixMultiplicationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}