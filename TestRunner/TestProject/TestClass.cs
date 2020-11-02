using System;
using TestRunner.Attributes;

namespace TestProject
{
    public class TestClass
    {
        [Test(typeof(ArgumentNullException))]
        public void PassedMethod1()
        {
            
        }

        [Test(typeof(ArgumentNullException))]
        public void PassedMethod2()
        {
            
        }
        
        
        [Test(typeof(ArgumentNullException), "ignore reason")]
        public void IgnoredTestMethod2()
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}