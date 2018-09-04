using Dcp.Mq.TestIn.Common;
using System;

namespace Dcp.Net.MQ.RocketMQ.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始测试!");

            int j = 0;
            RocketMqMessageQueueTest rocketMqMessageQueue = new RocketMqMessageQueueTest();
            for (int i = 0; i < 10000; i++)
            {
                j++;
                string msg = "发送测试消息_" + i;
                if(j==10000)
                Console.WriteLine("发送消息数=》"+j);
               rocketMqMessageQueue.Send(msg);
              
            }
            Console.ReadLine();
        }
    }
}
