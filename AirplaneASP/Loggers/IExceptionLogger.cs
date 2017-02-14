using System;

namespace AirplaneASP.Loggers
{
    public interface IExceptionLogger
    {
        void LogException(Exception ex);
    }
}
