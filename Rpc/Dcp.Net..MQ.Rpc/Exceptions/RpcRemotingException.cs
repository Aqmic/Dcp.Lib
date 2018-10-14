using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Exceptions
{
    public class RpcRemotingException:Exception
    {
        public ActionSerDes CallInfo { get; set; }

    }
}
