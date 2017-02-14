using System.Web;

namespace AirplaneASP.Loggers
{
    public interface IRequestLogger
    {
        void LogRequest(HttpRequest request);
    }
}
