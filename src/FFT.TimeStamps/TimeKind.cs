using System;
using System.Collections.Generic;
using System.Text;

namespace FFT.TimeStamps
{
  /// <summary>
  /// </summary>
  public enum TimeKind
  {
    /// <summary>
    /// The time is being expressed in the utc timezone.
    /// </summary>
    Utc,

    /// <summary>
    /// The time is being expressed in a specific timezone, known by the context.
    /// </summary>
    TimeZone,
  }
}
