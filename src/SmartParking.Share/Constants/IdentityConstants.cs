using System;

namespace SmartParking.Share.Constants
{
    public class IdentityConstants
    {
        public class Scope
        {
            public const string Api = "api";
            public const string Scope1 = "scope1";
            public const string Scope2 = "scope2";
        }

        public class ClientId
        {
            public const string Api = "smartparking.api";
            public const string Spa = "smartparking.spa";
            public const string Interactive = "interactive";
        }

        public class ClientIdSecret
        {
            public const string Api = "0940Be70B04561467eBb6Fe1498E7dA5";
            public const string Interactive = "29D419eF69b294077Aa40CeD43c694C0";
        }

        public class CustomClaimTypes
        {
            public const string Id = "id";
            public const string ClientId = "ClientId";
        }
    }
}
