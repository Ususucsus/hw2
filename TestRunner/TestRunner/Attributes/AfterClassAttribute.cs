using System;

namespace TestRunner.Attributes
{
    /// <summary>
    /// Represents attribute used for methods that should be running after all test methods execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterClassAttribute : Attribute
    {
        
    }
}