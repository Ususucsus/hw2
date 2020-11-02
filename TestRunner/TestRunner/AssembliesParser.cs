using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TestRunner.Attributes;

namespace TestRunner
{
    public static class AssembliesParser
    {
        /// <summary>
        /// Loads and returns all *.dll assemblies from specified folder.
        /// </summary>
        /// <param name="path">Path to folder with dlls.</param>
        /// <returns>Asseblies.</returns>
        public static IEnumerable<Assembly> GetDllAssembliesFrom(string path)
        {
            var dlls = Directory.EnumerateFiles(path, "*.dll");

            var assemblies = dlls.Select(Assembly.LoadFrom);

            return assemblies;
        }

        /// <summary>
        /// Selects test fixture classes from specified assembly.
        /// </summary>
        /// <remarks>
        /// Test fixture is
        /// class with name that starts with "test" or "tests"
        /// or
        /// class with name that ends with "test" or "tests"
        /// or
        /// class marked with <see cref="TestFixtureAttribute"/> attribute.
        /// </remarks>
        /// <param name="assembly">Assembly from which extract classes.</param>
        /// <returns>Types of test fixture classes.</returns>
        public static IEnumerable<Type> GetTextFixtureClasses(Assembly assembly)
        {
            var types = assembly.ExportedTypes;

            foreach (var type in types)
            {
                var name = type.Name.ToLowerInvariant();
                var isTestFixtureAttribute = type.GetCustomAttribute(typeof(TestFixtureAttribute)) != null;

                if (isTestFixtureAttribute ||
                    name.StartsWith("test") || name.EndsWith("test") || name.EndsWith("tests"))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Selects test methods from specified type.
        /// </summary>
        /// <remarks>
        /// Test method is method marked with <see cref="TestAttribute"/> attribute.
        /// </remarks>
        /// <param name="type">Type from which select methods.</param>
        /// <returns>Test methods.</returns>
        public static IEnumerable<MethodInfo> GetTestMethods(Type type) 
            => GetMethodWithCustomAttribute(type, typeof(TestAttribute));

        /// <summary>
        /// Selects before test methods from specified type.
        /// </summary>
        /// <remarks>
        /// Before test method is method marked with <see cref="BeforeAttribute"/> attribute.
        /// </remarks>
        /// <param name="type">Type from which select methods.</param>
        /// <returns>Before test methods.</returns>
        public static IEnumerable<MethodInfo> GetBeforeTestMethods(Type type)
            => GetMethodWithCustomAttribute(type, typeof(BeforeAttribute));

        /// <summary>
        /// Selects after test methods from specified type.
        /// </summary>
        /// <remarks>
        /// After test method is method marked with <see cref="AfterAttribute"/> attribute.
        /// </remarks>
        /// <param name="type">Type from which select methods.</param>
        /// <returns>After test methods.</returns>
        public static IEnumerable<MethodInfo> GetAfterTestMethods(Type type)
            => GetMethodWithCustomAttribute(type, typeof(AfterAttribute));

        /// <summary>
        /// Selects before class methods from specified type.
        /// </summary>
        /// <remarks>
        /// Before class methods is static methods marked with <see cref="BeforeClassAttribute"/> attribute.
        /// </remarks>
        /// <param name="type">Type from which select methods.</param>
        /// <returns>Before class methods.</returns>
        public static IEnumerable<MethodInfo> GetBeforeClassMethods(Type type)
        {
            var staticWithAttributeMethods = GetMethodWithCustomAttribute(type, typeof(BeforeClassAttribute))
                .Where(methodInfo => methodInfo.IsStatic);

            return staticWithAttributeMethods;
        }

        /// <summary>
        /// Selects after class methods from specified type.
        /// </summary>
        /// <remarks>
        /// After class methods is static methods marked with <see cref="AfterClassAttribute"/> attribute.
        /// </remarks>
        /// <param name="type">Type from which select methods.</param>
        /// <returns>After class methods.</returns>
        public static IEnumerable<MethodInfo> GetAfterClassMethods(Type type)
        {
            var staticWithAttributeMethods = GetMethodWithCustomAttribute(type, typeof(AfterClassAttribute))
                .Where(methodInfo => methodInfo.IsStatic);

            return staticWithAttributeMethods;
        }

        /// <summary>
        /// Select methods from specified type with specified custom attribute.
        /// </summary>
        /// <param name="type">Type from which select methods.</param>
        /// <param name="attributeType">Type of method attribute.</param>
        /// <returns>Methods with specified attribute.</returns>
        private static IEnumerable<MethodInfo> GetMethodWithCustomAttribute(Type type, Type attributeType)
        {
            var methodInfos = type.GetMethods();

            foreach (var methodInfo in methodInfos)
            {
                var isTestAttribute = methodInfo.GetCustomAttribute(attributeType) != null;
                
                if (isTestAttribute)
                {
                    yield return methodInfo;
                }
            }
        }
    }
}