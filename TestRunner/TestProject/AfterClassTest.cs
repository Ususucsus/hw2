using System;
using TestRunner.Attributes;

namespace TestProject
{
    [TestFixture]
    public class AfterClassTest
    {
        public static int ShouldBeFive = 1;

        [AfterClass]
        public static void AfterClass()
        {
            ShouldBeFive += 4;
        }
        
        [Test(typeof(ArgumentNullException))]
        public void Test1()
        {
        }
        
        [Test(typeof(ArgumentNullException))]
        public void Test2()
        {
        }
    }
}