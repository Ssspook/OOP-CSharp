using System;
using System.IO;

namespace BackupsExtra.Loggers
{
    public class FileLogger : ILogger
    {
        private bool _isTimeCodeNeeded;

        public FileLogger(bool isTimeCodeNeeded)
        {
            _isTimeCodeNeeded = isTimeCodeNeeded;
        }
        public void Log(string loggingLine)
        {
            using var writer = new StreamWriter("Log");
            if (_isTimeCodeNeeded)
                writer.WriteLine(DateTime.Now + ": "+ loggingLine);
            else
                writer.WriteLine(loggingLine);
        }
    }
}