namespace Geek.Net.MQ
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MQMsgResponse
    {
     

        public MQMsgResponse()
        {
          
        }

        public string Exchange
        {
            get;set;
        }

        public string ResponseQueue
        {
            get;set;
        }

        public string ResponseRouteKey
        {
            get;set;
        }
    }
}

