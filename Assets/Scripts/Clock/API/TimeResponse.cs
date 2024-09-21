using System;

namespace Clock.API
{
    [Serializable]
    public class TimeResponse
    {
        public long time;
        
        public TimeSpan LocalTime => DateTimeOffset.FromUnixTimeMilliseconds(time).LocalDateTime.TimeOfDay;
    }
}