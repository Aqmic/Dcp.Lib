using Dcp.Net.MQ.Rpc.Contract;
using Dcp.Net.MQ.Rpc.Handler.Internal;
using Dcp.Net.MQ.Rpc.TestIn.Constract;
using Dynamic.Core.Models;
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
            DcpApiClient.Init(new DcpApiConfig() {
                MqAddress = "amqp://icb:icb158@220.167.101.49:13043/",
            });
        }
        public async Task<ResultModel> TestIn(string content)
        {
            var rpcTestApi = DcpApiClient.Create<IRpcTestApi>();
            return await rpcTestApi.WriteLine(content+"测试WriteLine方法=》" +DateTime.Now);
        }
    }
}
