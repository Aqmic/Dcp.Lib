namespace Dcp.Net.MQ.Rpc
{
    using Dcp.Net.MQ.Rpc.Factory;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class RpcServer:RpcBase
    {
        
        public override event ReciveMQMessageHandler ReciveMsgedEvent;

        public RpcServer(DistributedMQConfig distributedMQConfig):base(distributedMQConfig)
        {
            this.MQConfig = distributedMQConfig;
            this.MsgQueue = MQFactory.Create(distributedMQConfig, null, MessageQueueTypeEnum.RabbitMq);
            this.MsgQueue.ReceiveMQ(delegate (MQMessage msg) {
                if ((msg != null) && (msg.Response !=null))
                {
                    MQMsgRequest request1 = new MQMsgRequest {
                        Exchange = msg.Response.Exchange,
                        RequestRouteKey = msg.Response.ResponseRouteKey
                    };
                    msg.Request = request1;
                    msg.Response = null;
                }
                if (this.ReciveMsgedEvent !=null)
                {
                    this.ReciveMsgedEvent(msg);
                }
            });
        }

        public void Send(MQMessage mQMessage)
        {
            this.MsgQueue.Send(mQMessage, null);
        }

      
    }
}

