using System;
using TestRunner.Attributes;

namespace TestRunnerTests.ClassesForTests
{
    [TestFixture]
    public class ClassWithTestFunctions
    {
        [Test(typeof(ArgumentNullException))]
        public void TestMethod1() {}
        
        [Test(typeof(ArgumentNullException))]
        public void TestMethod2() {}
        
        public void NotTestMethod() {}
        
        [Before]
        public void BeforeMethod1() {}
        
        [Before]
        public void BeforeMethod2() {}
        
        [Before]
        [After]
        public void BeforeAndAfterMethod() {}
        
        [After]
        public void AfterMethod1() {}
        
        [After]
        public void AfterMethod2() {}
        
        public void CommonMethod() {}
        
        [BeforeClass]
        public void NotStaticBeforeClassMethod() {}
        
        [BeforeClass]
        public static void StaticBeforeClassMethod() {}
        
        [AfterClass]
        public void NotStaticAfterClassMethod() {}

        [AfterClass]
        public static void StaticAfterClassMethod() {}
        
        [BeforeClass]
        [AfterClass]
        public static void StaticBeforeClassAndAfterClassMethod() {}
    }
}