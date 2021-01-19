using System;
using System.Linq;

namespace FFT.TimeStamps.Examples
{
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
