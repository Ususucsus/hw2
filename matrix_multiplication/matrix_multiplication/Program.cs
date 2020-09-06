using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = string.Empty;
            while (command != "exit")
            {
                Console.WriteLine("enter command (gen1, gen2, mul, exit): ");
                command = Console.ReadLine()?.ToLower();

                switch (command)
                {
                    case "gen1":
                    {
                        Console.WriteLine("enter height: ");
                        if (!int.TryParse(Console.ReadLine(), out var height))
                        {
                            Console.WriteLine("bad height");
                            continue;
                        }

                        Console.WriteLine("enter width: ");
                        if (!int.TryParse(Console.ReadLine(), out var width))
                        {
                            Console.WriteLine("bad width");
                            continue;
                        }

                        var matrix = MatrixHelper.GetRandomMatrix(height, width);
                        MatrixHelper.SaveMatrix(matrix, "input1.txt");
                        break;
                    }
                    case "gen2":
                    {
                        Console.WriteLine("enter height: ");
                        if (!int.TryParse(Console.ReadLine(), out var height))
                        {
                            Console.WriteLine("bad height");
                            continue;
                        }

                        Console.WriteLine("enter width: ");
                        if (!int.TryParse(Console.ReadLine(), out var width))
                        {
                            Console.WriteLine("bad width");
                            continue;
                        }

                        var matrix = MatrixHelper.GetRandomMatrix(height, width);
                        MatrixHelper.SaveMatrix(matrix, "input2.txt");
                        break;
                    }
                    case "mul":
                        var solvers = new List<ISolver>()
                        {
                            new SyncSolver(),
                            new PoolSolver(),
                        };

                        foreach (var solver in solvers)
                        {
                            var stopwatch = new Stopwatch();
                            stopwatch.Start();
                            var result = solver.Multiply(MatrixHelper.LoadMatrix("input1.txt"),
                                MatrixHelper.LoadMatrix("input2.txt"));
                            stopwatch.Stop();

                            MatrixHelper.SaveMatrix(result, $"result-{solver}.txt");
                            Console.WriteLine($"{solver} time: {stopwatch.Elapsed.TotalMilliseconds}");
                        }
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

    }
}
