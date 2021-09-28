using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class OperationConstants
    {
        public class Action
        {
            public const string ScanCard = "CARD_SCAN";
            public const string AllowIn = "OPEN_GATE_IN";
            public const string AllowOut = "OPEN_GATE_OUT";
            public const string Subscribe = "SUBSCRIBE";
            public const string Unsubscribe = "UNSUBSCRIBE";
        }
    }
}
