using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Exceptions
{
    public class RpcRemotingException : Exception
    {
        public ActionSerDes CallInfo { get; set; }

       public string OriMessage { get; set; }
        public override string Message
        {
            get
            {
                return this.OriMessage;
            }
        }
        public override string Source { get; set; }

        public  string OriInnerExceptionStr { get; set; }

        public string OriStackTrace { get; set; }
        public override string StackTrace
        {
            get
            {
                return this.OriStackTrace;
            }
        }
        public RpcRemotingException(string msg, string source, string stackTrace)
        {
          
            this.Source = source;
            this.OriMessage = msg;
            this.OriStackTrace = source;
        }
    }
}
