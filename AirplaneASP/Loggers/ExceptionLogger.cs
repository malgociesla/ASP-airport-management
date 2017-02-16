using System;
using System.IO;
using System.Configuration;

namespace AirplaneASP.Loggers
{
    public class ExceptionLogger : IExceptionLogger
    {
        private static string _filePath = ConfigurationManager.AppSettings["exceptionLoggerFilePath"].ToString();

        public void LogException(Exception ex)
        {
            //time, type, message, stack trace - .txt file - config filename in config
            using (FileStream fs = File.Open(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("[" + DateTime.Now + "]" + "\t" + ex.Message);
                    sw.WriteLine(ex.StackTrace);
                    sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }
    }
}
