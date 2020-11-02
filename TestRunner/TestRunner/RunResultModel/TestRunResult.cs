using System;
using System.Linq;

namespace TestRunner.RunResultModel
{
    public sealed class TestRunResult
    {
        public TestRunResult(string methodName, TimeSpan runningTime, TestRunStatus status, string message)
        {
            MethodName = methodName;
            RunningTime = runningTime;
            Message = message;
            Status = status;
        }

        public string MethodName { get; }

        public TimeSpan RunningTime { get; }
        
        public TestRunStatus Status { get; }

        public string Message { get; }

        public override string ToString()
        {
            return $"      [Test] {MethodName} ({RunningTime.TotalMilliseconds} ms) {Status}.\n{string.Join(Environment.NewLine, Message.Split(Environment.NewLine).Select(line => "        " + line))}";
        }
    }
}