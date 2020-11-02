using System;
using TestRunner.Attributes;

namespace TestProject
{
    [TestFixture]
    public class AfterTest
    {
        public static int ShouldBeFive = 1;

        [After]
        public void After()
        {
            ShouldBeFive += 2;
        }

        [Test(typeof(ArgumentNullException))]
        public void Test1()
        {
            throw new ArgumentNullException();
        }
        
        [Test(typeof(ArgumentNullException))]
        public void Test2()
        {
            throw new ArgumentNullException();
        }
    }
}