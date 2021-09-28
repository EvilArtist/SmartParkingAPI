using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Exceptions
{
    public class CardNotAssignedException : Exception
    {
        public CardNotAssignedException()
        {
        }

        public CardNotAssignedException(string message) : base(message)
        {
        }

        public CardNotAssignedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CardNotAssignedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
