using Dcp.Net.MQ.RocketMQ.SDK;
using System;
using System.Collections.Generic;
using System.Text;

using Dcp.Net.MQ.RocketMQ.Extions;
using Geek.Net.MQ;
using System.Threading;


namespace Dcp.Mq.TestIn.Common
{
    public class RocketMqMessageQueueTest //: MessageQueueBase
    {
        Dcp.Net.MQ.RocketMQ.RocketMQManager a;
        
         public RocketMqMessageQueueTest()
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
            int i = 0;

           a.Start(new Action<MQMessage>((msg)=> {
               i++;
               try
               {
                   if(i==10000)
                   {
                       Console.WriteLine("接收消息数：=>" + i);
                   }
                  // Console.Clear();
                   
                   // Console.WriteLine($"收到消息=>{msg.Body}");
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex);
               }
              

           }));
        }
        public void Send(string sendMsg)
        {
            a.Product.Send(sendMsg);
            
            
        }
    }
}
