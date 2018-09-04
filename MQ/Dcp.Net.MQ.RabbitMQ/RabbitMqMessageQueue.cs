namespace Dcp.Net.MQ.RabbitMQ
{
    using Dcp.Net.MQ.RabbitMQ.Extions;
    using Dynamic.Core.Auxiliary;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using Geek.Net.MQ.Extions;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
   
    public class RabbitMqMessageQueue : MessageQueueBase
    {
        private IModel _channel;
        private static IConnection _connection;
        private readonly Encoding _encoding;
        private readonly string _queue;



        public RabbitMqMessageQueue(DistributedMQConfig mqConfig, string applicationId = null) : base(mqConfig)
        {
            this._encoding = mqConfig.DefaultEncoding;
            ConnectionFactory factory1 = new ConnectionFactory();
            factory1.AutomaticRecoveryEnabled = true;
            factory1.TopologyRecoveryEnabled = true;
            factory1.Uri = new Uri(mqConfig.ServerAddress);
            ConnectionFactory factory = factory1;
            this._ApplicationCode = applicationId;
            this._queue = base.MQConfig.ProducerID;
            _connection = factory.CreateConnection(mqConfig.Name);
            this.CreateMQ();
        }

        public override void BindConfig(string queue, string routeKey)
        {
            this.Channel.QueueDeclare(queue, base.MQConfig.IsDurable, false, base.MQConfig.AutoDelete, null);
            IModelExensions.QueueBind(this.Channel, queue, base.MQConfig.Topic, routeKey, null);
        }

        private void CallBack(object obj, BasicDeliverEventArgs ea, Action<MQMessage> callAction)
        {
            if (callAction != null)
            {
                try
                {
                    MQMessage message = ea.Body.ToMessage();
                    message.MsgId = ea.BasicProperties.MessageId;
                    if (string.IsNullOrEmpty(this._ApplicationCode) || (!string.IsNullOrEmpty(this._ApplicationCode) && (this._ApplicationCode == ea.BasicProperties.CorrelationId)))
                    {
                        message.Label = ea.BasicProperties.CorrelationId;
                        message.MsgId = ea.BasicProperties.MessageId;
                        if (message.Response != null)
                        {
                            MQMsgRequest request1 = new MQMsgRequest
                            {
                                Exchange = message.Response.Exchange,
                                RequestRouteKey = message.Response.ResponseRouteKey
                            };
                            message.Request = request1;
                        }
                        callAction(message);
                    }
                    else
                    {
                        IOHelper.WriteLine("没有匹配到路由！", (ConsoleColor)ConsoleColor.Red);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    this.Channel.BasicAck(ea.DeliveryTag, false);
                }
            }
        }

        public override bool CloseMQ()
        {
            if (this._channel != null)
            {
                this._channel.Dispose();
            }
            if (_connection != null)
            {
                _connection.Dispose();
            }
            return true;
        }

        public override IMessageQueue CreateInstance(DistributedMQConfig mqConfig) =>
            new RabbitMqMessageQueue(mqConfig, null);

        public override bool CreateMQ()
        {
            this._channel = _connection.CreateModel();
            IModelExensions.ExchangeDeclare(this._channel, base.MQConfig.Topic, base.MQConfig.MsgSendType.ToExchangeType(), base.MQConfig.IsDurable, base.MQConfig.AutoDelete, null);
            this._channel.QueueDeclare(this._queue, base.MQConfig.IsDurable, false, base.MQConfig.AutoDelete, null);
            IModelExensions.QueueBind(this._channel, this._queue, base.MQConfig.Topic, base.MQConfig.ConsumerID, null);
            return true;
        }

        public override bool DeleteMQ(string queue, bool ifUnused, bool ifEmpty)
        {
            this.Channel.QueueDeleteNoWait(queue, ifUnused, ifEmpty);
            return true;
        }

        public override void ReceiveBinary(Action<byte[]> action)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(this.Channel);

            consumer.Received += (ch, ea) =>
            {
                try
                {
                    action(ea.Body);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    this.Channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            this.Channel.BasicQos(0, 1, false);
            string str = IModelExensions.BasicConsume(this.Channel, this._queue, false, consumer);
        }

        public override void ReceiveMQ(Action<MQMessage> action)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(this.Channel);
            consumer.Received += (ch, ea) =>
            {
                this.CallBack(ch, ea, action);
            };

            this.Channel.BasicQos(0, 1, false);
            string str = IModelExensions.BasicConsume(this.Channel, this._queue, false, consumer);
        }

        public override void Send(MQMessage mQMessage, Action<MQMessage> callBackAction)
        {
            string topic = base.MQConfig.Topic;
            string consumerID = base.MQConfig.ConsumerID;
            IBasicProperties properties = this.Channel.CreateBasicProperties();
            if (mQMessage.Response != null)
            {
                properties.ReplyTo = mQMessage.Response.ResponseQueue;
            }
            if (mQMessage.Request != null)
            {
                consumerID = mQMessage.Request.RequestRouteKey;
                topic = mQMessage.Request.Exchange;
            }
            properties.DeliveryMode = base.MQConfig.IsDurable ? ((byte)2) : ((byte)1);
            properties.ContentEncoding = this._encoding.EncodingName;
            if (!string.IsNullOrEmpty(mQMessage.MsgId))
            {
                properties.MessageId = mQMessage.MsgId;
            }
            if (!string.IsNullOrEmpty(mQMessage.Label))
            {
                properties.CorrelationId = mQMessage.Label;
            }
            IModelExensions.BasicPublish(this.Channel, topic, consumerID, properties, mQMessage.ToData());
            if ((mQMessage.Response != null) && (this.Consumer == null))
            {
                this.Consumer = new EventingBasicConsumer(this.Channel);
                IModelExensions.BasicConsume(this.Channel, mQMessage.Response.ResponseQueue, false, this.Consumer);
                if (callBackAction != null)
                {
                    this.Consumer.Received += (obj, ee) =>
                    {
                        if (callBackAction != null)
                        {
                            this.CallBack(obj, ee, callBackAction);
                        }
                    };
                }
            }
        }

        public override bool Send(string message, string label)
        {
            byte[] bytes = this._encoding.GetBytes(message);
            this.Send(bytes, label);
            return true;
        }

        public override void Send(byte[] body, string label = null)
        {
            IBasicProperties properties = this.Channel.CreateBasicProperties();
            properties.DeliveryMode = base.MQConfig.IsDurable ? ((byte)2) : ((byte)1);
            properties.ContentEncoding = this._encoding.EncodingName;
            if (label != null)
            {
                properties.CorrelationId = label;
            }
            IModelExensions.BasicPublish(this.Channel, base.MQConfig.Topic, base.MQConfig.ConsumerID, properties, body);
        }

        public override void SendAsync(string message, string label)
        {
            Task.Factory.StartNew(delegate
            {
                this.Send(message, label);
            });
        }

        public string _ApplicationCode { get; private set; }

        public IModel Channel
        {
            get
            {
                if ((this._channel == null) || !this._channel.IsOpen)
                {
                    if (_connection == null)
                    {
                        return null;
                    }
                    this.CreateMQ();
                }
                return this._channel;
            }
        }

        private EventingBasicConsumer Consumer { get; set; }
    }
}

