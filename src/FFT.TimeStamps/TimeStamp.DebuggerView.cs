namespace FFT.TimeStamps
{
  public readonly partial struct TimeStamp
  {
    private readonly struct DebuggerView
    {

      public readonly long TicksUTC;
      public readonly string Local;
      public readonly string Utc;
      public DebuggerView(TimeStamp value)
      {
        TicksUTC = value.TicksUtc;
        Local = value.AsLocal().ToString(DEFAULT_FORMAT_STRING);
        Utc = value.AsUtc().ToString(DEFAULT_FORMAT_STRING);
      }
    }
  }
}
