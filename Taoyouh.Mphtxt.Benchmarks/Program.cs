// <copyright file="Program.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using BenchmarkDotNet.Running;

namespace Taoyouh.Mphtxt.Benchmarks
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ReaderBenchmark>();
        }
    }
}
