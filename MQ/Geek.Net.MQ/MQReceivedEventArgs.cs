namespace Geek.Net.MQ
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MQReceivedEventArgs : EventArgs
    {
       

        public MQReceivedEventArgs()
        {
           
        }

        public MQReceivedEventArgs(bool isTimeOut)
        {
            this.IsTimeOut = isTimeOut;
            return;
        }

        public MQReceivedEventArgs(byte[] message, string label)
        {
            this.Message = new MQMessage(message, label);
            this.IsTimeOut = false;
            return;
        }

        public bool IsTimeOut
        {
            get;set;
        }

        public MQMessage Message
        {
            get;set;
        }
    }
}

