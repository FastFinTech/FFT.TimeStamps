// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Benchmarks
{
  using System;
  using System.Diagnostics;
  using System.Reflection;
  using BenchmarkDotNet.Running;

  internal class Program
  {
    private static void Main(string[] args)
    {
      BenchmarkRunner.Run(Assembly.GetExecutingAssembly());
    }
  }
}
