namespace Dcp.Net.MQ.Rpc.Const
{
    using System;

    public static class MQDefaultSetting
    {
        public static readonly string _Exchange = "RPC_EXCHANGE";
        public static readonly string _prefixQueue = "Rpc_Response_Queue";
        public static readonly string _RequestRouteKey = "Rpc_Request_RouteKey";
    }
}

