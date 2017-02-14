using System;
using System.IO;

namespace AirplaneASP.Loggers
{
    public class ExceptionLogger : IExceptionLogger
    {
        private readonly string _filePath = @"C:\Users\mciesla\Documents\visual studio 2015\Projects\airplaneProj\myrep\log\ExceptionLogg.txt";
        public void LogException(Exception ex)
        {
            //time, type, message, stack trace - .txt file - config filename in config
            if (File.Exists(_filePath))
            {
                using (StreamWriter sw = File.CreateText(_filePath))
                {
                    sw.WriteLine(DateTime.Now + "\t" + ex.Message + "\t" + ex.StackTrace);
                }
            }

        }
    }
}
