// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Examples
{
  using System;
  using System.Linq;

  internal class Program
  {
    private static void Main(string[] args)
    {
      foreach (var exampleType in typeof(Program).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IExample))))
      {
        var example = (IExample)Activator.CreateInstance(exampleType)!;
        example.Run();
      }
    }
  }
}
