using Dcp.Net.MQ.Rpc.Exceptions;
using Dynamic.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Handler
{
    public class DcpResponseMessage
    {
        public DcpResponseMessage()
        {
            this.StatusCode = HttpStatusCode.OK;
        }
        public HttpStatusCode StatusCode { get; set; }

        public ResultModel Result { get; set; }

        public RpcRemotingException RemotingException { get; set; }
    }
}
