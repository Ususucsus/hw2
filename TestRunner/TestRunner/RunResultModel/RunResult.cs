using System;
using System.Collections.Generic;
using System.Linq;

namespace TestRunner.RunResultModel
{
    public sealed class RunResult
    {
        public void AddAssemblyRunResult(AssemblyRunResult assemblyRunResult)
        {
            AssemblyRunResults.Add(assemblyRunResult);
        }

        public List<AssemblyRunResult> AssemblyRunResults { get; } = new List<AssemblyRunResult>();
        
        public TimeSpan RunningTime => TimeSpan.FromTicks(AssemblyRunResults.Sum(assemblyRunResult => assemblyRunResult.RunningTime.Ticks));
        
        public int TotalTests => AssemblyRunResults.Sum(assemblyRunResult => assemblyRunResult.TotalTests);

        public override string ToString()
        {
            return $"[Run] [{RunningTime.TotalMilliseconds} ms] ({TotalTests} tests):\n{string.Join("\n", AssemblyRunResults)}";
        }
    }
}