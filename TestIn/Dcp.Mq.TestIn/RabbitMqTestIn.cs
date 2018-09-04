using Dcp.Net.MQ.RabbitMQ;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using Dcp.Net.MQ.RabbitMQ;
using Geek.Net.MQ.Config;
using RabbitMQ.Client;

namespace Dcp.Mq.TestIn
{
    [TestClass]
    public class RabbitMqTestIn
    {
        RabbitMqMessageQueue rmq;
        public RabbitMqTestIn()
        {
         rmq=new RabbitMqMessageQueue(new DistributedMQConfig() {
             ServerAddress= "测试地址",
             Topic="testDcpTopic",
             ProducerID="testDcpProductId",
             ConsumerID="testConsumerId"
         });
        }
        [TestMethod]
        public void Send()
        {
            for (int i = 0; i < 100; i++)
            {
                rmq.Send("我是一个测序消息", "testLab");
            }
            
        }
    }
  
}
