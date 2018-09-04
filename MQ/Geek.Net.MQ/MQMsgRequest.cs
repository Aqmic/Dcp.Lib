namespace Geek.Net.MQ
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MQMsgRequest
    {
      
        public MQMsgRequest()
        {
           
        }

        public string Exchange
        {
            get;set;
        }

        public string RequestRouteKey
        {
            get;set;
        }
    }
}

