using System;
using System.Collections.Generic;
using System.Linq;

namespace TestRunner.RunResultModel
{
    public sealed class FixtureRunResult
    {
        public FixtureRunResult(string name)
        {
            Name = name;
        }

        public void AddTestRunResult(TestRunResult testRunResult)
        {
            TestRunResults.Add(testRunResult);
        }

        public string Name { get; }
        
        public List<TestRunResult> TestRunResults { get; } = new List<TestRunResult>();
        
        public TimeSpan RunningTime => TimeSpan.FromTicks(TestRunResults.Sum(testRunResult => testRunResult.RunningTime.Ticks));

        public int TotalTests => TestRunResults.Count;

        public override string ToString()
        {
            return $"    [Fixture] {Name} [{RunningTime.TotalMilliseconds} ms] ({TotalTests} tests):\n{string.Join("\n", TestRunResults)}";
        }
    }
}