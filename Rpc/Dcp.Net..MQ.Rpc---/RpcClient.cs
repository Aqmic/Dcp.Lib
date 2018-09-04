namespace Dcp.Net.MQ.Rpc
{
    using Dcp.Net.MQ.Rpc.Const;
    using Dcp.Net.MQ.Rpc.Core;
    using Dcp.Net.MQ.Rpc.Factory;
    using Dynamic.Core.Comm;
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

    public class RpcClient
    {
        private readonly object _LockObj = new object();
        private DataList<MQMessage> _lstDeviceDataQueue = new DataList<MQMessage>();
        [CompilerGenerated, DebuggerBrowsable((DebuggerBrowsableState) DebuggerBrowsableState.Never)]
        private MQMsgRequest <_mQMsgRequest>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable((DebuggerBrowsableState) DebuggerBrowsableState.Never)]
        private MQMsgResponse <_mQMsgResponse>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable((DebuggerBrowsableState) DebuggerBrowsableState.Never)]
        private DistributedMQConfig <MQConfig>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable((DebuggerBrowsableState) DebuggerBrowsableState.Never)]
        private IMessageQueue <MsgQueue>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ReciveMQMessageHandler ReciveMsgedEvent;

        public RpcClient(DistributedMQConfig distributedMQConfig, MQMsgRequest mQMsgRequest = null)
        {
            this.MQConfig = distributedMQConfig;
            this.MsgQueue = MQFactory.Create(this.MQConfig, ((int) Process.GetCurrentProcess().Id).ToString(), MessageQueueTypeEnum.RabbitMq);
            this.MsgQueue.DeleteMQ(this.MQConfig.ProducerID, true, true);
            string str = IdentityHelper.NewSequentialGuid().ToString("N");
            if (string.IsNullOrEmpty(this.MQConfig.Topic))
            {
                this.MQConfig.Topic = MQDefaultSetting._Exchange;
            }
            MQMsgResponse response1 = new MQMsgResponse {
                Exchange = this.MQConfig.Topic,
                ResponseQueue = MQDefaultSetting._prefixQueue + "-" + str,
                ResponseRouteKey = str
            };
            this._mQMsgResponse = response1;
            if (mQMsgRequest == null)
            {
                MQMsgRequest request1 = new MQMsgRequest {
                    Exchange = this.MQConfig.Topic,
                    RequestRouteKey = MQDefaultSetting._RequestRouteKey
                };
                this._mQMsgRequest = request1;
            }
            else
            {
                this._mQMsgRequest = mQMsgRequest;
            }
        }

        public T Call<T>(object sendObj, int timeoutMilliseconds) where T: class
        {
            bool flag = false;
            Type type = typeof(T);
            if (type.Name.Equals("string", (StringComparison) StringComparison.OrdinalIgnoreCase))
            {
                flag = true;
            }
            byte[] bytes = null;
            if (flag)
            {
                bytes = Encoding.get_UTF8().GetBytes((string) ((string) sendObj));
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
                DataItem<MQMessage> item = this._lstDeviceDataQueue.Pull(delegate (DataItem<MQMessage> f) {
                    return (f.get_Data() != null) && (f.get_Data().MsgId == requestMsg.MsgId);
                }, TimeSpan.FromMilliseconds((double) timeoutMilliseconds));
                if (((item != null) && (item.get_Data() != null)) && (item.get_Data().Body > null))
                {
                    if (flag)
                    {
                        local = Encoding.get_UTF8().GetString(item.get_Data().Body) as T;
                    }
                    else
                    {
                        local = SerializationUtility.BytesToObject<T>(item.get_Data().Body);
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
                    MQMsgResponse response1 = new MQMsgResponse {
                        Exchange = this.MQConfig.Topic
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
                    mQMessage.Label = ((int) Process.GetCurrentProcess().Id).ToString();
                }
                this.MsgQueue.BindConfig(this._mQMsgResponse.ResponseQueue, this._mQMsgResponse.ResponseRouteKey);
                this.MsgQueue.Send(mQMessage, delegate (MQMessage msg) {
                    DataItem<MQMessage> command = new DataItem<MQMessage>();
                    command.set_Data(msg);
                    command.set_ReceivedTime(DateTime.get_Now());
                    this._lstDeviceDataQueue.Push(command);
                    Task.get_Factory().StartNew(delegate {
                        if (this.ReciveMsgedEvent > null)
                        {
                            this.ReciveMsgedEvent(msg);
                        }
                    });
                });
            }
        }

        public MQMsgRequest _mQMsgRequest { get; private set; }

        public MQMsgResponse _mQMsgResponse { get; private set; }

        public DistributedMQConfig MQConfig { get; set; }

        public IMessageQueue MsgQueue { get; private set; }
    }
}

