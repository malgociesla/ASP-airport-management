using System;

namespace AirportService
{
    [Serializable]
    public class AirportServiceException : ApplicationException
    {
        public AirportServiceException() { }
        public AirportServiceException(string message) : base(message) { }
        public AirportServiceException(string message, Exception inner) : base(message, inner) { }
        protected AirportServiceException(System.Runtime.Serialization.SerializationInfo info,
                                 System.Runtime.Serialization.StreamingContext context)
                                : base(info, context) { }
    }
}
