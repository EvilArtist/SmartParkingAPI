using System;
using System.Runtime.Serialization;

namespace SmartParking.Share.Exceptions
{
    public class CardNotFoundException : Exception
    {
        public CardNotFoundException()
        {
        }

        public CardNotFoundException(string message) : base(message)
        {
        }

        public CardNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CardNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
