using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Exceptions
{

    [Serializable]
    public class PricebookValidationException : Exception
    {
        public string Code { get; set; }
        public PricebookValidationException(string code) {
            Code = code;
        }
        public PricebookValidationException(string code, string message) : base(message){
            Code = code;
        }
        public PricebookValidationException(string message, Exception inner) : base(message, inner) { }
        protected PricebookValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
