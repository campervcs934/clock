using System;
using System.Collections;

namespace Clock.API
{
    public interface ITimeApi
    {
        IEnumerator GetTime(Action<TimeResponse> action);
    }
}