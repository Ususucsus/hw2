using System;
using NUnit.Framework;
using TestRunner.RunResultModel;

namespace TestRunnerTests
{
    public class RunResultTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [TearDown]
        public void Teardown()
        {
            
        }

        [Test]
        public void TestRunResult()
        {
            var testResult = new TestRunResult("methodName", TimeSpan.FromSeconds(3), TestRunStatus.Ignored, "message");
            
            Assert.AreEqual("methodName", testResult.MethodName);
            Assert.AreEqual(TimeSpan.FromSeconds(3), testResult.RunningTime);
            Assert.AreEqual(TestRunStatus.Ignored, testResult.Status);
            Assert.AreEqual("message", testResult.Message);
            Assert.AreNotEqual(
                new TestRunResult("methodName", TimeSpan.Zero, TestRunStatus.Crashed, "message").ToString(),
                testResult.ToString());
        }

        [Test]
        public void FixtureRunResult()
        {
            var testResult1 = new TestRunResult("methodName1", TimeSpan.FromSeconds(3), TestRunStatus.Ignored, "message1");
            var testResult2 = new TestRunResult("methodName2", TimeSpan.FromSeconds(2), TestRunStatus.Crashed, "message2");
            
            var fixtureResult = new FixtureRunResult("fixtureName");
            fixtureResult.AddTestRunResult(testResult1);
            fixtureResult.AddTestRunResult(testResult2);
            
            Assert.AreEqual("fixtureName", fixtureResult.Name);
            Assert.AreEqual(2, fixtureResult.TestRunResults.Count);
            Assert.AreEqual(2, fixtureResult.TotalTests);
            Assert.AreEqual(TimeSpan.FromSeconds(5), fixtureResult.RunningTime);
            Assert.AreNotEqual(new FixtureRunResult("fixtureName").ToString(), fixtureResult.ToString());
        }

        [Test]
        public void AssemblyRunResult()
        {
            var testResult1 = new TestRunResult("methodName1", TimeSpan.FromSeconds(4), TestRunStatus.Ignored, "message1");
            var testResult2 = new TestRunResult("methodName2", TimeSpan.FromSeconds(2), TestRunStatus.Crashed, "message2");
            var testResult3 = new TestRunResult("methodName3", TimeSpan.FromSeconds(1), TestRunStatus.Passed, "message3");
            
            var fixtureResult1 = new FixtureRunResult("fixtureName1");
            fixtureResult1.AddTestRunResult(testResult1);
            fixtureResult1.AddTestRunResult(testResult2);
            
            var fixtureResult2 = new FixtureRunResult("fixtureName2");
            fixtureResult2.AddTestRunResult(testResult3);
            
            var assemblyResult = new AssemblyRunResult("assemblyName");
            assemblyResult.AddFixtureRunResult(fixtureResult1);
            assemblyResult.AddFixtureRunResult(fixtureResult2);
            
            Assert.AreEqual("assemblyName", assemblyResult.Name);
            Assert.AreEqual(2, assemblyResult.FixtureRunResults.Count);
            Assert.AreEqual(3, assemblyResult.TotalTests);
            Assert.AreEqual(TimeSpan.FromSeconds(7), assemblyResult.RunningTime);
            Assert.AreNotEqual(new AssemblyRunResult("assemblyName").ToString(), assemblyResult.ToString());
        }

        [Test]
        public void RunResult()
        {
            var testResult1 = new TestRunResult("methodName1", TimeSpan.FromSeconds(4), TestRunStatus.Ignored, "message1");
            var testResult2 = new TestRunResult("methodName2", TimeSpan.FromSeconds(2), TestRunStatus.Crashed, "message2");
            var testResult3 = new TestRunResult("methodName3", TimeSpan.FromSeconds(1), TestRunStatus.Passed, "message3");
            var testResult4 = new TestRunResult("methodName4", TimeSpan.FromSeconds(8), TestRunStatus.Failed, "message4");

            var fixtureResult1 = new FixtureRunResult("fixtureName1");
            fixtureResult1.AddTestRunResult(testResult1);
            fixtureResult1.AddTestRunResult(testResult2);
            
            var fixtureResult2 = new FixtureRunResult("fixtureName2");
            fixtureResult2.AddTestRunResult(testResult3);

            var fixtureResult3 = new FixtureRunResult("fixtureName3");
            fixtureResult3.AddTestRunResult(testResult4);
            
            var assemblyResult1 = new AssemblyRunResult("assemblyName1");
            assemblyResult1.AddFixtureRunResult(fixtureResult1);
            assemblyResult1.AddFixtureRunResult(fixtureResult2);
            
            var assemblyResult2 = new AssemblyRunResult("assemblyName2");
            assemblyResult2.AddFixtureRunResult(fixtureResult3);
            
            var runResult = new RunResult();
            runResult.AddAssemblyRunResult(assemblyResult1);
            runResult.AddAssemblyRunResult(assemblyResult2);
            
            Assert.AreEqual(2, runResult.AssemblyRunResults.Count);
            Assert.AreEqual(4, runResult.TotalTests);
            Assert.AreEqual(TimeSpan.FromSeconds(15), runResult.RunningTime);
            Assert.AreNotEqual(new RunResult().ToString(), runResult.ToString());
        }
    }
}