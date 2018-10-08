using Geek.Net.MQ;
using Geek.Net.MQ.Config;
using Geek.Net.MQ.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.RabbitMQ.Factory
{
    public  class RabbitMqFactory: IMqFactory
    {
        public  DistributedMQConfig MqConfig { get; private set; }
        public string ApplicationId { get;private set; }
        public IList<string> RouteKeyList { get;private set; }
        public RabbitMqFactory(DistributedMQConfig mQConfig,string applicationId,IList<string> routeKeyList)
        {
            this.MqConfig = mQConfig;
            this.ApplicationId = applicationId;
            this.RouteKeyList = routeKeyList;
        }
        public IMessageQueue CreateRabbitFactory()
        {
            IMessageQueue messageQueue = null;
            switch (this.MqConfig.MsgSendType)
            {
                case MessageSendType.Simple: {
                        messageQueue = new SimpleMQ(this.MqConfig, this.ApplicationId);
                    };break;
                case MessageSendType.Worker: {
                        messageQueue = new WorkerMQ(this.MqConfig, this.ApplicationId);
                    };break;
                case MessageSendType.PublishOrder: {

                    };break;
                case MessageSendType.Router: {
                        messageQueue = new RouterMQ(this.MqConfig, this.ApplicationId, this.RouteKeyList);
                    };break;
            }
            return messageQueue;
        }
    }
}
