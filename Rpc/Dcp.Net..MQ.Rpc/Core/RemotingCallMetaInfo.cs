namespace Dcp.Net.MQ.Rpc.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class RemotingCallMetaInfo<T>
    {
        public readonly string _RouteUrlTemplate="dcp://exchange.routekey/namespace.interface/methodinfo";


        public RemotingCallMetaInfo()
        {
            
        }
        public RemotingCallMetaInfo(T paramter,string routeUrl)
        {
            this.Paramter = paramter;
            this.RouteUrl = routeUrl;
        }
        public T Paramter { get; set; }

        public string RouteUrl { get; set; }
    }
}

