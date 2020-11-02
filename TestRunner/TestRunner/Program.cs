using System;
using System.IO;
using TestRunner.RunResultModel;

namespace TestRunner
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Test Runner");
            Console.WriteLine("Enter path to directory with assemblies: ");
            var path = Console.ReadLine();
            Console.WriteLine($"Path: {path}");

            var runner = new Runner();
            RunResult result;

            try
            {
                result = runner.RunAllTests(path ?? throw new ArgumentNullException());
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Incorrect path");
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found");
                return;
            }
            catch (IOException)
            {
                Console.WriteLine("Path if a file name");
                return;
            } 

            Console.WriteLine("Runner result: ");
            Console.WriteLine(result);
        }
    }
}