using Dcp.Net.MQ.Rpc.Handler;
using Geek.Net.MQ.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dcp.Net.MQ.Rpc
{
    public class DcpApiConfig : IDisposable
    {
        private static string _mqAddress = File.ReadAllText(@"d:\mqaddress.txt");
        /// <summary>
        /// 获取全局过滤器集合
        /// 非线程安全类型
        /// </summary>
        public GlobalFilterCollection GlobalFilters { get; private set; } = new GlobalFilterCollection();
        public RpcClient ApiClient
        {
            get => this.GetDcpApiClientSafeSync();
        }
        private RpcClient GetDcpApiClientSafeSync()
        {

            //DistributedMQConfig distributedMQConfig = new DistributedMQConfig
            //{
            //    ServerAddress = _mqAddress,
            //    Topic = "RPC_EXCHANGE",
            //    ProducerID = "Rpc_Request_Queque",
            //    ConsumerID = "Rpc_Request_RouteKey",
            //    MsgSendType = MessageSendType.P2P,
            //    IsDurable = false
            //};

            var rpcClient=new RpcClient(new DistributedMQConfig
           {
               ServerAddress = _mqAddress,
               Topic = "RPC_EXCHANGE",
               ProducerID = "Rpc_Response_Queue",
               ConsumerID = "Rpc_Response_RouteKey",
               MsgSendType = MessageSendType.P2P,
               IsDurable = false
           }, null);
            return rpcClient;
        }
        /// <summary>
        /// 获取或设置是否对参数的属性值进行输入有效性验证
        /// 默认为true
        /// </summary>
        public bool UseParameterPropertyValidate { get; set; } = true;

        public Uri HttpHost { get; set; }
        public void Dispose()
        {
           
        }
    }
}
