using CDynamic.Command.Defaults.Dcp.Net.MQ.Rpc.Core;
using Dynamic.Core.Excuter;
using Geek.Net.MQ;
using Geek.Net.MQ.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc
{

    public abstract class RpcBase
    {
        protected readonly object _LockObj = new object();

        protected IDataList<MQMessage> _lstDeviceDataQueue = new DataList<MQMessage>();

        public abstract event ReciveMQMessageHandler ReciveMsgedEvent;

        public string ApplicationId { get;protected set; }

        public DistributedMQConfig MQConfig { get; set; }

        public IMessageQueue MsgQueue { get; protected set; }

        public RpcBase(DistributedMQConfig distributedMQConfig)
        {
            this.MQConfig = distributedMQConfig;
        }


    }
}
