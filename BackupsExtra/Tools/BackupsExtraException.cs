using System;

namespace BackupsExtra.Service
{
    public class BackupsExtraException : Exception
    {
        public BackupsExtraException()
        {
        }

        public BackupsExtraException(string message)
            : base(message)
        {
        }

        public BackupsExtraException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}