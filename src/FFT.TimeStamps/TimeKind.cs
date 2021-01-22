// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  /// <summary>
  /// Use this to indicate whether time is being expressed in the utc timezone or some other specific (known) timezone.
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
