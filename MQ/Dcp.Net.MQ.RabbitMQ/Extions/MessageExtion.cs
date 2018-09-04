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
                case MessageSendType.P2P:
                    return "direct";

                case MessageSendType.RadioBroadcast:
                    return "fanout";

                case MessageSendType.Topic:
                    return "topic";
            }
            return null;
        }
    }
}

