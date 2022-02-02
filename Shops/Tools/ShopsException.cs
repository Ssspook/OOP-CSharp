using System;
namespace Shops.Tools
{
    public class ShopsException : Exception
    {
        public ShopsException()
        {
        }

        public ShopsException(string message)
            : base(message)
        {
        }

        public ShopsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}