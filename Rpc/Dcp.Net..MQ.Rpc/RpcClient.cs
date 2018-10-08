namespace Dcp.Net.MQ.Rpc
{
    using CDynamic.Command.Defaults.Dcp.Net.MQ.Rpc.Core;
    using Dcp.Net.MQ.Rpc.Const;
    using Dcp.Net.MQ.Rpc.Core;
    using Dcp.Net.MQ.Rpc.Factory;
    using Dynamic.Core.Comm;
    using Dynamic.Core.Excuter;
    using Dynamic.Core.Models;
    using Dynamic.Core.Runtime;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class RpcClient : RpcBase
    {


        public RpcClient(DistributedMQConfig distributedMQConfig, MQMsgRequest mQMsgRequest = null) : base(distributedMQConfig)
        {
            this.MQConfig = distributedMQConfig;
            this.MsgQueue = MQFactory.Create(this.MQConfig, MessageQueueTypeEnum.RabbitMq, null, ((int)Process.GetCurrentProcess().Id).ToString());

            if (!string.IsNullOrEmpty(this.MQConfig.ProducerID))
            {
                this.MsgQueue.DeleteMQ(this.MQConfig.ProducerID, true, true);
            }
            
            string str = IdentityHelper.NewSequentialGuid().ToString("N");
            if (string.IsNullOrEmpty(this.MQConfig.Exchange))
            {
                this.MQConfig.Exchange = MQDefaultSetting._Exchange;
            }
            MQMsgResponse response1 = new MQMsgResponse
            {
                Exchange = this.MQConfig.Exchange,
                ResponseQueue = MQDefaultSetting._prefixQueue + "-" + str,
                ResponseRouteKey = str
            };
            this._mQMsgResponse = response1;
            if (mQMsgRequest == null)
            {
                MQMsgRequest request1 = new MQMsgRequest
                {
                    Exchange = this.MQConfig.Exchange,
                    RequestRouteKey = MQDefaultSetting._RequestRouteKey
                };
                this._mQMsgRequest = request1;
            }
            else
            {
                this._mQMsgRequest = mQMsgRequest;
            }
        }

        public override event ReciveMQMessageHandler ReciveMsgedEvent;

        public T Call<T>(object sendObj, int timeoutMilliseconds) where T : class
        {
            bool flag = false;
            Type type = typeof(T);
            if (type.Name.Equals("string", (StringComparison)StringComparison.OrdinalIgnoreCase))
            {
                flag = true;
            }
            byte[] bytes = null;
            if (flag)
            {
                bytes = Encoding.UTF8.GetBytes((string)((string)sendObj));
            }
            else
            {
                bytes = SerializationUtility.ToBytes(sendObj);
            }
            T local = default(T);
            MQMessage requestMsg = new MQMessage();
            object obj2 = this._LockObj;
            lock (obj2)
            {
                requestMsg.Body = bytes;
                requestMsg.MsgId = IdentityHelper.NewSequentialGuid().ToString("N");

                this.Send(requestMsg);
                DataItem<MQMessage> item = this._lstDeviceDataQueue.Pull(f => f.Data != null && f.Data.MsgId == requestMsg.MsgId, TimeSpan.FromMilliseconds((double)timeoutMilliseconds));
                if (((item != null) && (item.Data != null)) && (item.Data.Body != null))
                {
                    if (flag)
                    {
                        local = Encoding.UTF8.GetString(item.Data.Body) as T;
                    }
                    else
                    {
                        local = SerializationUtility.BytesToObject<T>(item.Data.Body);
                    }
                }
                return local;
            }
        }

        public T Call<T>(MQMessage sendMsg, int timeoutMilliseconds) where T : class
        {
            bool flag = false;
            Type type = typeof(T);

            T local = default(T);

            object obj2 = this._LockObj;
            lock (obj2)
            {
                this.Send(sendMsg);
                DataItem<MQMessage> item = this._lstDeviceDataQueue.Pull(f => f.Data != null && f.Data.MsgId == sendMsg.MsgId, TimeSpan.FromMilliseconds((double)timeoutMilliseconds));
                if (((item != null) && (item.Data != null)) && (item.Data.Body != null))
                {
                    if (flag)
                    {
                        local = Encoding.UTF8.GetString(item.Data.Body) as T;
                    }
                    else
                    {
                        local = SerializationUtility.BytesToObject<T>(item.Data.Body);
                    }
                }
                return local;
            }
        }

        public int GetReplyCount() =>
            this._lstDeviceDataQueue.Count();

        public void Send(MQMessage mQMessage)
        {
            object obj2 = this._LockObj;
            lock (obj2)
            {
                if (mQMessage.Response == null)
                {
                    MQMsgResponse response1 = new MQMsgResponse
                    {
                        Exchange = this.MQConfig.Exchange
                    };
                    mQMessage.Response = response1;
                }
                mQMessage.Response.ResponseQueue = this._mQMsgResponse.ResponseQueue;
                mQMessage.Response.ResponseRouteKey = this._mQMsgResponse.ResponseRouteKey;
                if (mQMessage.Request == null)
                {
                    mQMessage.Request = this._mQMsgRequest;
                }
                if (string.IsNullOrEmpty(mQMessage.Label))
                {
                    mQMessage.Label = ((int)Process.GetCurrentProcess().Id).ToString();
                }
                this.MsgQueue.BindConfig(this._mQMsgResponse.ResponseQueue, this._mQMsgResponse.ResponseRouteKey);
                this.MsgQueue.Send(mQMessage, (msg) =>
                {
                    DataItem<MQMessage> command = new DataItem<MQMessage>();
                    command.Data = msg;
                    command.ReceivedTime = DateTime.Now;
                    this._lstDeviceDataQueue.Push(command);
                    Task.Factory.StartNew(() =>
                    {
                        if (this.ReciveMsgedEvent != null)
                        {
                            this.ReciveMsgedEvent(msg);
                        }
                    });

                });
            }
        }

        public MQMsgRequest _mQMsgRequest { get; private set; }

        public MQMsgResponse _mQMsgResponse { get; private set; }

    }
}

