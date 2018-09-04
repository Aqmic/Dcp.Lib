namespace Dcp.Net.MQ.Rpc.Factory
{
    using Dcp.Net.MQ.RabbitMQ;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using System;
    using System.Runtime.InteropServices;

    public static class MQFactory
    {
        public static IMessageQueue Create(DistributedMQConfig distributedMQConfig, string applicationId = null, MessageQueueTypeEnum messageQueueTypeEnum = 0)
        {
            switch (messageQueueTypeEnum)
            {
                case MessageQueueTypeEnum.RabbitMq:
                    return new RabbitMqMessageQueue(distributedMQConfig, applicationId);
            }
            return null;
        }
    }
}

