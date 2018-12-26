using System;

namespace Vapps.Common.Infrastructure
{
    [Serializable]
    internal class ApplicationException : Exception
    {
        private object p;

        public ApplicationException()
        {
        }

        public ApplicationException(object p)
        {
            this.p = p;
        }

        public ApplicationException(string message) : base(message)
        {
        }

        public ApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}