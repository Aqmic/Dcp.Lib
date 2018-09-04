using Dcp.Net.MQ.RocketMQ.SDK;
using System;
using System.Collections.Generic;
using System.Text;

using Dcp.Net.MQ.RocketMQ.Extions;
using Geek.Net.MQ;

namespace Dcp.Mq.TestIn.Common
{
    public class RocketMqMessageQueue //: MessageQueueBase
    {
        Dcp.Net.MQ.RocketMQ.RocketMQManager a;
        
        public RocketMqMessageQueue()
        {
            //118.24.153.159:9876
            //220.167.101.61  10911   9876
            a = new Dcp.Net.MQ.RocketMQ.RocketMQManager(new Dcp.Net.MQ.RocketMQ.Configuration.RocketMQItemConfig()
            {
                Server = "220.167.101.49:9876",// "220.167.101.61:9876", //"118.24.153.159:9876",//
                ConsumeThreadNums = Environment.ProcessorCount * 2,
                ProducerID = "testProducerID",
                ConsumerID = "testConsumerID",
                Topic = "testTopic",
            });


            //a.Consumer.StartListener(new System.Action<MQMessage>((msg)=> {

            //    System.Console.WriteLine(msg.Body);
            //}));
           
            //  a.Consumer.Start(messageLister);
            a.Product.Start();
            a.Product.Send("测试发生", "tag", "key");

        }
        public void Send()
        {



            a.Product.Send("测试发生", "tag", "key");
            
            
        }
    }
}
