using System;

namespace TestRunner.Attributes
{
    /// <summary>
    /// Represents attribute used for methods that represents tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        public Type Expected { get; }
        
        public string? IgnoreReason { get; }

        public bool Ignore => IgnoreReason != null;
        
        /// <summary>
        /// Initalizes a new instance of the <see cref="TestAttribute"/> class.
        /// </summary>
        /// <param name="expected">Type of exception expected as assertion exception.</param>
        /// <param name="ignoreReason">Ignore reason. If not empty test will be ignored.</param>
        public TestAttribute(Type expected, string? ignoreReason = null)
        {
            Expected = expected;
            IgnoreReason = ignoreReason;
        }
    }
}