using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Config
{
    public  class MqRpcConfig
    {
        public MqRpcConfig()
        {

        }
        public MqRpcConfig(string mqAddress)
        {
            this.MqAddress = mqAddress;
        }
        public readonly static string _MqRpcDefaultKeyName = "rpc:default_rabbitmq_rpc_config";
        public string MqAddress { get; set; }
        public string Exchange { get; set; }
        public int RequestTimeOut { get; set; }

        public string ApplicationId { get; set; }
    }
}
