using Dcp.Net.MQ.RocketMQ.Default;
using Geek.Net.MQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.RocketMQ.Extions
{
    public static class ConsumerExtion
    {
        /// <summary>
        /// 开启消息监听
        /// </summary>
        /// <param name="rocketMQConsumer"></param>
        /// <param name="lable"></param>
        /// <param name="action"></param>
        public static void StartListener(this IRocketMQConsumer rocketMQConsumer, Action<MQMessage> action, string lable = "*")
        {
            DefaultMessageListener defaultMessageListener = new DefaultMessageListener();

            defaultMessageListener.ReciveMessaged += ((msg)=> {
                if (action != null)
                {
                    if (string.IsNullOrEmpty(lable) || (lable.Equals(msg.Label)))
                    {
                        action.Invoke(msg);
                    }
                }
                return true;
            });
            rocketMQConsumer.Start(defaultMessageListener, lable);
        }

      
    }
}
