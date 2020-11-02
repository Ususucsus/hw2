using System;

namespace TestRunner.Attributes
{
    /// <summary>
    /// Represents attributes used for classes that contains test methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestFixtureAttribute : Attribute
    {
        
    }
}