using System;
using System.Runtime.Serialization;

namespace SmartParking.Share.Exceptions
{
    public class CardInvalidStatusException : Exception
    {
        public CardInvalidStatusException()
        {
        }

        public CardInvalidStatusException(string message) : base(message)
        {
        }

        public CardInvalidStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CardInvalidStatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
