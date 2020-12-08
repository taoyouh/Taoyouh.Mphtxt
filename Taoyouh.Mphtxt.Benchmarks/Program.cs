using System;
using BenchmarkDotNet.Running;

namespace Taoyouh.Mphtxt.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ReaderBenchmark>();
        }
    }
}
