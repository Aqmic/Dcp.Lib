using Dcp.Net.MQ.Rpc.Contract;
using Dcp.Net.MQ.Rpc.TestIn.Constract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.TestIn.RpcTest
{
    public class RpcDemo
    {
        public RpcDemo()
        {
         
        }
        public void TestIn()
        {
            var rpcTestApi = DcpApiClient.Create<IRpcTestApi>();
            rpcTestApi.ConsoleTest();
        }
    }
}
