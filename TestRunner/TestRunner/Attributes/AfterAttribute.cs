using System;

namespace TestRunner.Attributes
{
    /// <summary>
    /// Represents attribute used for methods that should be running after test method execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterAttribute : Attribute
    {
        
    }
}