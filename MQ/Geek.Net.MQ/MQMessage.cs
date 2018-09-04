namespace Geek.Net.MQ
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MQMessage
    {
      

        public MQMessage()
        {
            return;
        }

        public MQMessage(byte[] body, string label)
        {
            this.Label = label;
            this.Body = body;
         
        }

        public byte[] Body
        {
            get;set;
        }

        public string Label
        {
            get;set;
        }

        public string MsgId
        {
            get;set;
        }

        public DateTime MsgTime
        {
            get;set;
        }

        public MQMsgRequest Request
        {
            get;set;
        }

        public MQMsgResponse Response
        {
            get;set;
        }

        public string Topic
        {
            get;set;
        }
    }
}

