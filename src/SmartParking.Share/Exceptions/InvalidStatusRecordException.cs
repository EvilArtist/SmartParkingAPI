using System;
using System.Runtime.Serialization;

namespace SmartParking.Share.Exceptions
{
    public class InvalidStatusRecordException : Exception
    {
        public InvalidStatusRecordException()
        {
        }

        public InvalidStatusRecordException(string message) : base(message)
        {
        }

        public InvalidStatusRecordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStatusRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
