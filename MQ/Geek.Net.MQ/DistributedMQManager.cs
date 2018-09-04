using System;
using System.Collections.Generic;
using System.Text;
using Geek.Net.MQ.Config;

namespace Geek.Net.MQ
{
    public class DistributedMQManager : IMessageQueue
    {
        protected DistributedMQConfig _mQconfig;
        public DistributedMQConfig MQConfig => _mQconfig;

        public static IDictionary<MessageQueueTypeEnum, IMessageQueue> _CacheMQManager = new Dictionary<MessageQueueTypeEnum, IMessageQueue>();
        private readonly static object _lockObj = new object();
        public static IMessageQueue Get(MessageQueueTypeEnum mQTypeEnum) {

            IMessageQueue messageQueue = null;
            lock (_lockObj)
            {
                if (_CacheMQManager.ContainsKey(mQTypeEnum))
                {
                    messageQueue = _CacheMQManager[mQTypeEnum];
                }
                return messageQueue;
            }
        }

        public bool CloseMQ()
        {
            throw new NotImplementedException();
        }
    
        public IMessageQueue CreateInstance(DistributedMQConfig mqConfig)
        {
            throw new NotImplementedException();
        }

        public bool CreateMQ()
        {
            throw new NotImplementedException();
        }

    

        public bool Send(string message, string label)
        {
            throw new NotImplementedException();
        }

        public void SendAsync(string message, string label)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                //if (disposing)
                //{
                //    TODO: 释放托管状态(托管对象)。
                //    this.CloseMQ();
                //}
                this.CloseMQ();
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~DistributedMQManager()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        public bool Send(string message, string label, bool isAsync)
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] body, string label)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMQ(Action<MQMessage> action)
        {
            throw new NotImplementedException();
        }

        public void ReceiveBinary(Action<byte[]> action)
        {
            throw new NotImplementedException();
        }

        public void BindConfig(string queue, string routeKey)
        {
            throw new NotImplementedException();
        }

        public void Send(MQMessage mQMessage, Action<MQMessage> callBackAction)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMQ(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
