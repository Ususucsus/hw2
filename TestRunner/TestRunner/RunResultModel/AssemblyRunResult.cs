using System;
using System.Collections.Generic;
using System.Linq;

namespace TestRunner.RunResultModel
{
    public sealed class AssemblyRunResult
    {
        public AssemblyRunResult(string name)
        {
            Name = name;
        }

        public void AddFixtureRunResult(FixtureRunResult fixtureRunResult)
        {
            FixtureRunResults.Add(fixtureRunResult);
        }

        public string Name { get; }
        
        public List<FixtureRunResult> FixtureRunResults { get; } = new List<FixtureRunResult>();
        
        public TimeSpan RunningTime => TimeSpan.FromTicks(FixtureRunResults.Sum(fixtureRunResult => fixtureRunResult.RunningTime.Ticks));

        public int TotalTests => FixtureRunResults.Sum(fixtureRunResult => fixtureRunResult.TotalTests);
        
        public override string ToString()
        {
            return $"  [Assembly] {Name} [{RunningTime.TotalMilliseconds} ms] ({TotalTests} tests):\n{string.Join("\n", FixtureRunResults)}";
        }
    }
}