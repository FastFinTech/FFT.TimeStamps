using System;
using System.Threading;

namespace FFT.TimeStamps;

internal static class ImmutableInterlocked
{
  public static bool Update<T>(ref T location, Func<T, T> transformer)
    where T : class
  {
    bool successful;
    T oldValue = Volatile.Read(ref location);
    do
    {
      T newValue = transformer(oldValue);
      if (ReferenceEquals(oldValue, newValue))
      {
        // No change was actually required.
        return false;
      }

      T interlockedResult = Interlocked.CompareExchange(ref location, newValue, oldValue);
      successful = ReferenceEquals(oldValue, interlockedResult);
      oldValue = interlockedResult; // we already have a volatile read that we can reuse for the next loop
    }
    while (!successful);

    return true;
  }
}
