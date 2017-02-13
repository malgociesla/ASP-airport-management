using System;

namespace Utils
{
    [Serializable]
    public class UtilsException : ApplicationException
    {
        public UtilsException() { }
        public UtilsException(string message) : base(message) { }
        public UtilsException(string message, Exception inner) : base(message, inner) { }
        protected UtilsException(System.Runtime.Serialization.SerializationInfo info,
                                 System.Runtime.Serialization.StreamingContext context)
                                : base(info, context) { }
}
}
