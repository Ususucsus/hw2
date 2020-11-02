using System;

namespace TestRunner.Attributes
{
    /// <summary>
    /// Represents attribute used for methods that should be running before all test methods execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeClassAttribute : Attribute
    {
        
    }
}