using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Handler
{
    public class DcpRequestMessage
    {
        public string Content { get; set; }
        public string RequestUri { get; set; }
    }
}
