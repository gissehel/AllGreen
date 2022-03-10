using System;

namespace AllGreen.Lib
{
    [Serializable]
    public class AllGreenException : Exception
    {
        private AllGreenException() : base()
        {
        }

        public AllGreenException(string message) : base(message)
        {
        }

        public AllGreenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}