using System;
using TestRunner.Attributes;

namespace TestProject
{
    public class ClassTest
    {
        [Test(typeof(ArgumentNullException))]
        public void FailedTestMethod()
        {
            throw new ArgumentNullException();
        }

        [Test(typeof(ArgumentOutOfRangeException))]
        public void CrashedTestMethod()
        {
            throw new ArgumentNullException();
        }

        [Test(typeof(ArgumentNullException), "ignore reason")]
        public void IgnoredTestMethod1()
        {
            throw new ArgumentNullException();
        }
    }
}