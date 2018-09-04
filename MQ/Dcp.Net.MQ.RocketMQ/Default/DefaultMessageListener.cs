using Dcp.Net.MQ.RocketMQ.SDK;
using Geek.Net.MQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.RocketMQ.Default
{
    public delegate bool ReciveMessageHandler(MQMessage message);
    public class DefaultMessageListener : MessageListener
    {
        internal DefaultMessageListener() { }
        public static readonly DefaultMessageListener Instance = new DefaultMessageListener();

        public event ReciveMessageHandler ReciveMessaged;

        public override SDK.Action consume(Message message, ConsumeContext context)
        {
            using (context)
            {
                using (message)
                {
                    bool isOk = true;
                    if (this.ReciveMessaged != null)
                    {
                        isOk = this.ReciveMessaged.Invoke(new MQMessage()
                        {
                            Label = message.getTag(),
                            Body =Encoding.UTF8.GetBytes(message.getBody()),
                            Topic = message.getTopic(),
                            MsgId = message.getMsgID(),
                            MsgTime = DateTime.Now
                        });
                    }
                    if (isOk)
                    {
                        return SDK.Action.CommitMessage;
                    }
                    else
                    {
                        return SDK.Action.ReconsumeLater;
                    }
                }
            }
        }
    }
}
