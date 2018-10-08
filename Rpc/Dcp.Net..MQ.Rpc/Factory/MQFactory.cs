namespace Dcp.Net.MQ.Rpc.Factory
{
    using Dcp.Net.MQ.RabbitMQ;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using Geek.Net.MQ.Factory;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class MQFactory
    {
        public static IMessageQueue Create(DistributedMQConfig distributedMQConfig, MessageQueueTypeEnum messageQueueTypeEnum = 0, IList<string> routeKeyList=null, string applicationId = null)
        {
            IMqFactory mqFactory = null;
            switch (messageQueueTypeEnum)
            {
                case MessageQueueTypeEnum.RabbitMq:
                    {
                        mqFactory = new RabbitMQ.Factory.RabbitMqFactory(distributedMQConfig, applicationId, routeKeyList);
                    };break;
                   // return new RouterMQ(distributedMQConfig,applicationId, routeKeyList);
            }
           return mqFactory.CreateRabbitFactory();
        }
    }
}

