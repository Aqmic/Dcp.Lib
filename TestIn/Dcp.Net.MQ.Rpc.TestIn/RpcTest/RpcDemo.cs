using Dcp.Net.MQ.Rpc.Contract;
using Dcp.Net.MQ.Rpc.Handler.Internal;
using Dcp.Net.MQ.Rpc.TestIn.Constract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.TestIn.RpcTest
{
    public class RpcDemo
    {
        public RpcDemo()
        {
         
        }
        public async Task<string> TestIn()
        {
            var rpcTestApi = DcpApiClient.Create<IRpcTestApi>();
            return await rpcTestApi.ConsoleTest();
        }
    }
}
