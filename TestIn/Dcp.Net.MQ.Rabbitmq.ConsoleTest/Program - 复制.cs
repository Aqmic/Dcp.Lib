//using Dcp.Net.MQ.RabbitMQ;
//using Geek.Net.MQ;
//using Geek.Net.MQ.Config;
//using System;

//using Geek.Net.MQ.Extions;
//using Dynamic.Core.Comm;

//namespace Dcp.Net.MQ.Rabbitmq.ConsoleTest
//{
//    class Program
//    {
//        static RabbitMqMessageQueue rmq = new RabbitMqMessageQueue(new DistributedMQConfig()
//        {
//            ServerAddress = "测试地址", //"amqp://icb:a123456@192.168.0.251:5672/icb",//
//            Topic = "RPC_EXCHANGE",
//            ProducerID = "Rpc_Request_Queue",
//            ConsumerID = "Rpc_Request_RouteKey",
//            MsgSendType = MessageSendType.RadioBroadcast
//        });
//        static RabbitMqMessageQueue responseMQ = new RabbitMqMessageQueue(new DistributedMQConfig()
//        {
//            ServerAddress = "测试地址", //"amqp://icb:a123456@192.168.0.251:5672/icb",//
//            Topic = "RPC_EXCHANGE",
//            ProducerID = "Rpc_Response_Queue",
//            ConsumerID = "Rpc_Response_RouteKey"
//        });
//        static void Main(string[] args)
//        {
//            rmq.ReceiveMQ(new Action<Geek.Net.MQ.MQMessage>((obj) =>
//            {
//                Console.WriteLine(DateTime.Now + obj.Label+"服务器收到消息=》" + obj.Body);
//                obj.Label = obj.ResponseRouteKey;
//                responseMQ.Send(obj,null);

//            }));
//            responseMQ.ReceiveMQ(new Action<Geek.Net.MQ.MQMessage>((obj) =>
//            {
//                Console.WriteLine(DateTime.Now + "客户端收到消息=》" + obj.Label);
//            }));

//            int i = 0;
//            while (Console.ReadLine() != "0")
//            {
//                i++;
//                var msg = new MQMessage()
//                {
//                  //  Label = IdentityHelper.NewSequentialGuid().ToString("N"),
//                    Body = "rpc测试消息".ToUTF8Bytes(),
//                    ResponseQueue= "Rpc_Response_Queue",
//                    ResponseRouteKey= IdentityHelper.NewSequentialGuid().ToString("N")
//                };
//                rmq.Send(msg , responseMQ);
//                Console.WriteLine(DateTime.Now);
//            }

//    }
      
//    }
//}
