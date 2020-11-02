using System;
using TestRunner.Attributes;

namespace TestProject
{
    [TestFixture]
    public class BeforeClassTest
    {
        public static int ShouldBeFive = 1;

        [BeforeClass]
        public static void BeforeClass()
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