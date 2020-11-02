using System;

namespace TestRunner.Attributes
{
    /// <summary>
    /// Represents attribute used for methods that should be running before test method execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute
    {
        
    }
}