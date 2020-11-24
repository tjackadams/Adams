using System;

namespace Adams.Services.Smoking.Domain.Exceptions
{
    public class SmokingDomainException : Exception
    {
        public SmokingDomainException()
        {
        }

        public SmokingDomainException(string message)
            : base(message)
        {
        }

        public SmokingDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}