using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace AirplaneASP.Loggers
{
    public class RequestLogger : IRequestLogger
    {
        private static string _filePath = ConfigurationManager.AppSettings["requestLoggerFilePath"].ToString();

        public void LogRequest(HttpRequest request)
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("[" + DateTime.Now + "]" + "\t" + request.UrlReferrer + "\t" + request.UserAgent);
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
