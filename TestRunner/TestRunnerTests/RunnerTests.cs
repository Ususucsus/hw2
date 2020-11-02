using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TestRunner;
using TestRunner.RunResultModel;

namespace TestRunnerTests
{
    public class RunnerTests
    {
        private Runner runner;
        private string dllsForTestsDirectory;
        private const int ExpectedTotalTestNumber = 14;
        private const int ExpectedRunFixtureNumber = 6;
        
        [SetUp]
        public void Setup()
        {
            var testProjectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            dllsForTestsDirectory = Path.Join(testProjectDirectory, "DllsForTests");
            
            runner = new Runner();
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void RunAllTestsResultShouldContainResultForAssembly()
        {
            for (var i = 0; i < 1000; i++)
            {
                var lRunner = new Runner();
                var result = lRunner.RunAllTests(dllsForTestsDirectory);

                Assert.AreEqual(1, result.AssemblyRunResults.Count);
                Assert.AreEqual(ExpectedTotalTestNumber, result.TotalTests);
                Assert.AreNotEqual(TimeSpan.Zero, result.RunningTime);
            }
        }

        [Test]
        public void AssemblyResultShouldContainTestFixtureResults()
        {
            var result = runner.RunAllTests(dllsForTestsDirectory);
            var assemblyResult = result.AssemblyRunResults[0];
            
            Assert.AreEqual(ExpectedRunFixtureNumber, assemblyResult.FixtureRunResults.Count);
            Assert.AreEqual(ExpectedTotalTestNumber, assemblyResult.TotalTests);
            Assert.AreNotEqual(TimeSpan.Zero, assemblyResult.RunningTime);
            Assert.IsNotEmpty(assemblyResult.Name);
        }

        [Test]
        public void FixtureResultShouldContainTestMethodsResults()
        {
            var result = runner.RunAllTests(dllsForTestsDirectory);
            var fixtureResult = result.AssemblyRunResults[0].FixtureRunResults
                .First(fixtureRunResult => fixtureRunResult.Name == "ClassTest");
            
            Assert.AreEqual(3, fixtureResult.TotalTests);
            Assert.AreNotEqual(TimeSpan.Zero, fixtureResult.RunningTime);
            Assert.IsNotEmpty(fixtureResult.Name);
        }

        [Test]
        public void TestResultShouldBePassed()
        {
            var result = runner.RunAllTests(dllsForTestsDirectory);
            var testsResult = result.AssemblyRunResults[0].FixtureRunResults
                .SelectMany(fixtureResult => fixtureResult.TestRunResults);

            var shouldBePassedTests = testsResult.Where(testResult =>
                new[] {"PassedMethod1", "PassedMethod2"}.Contains(testResult.MethodName)).ToList();

            foreach (var testResult in shouldBePassedTests)
            {
                Assert.IsNotEmpty(testResult.MethodName);
                Assert.AreEqual(TestRunStatus.Passed, testResult.Status);
                Assert.AreNotEqual(TimeSpan.Zero, testResult.RunningTime);
            }
        }

        [Test]
        public void TestResultShouldBeFailed()
        {
            var result = runner.RunAllTests(dllsForTestsDirectory);
            var testsResult = result.AssemblyRunResults[0].FixtureRunResults
                .SelectMany(fixtureResult => fixtureResult.TestRunResults);

            var shouldBeFailedTest = testsResult.First(testResult => testResult.MethodName == "FailedTestMethod");
            
            Assert.IsNotEmpty(shouldBeFailedTest.MethodName);
            Assert.AreEqual(TestRunStatus.Failed, shouldBeFailedTest.Status);
            Assert.IsNotEmpty(shouldBeFailedTest.Message);
            Assert.AreNotEqual(TimeSpan.Zero, shouldBeFailedTest.RunningTime);
        }
        
        [Test]
        public void TestResultShouldBeCrashed()
        {
            var result = runner.RunAllTests(dllsForTestsDirectory);
            var testsResult = result.AssemblyRunResults[0].FixtureRunResults
                .SelectMany(fixtureResult => fixtureResult.TestRunResults);

            var shouldBeCrashedTest = testsResult.First(testResult => testResult.MethodName == "CrashedTestMethod");
            
            Assert.IsNotEmpty(shouldBeCrashedTest.MethodName);
            Assert.AreEqual(TestRunStatus.Crashed, shouldBeCrashedTest.Status);
            Assert.IsNotEmpty(shouldBeCrashedTest.Message);
            Assert.AreNotEqual(TimeSpan.Zero, shouldBeCrashedTest.RunningTime);
        }
        
        [Test]
        public void TestResultShouldBeIgnored()
        {
            var result = runner.RunAllTests(dllsForTestsDirectory);
            var testsResult = result.AssemblyRunResults[0].FixtureRunResults
                .SelectMany(fixtureResult => fixtureResult.TestRunResults);

            var shouldBeIgnoredTests = testsResult.Where(testResult =>
                new[] {"IgnoredTestMethod1", "IgnoredTestMethod2"}.Contains(testResult.MethodName)).ToList();

            foreach (var testResult in shouldBeIgnoredTests)
            {
                Assert.IsNotEmpty(testResult.MethodName);
                Assert.AreEqual(TestRunStatus.Ignored, testResult.Status);
                Assert.AreEqual(TimeSpan.Zero, testResult.RunningTime);
                Assert.AreEqual("ignore reason", testResult.Message);
            }
        }

        [Test]
        public void BeforeShouldExecutesBeforeTestRun()
        {
            var beforeTestFixture = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory)
                .SelectMany(AssembliesParser.GetTextFixtureClasses)
                .First(fixture => fixture.Name == "BeforeTest");

            var shouldBeFive = beforeTestFixture.GetField("ShouldBeFive");
            
            Assert.NotNull(beforeTestFixture);
            Assert.NotNull(shouldBeFive);
            
            shouldBeFive.SetValue(null, 1);

            runner.RunAllTests(dllsForTestsDirectory);

            Assert.AreEqual(5, shouldBeFive.GetValue(null));
        }
        
        [Test]
        public void AfterShouldExecutesBeforeTestRun()
        {
            var afterTestFixture = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory)
                .SelectMany(AssembliesParser.GetTextFixtureClasses)
                .First(fixture => fixture.Name == "BeforeTest");

            var shouldBeFive = afterTestFixture.GetField("ShouldBeFive");
            
            Assert.NotNull(afterTestFixture);
            Assert.NotNull(shouldBeFive);
            
            shouldBeFive.SetValue(null, 1);

            runner.RunAllTests(dllsForTestsDirectory);

            Assert.AreEqual(5, shouldBeFive.GetValue(null));
        }

        [Test]
        public void BeforeClassMethodShouldExecutesOnlyOnce()
        {
            var beforeClassFixture = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory)
                .SelectMany(AssembliesParser.GetTextFixtureClasses)
                .First(fixture => fixture.Name == "BeforeClassTest");

            var shouldBeFive = beforeClassFixture.GetField("ShouldBeFive");
            
            Assert.NotNull(beforeClassFixture);
            Assert.NotNull(shouldBeFive);
            
            shouldBeFive.SetValue(null, 1);

            runner.RunAllTests(dllsForTestsDirectory);

            Assert.AreEqual(5, shouldBeFive.GetValue(null));
        }
        
        [Test]
        public void AfterClassMethodShouldExecutesOnlyOnce()
        {
            var afterClassFixture = AssembliesParser.GetDllAssembliesFrom(dllsForTestsDirectory)
                .SelectMany(AssembliesParser.GetTextFixtureClasses)
                .First(fixture => fixture.Name == "AfterClassTest");

            var shouldBeFive = afterClassFixture.GetField("ShouldBeFive");
            
            Assert.NotNull(afterClassFixture);
            Assert.NotNull(shouldBeFive);
            
            shouldBeFive.SetValue(null, 1);

            runner.RunAllTests(dllsForTestsDirectory);

            Assert.AreEqual(5, shouldBeFive.GetValue(null));
        }
    }
}