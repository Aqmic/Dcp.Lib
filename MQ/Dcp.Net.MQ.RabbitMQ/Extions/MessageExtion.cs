namespace Dcp.Net.MQ.RabbitMQ.Extions
{
    using Geek.Net.MQ.Config;
    using System;
    using System.Runtime.CompilerServices;

    public static class MessageExtion
    {
        public static string ToExchangeType(this MessageSendType messageSendType)
        {
            switch (messageSendType)
            {
                case MessageSendType.PublishOrder:
                    return "fanout";
                case MessageSendType.Router:
                    return "direct";
                case MessageSendType.TopicLike:
                    return "topic";
            }
            return null;
        }
        public static bool IsNeedExhange(this MessageSendType messageSendType)
        {
            if (messageSendType == MessageSendType.PublishOrder || messageSendType == MessageSendType.Router || messageSendType == MessageSendType.TopicLike)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

