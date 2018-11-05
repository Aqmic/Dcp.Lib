using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Exceptions
{
    public class RpcRemotingException : Exception
    {
        private string messagePro;
        private string stackTrace;
        public ActionSerDes CallInfo { get; set; }
        public override string Message
        {
            get
            {
                return messagePro;
            }
        }
        public override string Source { get; set; }
        public override string StackTrace
        {
            get
            {
                return stackTrace;
            }
        }
        public RpcRemotingException(string msg, string source, string stackTrace)
        {
            this.messagePro = msg;
            this.Source = source;
            this.stackTrace = stackTrace;
        }
    }
}
