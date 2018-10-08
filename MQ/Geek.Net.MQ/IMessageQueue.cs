namespace Geek.Net.MQ
{
    using Geek.Net.MQ.Config;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public interface IMessageQueue : IDisposable
    {
        void BindConfig(string queue, IList<string> routeKeyList);
        void BindConfig(string queue, string routeKey);
        bool CloseMQ();
        IMessageQueue CreateInstance(DistributedMQConfig mqConfig);
        bool CreateMQ(IList<string> routeKeyList=null);
        bool DeleteMQ(string queue, bool ifUnused, bool ifEmpty);
        void ReceiveBinary(Action<byte[]> action);
        void ReceiveMQ(Action<MQMessage> action);
        void Send(MQMessage mQMessage, [Optional, DefaultParameterValue(null)] Action<MQMessage> callBackAction);
        bool Send(string message, [Optional, DefaultParameterValue(null)] string label);
        void Send(byte[] body, [Optional, DefaultParameterValue(null)] string label);
        void SendAsync(string message, [Optional, DefaultParameterValue(null)] string label);

        DistributedMQConfig MQConfig { get; }
    }
}

