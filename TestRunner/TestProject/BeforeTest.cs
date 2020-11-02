using System;
using TestRunner.Attributes;

namespace TestProject
{
    [TestFixture]
    public class BeforeTest
    {
        public static int ShouldBeFive = 1;

        [Before]
        public void Before()
        {
            ShouldBeFive += 2;
        }

        [Test(typeof(ArgumentNullException))]
        public void Test3()
        {
            throw new ArgumentNullException();
        }
        
        [Test(typeof(ArgumentNullException))]
        public void Test4()
        {
            throw new ArgumentNullException();
        }
    }
}