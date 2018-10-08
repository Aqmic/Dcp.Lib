using Geek.Net.MQ.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geek.Net.MQ
{
    public abstract class MessageQueueBase : IMessageQueue
    {
        DistributedMQConfig mQConfig = null;
        public DistributedMQConfig MQConfig => mQConfig;

        protected MessageQueueBase(DistributedMQConfig mqConfig)
        {
            this.mQConfig = mqConfig;
        }

        #region IMessageQueue 成员

        public abstract bool Send(string message, string label);

        public abstract void SendAsync(string message, string label);

        public abstract void Send(byte[] body, string label = null);


        public abstract bool CreateMQ(IList<string> routeKeyList=null);

        public abstract bool CloseMQ();

        public abstract IMessageQueue CreateInstance(DistributedMQConfig mqConfig);

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.CloseMQ();
                    // TODO: 释放托管状态(托管对象)。
                }
                
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MessageQueueBase() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        public abstract void ReceiveMQ(Action<MQMessage> action);


        public abstract void ReceiveBinary(Action<byte[]> action);

       

        public abstract void Send(MQMessage mQMessage,Action<MQMessage> callBackAction);

        public abstract bool DeleteMQ(string queue, bool ifUnused, bool ifEmpty);

        public virtual void BindConfig(string queue, string routeKey)
        {
            throw new NotSupportedException();
        }

        public virtual void BindConfig(string queue, IList<string> routeKeyList)
        {
            throw new NotSupportedException();
        }
        
        #endregion
        #endregion
    }
}
