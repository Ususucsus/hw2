using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestRunner.Attributes;
using TestRunner.RunResultModel;

namespace TestRunner
{
    /// <summary>
    /// Represents class that runs test fixture and collects run result.
    /// </summary>
    public sealed class Runner
    {
        /// <summary>
        /// Run result object.
        /// </summary>
        private readonly RunResult result = new RunResult();

        /// <summary>
        /// Lock object.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// Runs all test fixtures from assemblies located at specified path.
        /// </summary>
        /// <remarks>
        /// Assembly TestRunner.dll ignored.
        /// </remarks>
        /// <param name="path">Path to folder with assemblies which should be run.</param>
        /// <returns>Run result object.</returns>
        public RunResult RunAllTests(string path)
        {
            var assemblies = AssembliesParser.GetDllAssembliesFrom(path)
                .Where(assembly => assembly != Assembly.GetExecutingAssembly())
                .ToList();

            foreach (var assembly in assemblies)
            {
                RunAssembly(assembly);
            }

            return result;
        }

        /// <summary>
        /// Runs all test fixtures from specified assembly and collect result to run result object.
        /// </summary>
        /// <param name="assembly">Assembly from which test fixtures should be run.</param>
        private void RunAssembly(Assembly assembly)
        {
            var assemblyRunResult = new AssemblyRunResult(assembly.FullName ?? string.Empty);
            
            var testFixtures = AssembliesParser.GetTextFixtureClasses(assembly);

            var tasks = new List<Task>();
            foreach (var testFixture in testFixtures)
            {
                var task = Task.Run(() => RunTestFixture(testFixture, assemblyRunResult));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            if (assemblyRunResult.TotalTests > 0)
            {
                result.AddAssemblyRunResult(assemblyRunResult);
            }
        }

        /// <summary>
        /// Runs test methods and pipeline methods from specified type and collects result to assembly run result object.
        /// </summary>
        /// <param name="testFixture">Type of test fixture from which tests should be run.</param>
        /// <param name="assemblyRunResult">Assembly run result object.</param>
        private void RunTestFixture(Type testFixture, AssemblyRunResult assemblyRunResult)
        {
            var fixtureRunResult = new FixtureRunResult(testFixture.Name);

            var fixtureInstance = Activator.CreateInstance(testFixture) ?? throw new ArgumentException(nameof(testFixture));
            var testInfos = AssembliesParser.GetTestMethods(testFixture);

            InvokeBeforeClassMethods(testFixture);
            
            foreach (var testInfo in testInfos)
            {
                InvokeBeforeTestMethods(fixtureInstance);
                
                RunTestMethod(fixtureInstance, testInfo, fixtureRunResult);

                InvokeAfterTestMethods(fixtureInstance);
            }

            InvokeAfterClassMethods(testFixture);
            
            if (fixtureRunResult.TotalTests > 0)
            {
                lock (lockObject)
                {
                    assemblyRunResult.AddFixtureRunResult(fixtureRunResult);
                }
            }
        }

        /// <summary>
        /// Runs all before class methods from specified type.
        /// </summary>
        /// <param name="fixture">Type of test fixture from which before class methods should be run.</param>
        private void InvokeBeforeClassMethods(Type fixture)
        {
            var beforeClassMethods = AssembliesParser.GetBeforeClassMethods(fixture).ToList();
            
            foreach (var beforeClassMethod in beforeClassMethods)
            {
                beforeClassMethod.Invoke(null, new object[0]);
            }
        }

        /// <summary>
        /// Runs all after class methods from specified type.
        /// </summary>
        /// <param name="fixture">Type of test fixture from which after class methods should be run.</param>
        private void InvokeAfterClassMethods(Type fixture)
        {
            var afterClassMethods = AssembliesParser.GetAfterClassMethods(fixture).ToList();
            
            foreach (var afterClassMethod in afterClassMethods)
            {
                afterClassMethod.Invoke(null, new object[0]);
            }
        }

        /// <summary>
        /// Runs all before test methods from specified type.
        /// </summary>
        /// <param name="fixtureInstance">Instance of test fixture from which before test methods should be run.</param>
        private void InvokeBeforeTestMethods(object fixtureInstance)
        {
            var beforeTestMethods = AssembliesParser.GetBeforeTestMethods(fixtureInstance.GetType()).ToList();
            
            foreach (var beforeTestMethod in beforeTestMethods)
            {
                beforeTestMethod.Invoke(fixtureInstance, new object[0]);
            }
        }

        /// <summary>
        /// Runs all after test methods from specified type.
        /// </summary>
        /// <param name="fixtureInstance">Instance of test fixture from which after test methods should be run.</param>
        private void InvokeAfterTestMethods(object fixtureInstance)
        {
            var afterTestMethods = AssembliesParser.GetAfterTestMethods(fixtureInstance.GetType()).ToList();

            foreach (var afterTestMethod in afterTestMethods)
            {
                afterTestMethod.Invoke(fixtureInstance, new object[0]);
            }
        }
        
        /// <summary>
        /// Runs test methods and collects run information to fixture run result object.
        /// </summary>
        /// <param name="fixtureInstance">Instance of test fixture from which test methods should be run.</param>
        /// <param name="methodInfo">Test method information.</param>
        /// <param name="fixtureRunResult">Fixture run result object.</param>
        private void RunTestMethod(object fixtureInstance, MethodInfo methodInfo, FixtureRunResult fixtureRunResult)
        {
            Stopwatch stopwatch = new Stopwatch();
            var status = TestRunStatus.Passed;
            Exception? exception = null;
            var testAttribute = (TestAttribute) methodInfo.GetCustomAttribute(typeof(TestAttribute))!;
            var expectedExceptionType = testAttribute.Expected;
            var ignore = testAttribute.Ignore;

            if (ignore)
            {
                fixtureRunResult.AddTestRunResult(new TestRunResult(methodInfo.Name, TimeSpan.Zero,
                    TestRunStatus.Ignored, testAttribute.IgnoreReason ?? string.Empty));
                return;
            }
            
            try
            {
                stopwatch = Stopwatch.StartNew();
                methodInfo.Invoke(fixtureInstance, new object[0]);
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException ?? throw ex;
                status = innerException.GetType() == expectedExceptionType ? TestRunStatus.Failed : TestRunStatus.Crashed;
                exception = ex.InnerException;
            }
            finally
            {
                stopwatch.Stop();
            }

            var testRunResult = new TestRunResult(methodInfo.Name, stopwatch.Elapsed, status, exception?.ToString() ?? string.Empty);
            
            fixtureRunResult.AddTestRunResult(testRunResult);
        }
    }
}