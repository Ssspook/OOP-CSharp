using System;

namespace BackupsExtra.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private readonly bool _isTimeCodeNeeded;
        public ConsoleLogger(bool isTimeCodeNeeded)
        {
            _isTimeCodeNeeded = isTimeCodeNeeded;
        }

        public void Log(string loggingLine)
        {
            if (_isTimeCodeNeeded)
                Console.WriteLine(DateTime.Now + ": " + loggingLine);
            else
                Console.WriteLine(loggingLine);
        }
    }
}