using Geek.Net.MQ;
using Geek.Net.MQ.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.RocketMQ
{
    public class RocketMqMessageQueue : MessageQueueBase
    {
        protected RocketMqMessageQueue(DistributedMQConfig mqConfig) : base(mqConfig)
        {
        }

        public override void BindConfig(string queue, string routeKey)
        {
            throw new NotImplementedException();
        }

        public override bool CloseMQ()
        {
            throw new NotImplementedException();
        }

        public override IMessageQueue CreateInstance(DistributedMQConfig mqConfig)
        {
            throw new NotImplementedException();
        }

        public override bool CreateMQ()
        {
            throw new NotImplementedException();
        }

        public override bool DeleteMQ(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public override void ReceiveBinary(Action<byte[]> action)
        {
            throw new NotImplementedException();
        }

        public override void ReceiveMQ(Action<MQMessage> action)
        {
            throw new NotImplementedException();
        }

        public override bool Send(string message, string label)
        {
            throw new NotImplementedException();
        }

        public override void Send(byte[] body, string label)
        {
            throw new NotImplementedException();
        }

        public override void Send(MQMessage mQMessage, Action<MQMessage> callBackAction)
        {
            throw new NotImplementedException();
        }

        public override void SendAsync(string message, string label)
        {
            throw new NotImplementedException();
        }
    }
}
