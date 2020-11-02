using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TestRunner;
using TestRunnerTests.ClassesForTests;

namespace TestRunnerTests
{
    public class AssembliesParserTests
    {
        private string dllsForTestsDirectory;

        [SetUp]
        public void Setup()
        {
            var testProjectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            dllsForTestsDirectory = Path.Join(testProjectDirectory, "DllsForTests");
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void GetDllAssembliesFromShouldReturnAssemblies()
        {
            var expectedAssembliesFullNames = new[]
            {
                "TestProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "TestRunner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            };

            var assemblies = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory).ToList();

            Assert.AreEqual(2, assemblies.Count);

            Assert.That(assemblies[0].FullName == expectedAssembliesFullNames[0] &&
                        assemblies[1].FullName == expectedAssembliesFullNames[1] ||
                        assemblies[0].FullName == expectedAssembliesFullNames[1] &&
                        assemblies[1].FullName == expectedAssembliesFullNames[0]);
        }

        [Test]
        [TestCase("ClassTest", true)]
        [TestCase("ClassTests", true)]
        [TestCase("TestClass", true)]
        [TestCase("TestsClass", true)]
        [TestCase("NotTestClass", false)]
        public void GetTestFixtureClassesShouldReturnClassesWithNameThatStartsOrEndsWithTestOrTests(
            string expectedClassName, bool result)
        {
            var assembly = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory)
                .First(a => a != Assembly.GetExecutingAssembly());

            var testFixtureClasses = AssembliesParser.GetTextFixtureClasses(assembly);
            var testFixtureClassesNames = testFixtureClasses.Select(fixture => fixture.Name);

            Assert.That(testFixtureClassesNames.Contains(expectedClassName) == result);
        }

        [Test]
        public void GetTestFixtureClassesShouldReturnClassesMarkedWithTestFixtureAttribute()
        {
            var assembly = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory)
                .First(a => a != Assembly.GetExecutingAssembly());

            var testFixtureClasses = AssembliesParser.GetTextFixtureClasses(assembly);
            var testFixtureClassesNames = testFixtureClasses.Select(fixture => fixture.Name);

            Assert.IsTrue(testFixtureClassesNames.Contains("ClassTestFixture"));
        }

        [Test]
        [TestCase("TestMethod1", true)]
        [TestCase("TestMethod2", true)]
        [TestCase("NotTestMethod", false)]
        public void GetTestMethodsShouldReturnMethodsOnlyMarkedWithTestAttribute(string expectedMethodName, bool result)
        {
            var methodInfos = AssembliesParser.GetTestMethods(typeof(ClassWithTestFunctions));
            var methodNames = methodInfos.Select(methodInfo => methodInfo.Name);

            Assert.That(methodNames.Contains(expectedMethodName) == result);
        }

        [Test]
        [TestCase("BeforeMethod1", true)]
        [TestCase("BeforeMethod2", true)]
        [TestCase("BeforeAndAfterMethod", true)]
        [TestCase("CommonMethod", false)]
        [TestCase("TestMethod1", false)]
        [TestCase("AfterMethod1", false)]
        public void GetBeforeTestMethodsShouldReturnMethodOnlyMarkedWithBeforeAttribute(string expectedMethodName,
            bool result)
        {
            var methodInfos = AssembliesParser.GetBeforeTestMethods(typeof(ClassWithTestFunctions));
            var methodNames = methodInfos.Select(methodInfo => methodInfo.Name);

            Assert.That(methodNames.Contains(expectedMethodName) == result);
        }

        [Test]
        [TestCase("AfterMethod1", true)]
        [TestCase("AfterMethod2", true)]
        [TestCase("BeforeAndAfterMethod", true)]
        [TestCase("CommonMethod", false)]
        [TestCase("TestMethod1", false)]
        [TestCase("BeforeMethod1", false)]
        public void GetAfterTestMethodsShouldReturnMethodOnlyMarkedWithAfterAttribute(string expectedMethodName,
            bool result)
        {
            var methodInfos = AssembliesParser.GetAfterTestMethods(typeof(ClassWithTestFunctions));
            var methodNames = methodInfos.Select(methodInfo => methodInfo.Name);

            Assert.That(methodNames.Contains(expectedMethodName) == result);
        }

        [Test]
        [TestCase("NotStaticBeforeClassMethod", false)]
        [TestCase("StaticBeforeClassMethod", true)]
        [TestCase("StaticBeforeClassAndAfterClassMethod", true)]
        [TestCase("CommonMethod", false)]
        public void GetBeforeClassMethodsShouldReturnOnlyStaticAndMarkedWithBeforeClassAttributeMethods(
            string expectedMethodName, bool result)
        {
            var methodInfos = AssembliesParser.GetBeforeClassMethods(typeof(ClassWithTestFunctions));
            var methodNames = methodInfos.Select(methodInfo => methodInfo.Name);

            Assert.That(methodNames.Contains(expectedMethodName) == result);
        }

        [Test]
        [TestCase("NotStaticAfterClassMethod", false)]
        [TestCase("StaticAfterClassMethod", true)]
        [TestCase("StaticBeforeClassAndAfterClassMethod", true)]
        [TestCase("CommonMethod", false)]
        public void GetAfterClassMethodsShouldReturnOnlyStaticAndMarkedWithAfterClassAttributeMethods(
            string expectedMethodName, bool result)
        {
            var methodInfos = AssembliesParser.GetAfterClassMethods(typeof(ClassWithTestFunctions));
            var methodNames = methodInfos.Select(methodInfo => methodInfo.Name);

            Assert.That(methodNames.Contains(expectedMethodName) == result);
        }
    }
}