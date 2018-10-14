using Dcp.Net.MQ.Rpc.Handler;
using Dynamic.Core.Service;
using Geek.Net.MQ.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dcp.Net.MQ.Rpc
{
    public class DcpApiConfig : IDisposable
    {
        public string MqAddress { get; set; }
        public string Exchange { get; set; }
        public DcpApiConfig()
        {
            this.TimeOut = 30 * 1000;
            this.Exchange = "RPC_EXCHANGE";
        }
        public int TimeOut { get; set; }
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
            var rpcClient=IocUnity.Get<RpcClient>();
            if (rpcClient == null)
            {
                rpcClient = new RpcClient(new DistributedMQConfig
                {
                    ServerAddress = MqAddress,
                    Exchange = this.Exchange,
                    MsgSendType = MessageSendType.Worker,
                    IsDurable = false
                }, null);
                IocUnity.AddSingleton<RpcClient>(rpcClient);
            }
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
