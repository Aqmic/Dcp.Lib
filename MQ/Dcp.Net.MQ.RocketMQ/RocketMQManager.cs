// ********************************************
// 作者：huajunsoft
// 时间：2018-08-11 22:50
// ********************************************

using Dcp.Net.Configuration;
using Dcp.Net.MQ.RocketMQ.Configuration;
using Dcp.Net.MQ.RocketMQ.DynamicLib;
using Dcp.Net.MQ.RocketMQ.Extions;
using Dcp.Net.MQ.RocketMQ.SDK;
using Geek.Net.MQ;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.RocketMQ
{
    /// <summary>
    ///     RocketMQ管理器（目前只支持64位）
    /// </summary>
    public class RocketMQManager : IRocketMQManager,IDisposable
    {
        static RocketMQManager()
        {
            Console.WriteLine("静态初始化方法执行");
            //这儿初始化动态库
            DynamicGlobSetting.InitDynamicLib();
        }
        /// <summary>
        ///     创建消息队列属性
        /// </summary>
        private readonly ONSFactoryProperty _factoryInfo;
        private static readonly object ObjLock = new object();

        /// <summary> RocketMQ管理器 </summary>
        public RocketMQManager(RocketMQItemConfig config)
        {
            DynamicGlobSetting.InitDynamicLib();
            _factoryInfo = new ONSFactoryProperty();
            if (config.AccessKey != null) _factoryInfo.setFactoryProperty(ONSFactoryProperty.AccessKey, config.AccessKey);
            if (config.SecretKey != null) _factoryInfo.setFactoryProperty(ONSFactoryProperty.SecretKey, config.SecretKey);
            if (config.ConsumerID != null) _factoryInfo.setFactoryProperty(ONSFactoryProperty.ConsumerId, config.ConsumerID);
            if (config.ProducerID != null) _factoryInfo.setFactoryProperty(ONSFactoryProperty.ProducerId, config.ProducerID);
            if (config.Topic != null) _factoryInfo.setFactoryProperty(ONSFactoryProperty.PublishTopics, config.Topic);
            if (config.Server != null)
            {
                _factoryInfo.setFactoryProperty(ONSFactoryProperty.NAMESRV_ADDR, config.Server);
                //_factoryInfo.setFactoryProperty(ONSFactoryProperty.ONSAddr, config.Server);
            }

            // 设置线程数
            if (config.ConsumeThreadNums < 1) config.ConsumeThreadNums = 1;
            _factoryInfo.setFactoryProperty(ONSFactoryProperty.ConsumeThreadNums, config.ConsumeThreadNums.ToString());

            // 默认值为ONSChannel.ALIYUN，聚石塔用户必须设置为CLOUD，阿里云用户不需要设置(如果设置，必须设置为ALIYUN)
            _factoryInfo.setOnsChannel(config.Channel);
            if (config.IsWriteLog) _factoryInfo.setFactoryProperty(ONSFactoryProperty.LogPath, SysPath.LogPath);
            _factoryInfo.setFactoryProperty(ONSFactoryProperty.SendMsgTimeoutMillis, "3000");
        }
        public async void StartAsync(Action<MQMessage> reciveAction)
        {
            await Task.Factory.StartNew(()=> {
                this.Product.Start();
                this.Consumer.StartListener(reciveAction);
            });
        }
        public  void Start(Action<MQMessage> reciveAction)
        {
            Task.Factory.StartNew(() => {
                this.Product.Start();
                this.Consumer.StartListener(reciveAction);
            }).Wait();
        }

        public void Dispose()
        {
            this.Consumer.Close();
            this.Product.Close();
        }

        /// <summary>
        ///     生产消息
        /// </summary>
        private IRocketMQProduct _product;
        /// <summary>
        ///     生产消息
        /// </summary>
        public IRocketMQProduct Product
        {
            get
            {
                if (_product != null) return _product;
                lock (ObjLock) { return _product ?? (_product = new RocketMQProduct(_factoryInfo)); }
            }
        }

        /// <summary>
        ///     生产消息
        /// </summary>
        private IRocketMQOrderProduct _orderProduct;
        /// <summary>
        ///     生产消息
        /// </summary>
        public IRocketMQOrderProduct OrderProduct
        {
            get
            {
                if (_orderProduct != null) return _orderProduct;
                lock (ObjLock) { return _orderProduct ?? (_orderProduct = new RocketMQOrderProduct(_factoryInfo)); }
            }
        }

        /// <summary>
        ///     生产消息
        /// </summary>
        private IRocketMQConsumer _pushConsumer;
        /// <summary>
        ///     订阅消费
        /// </summary>
        public IRocketMQConsumer Consumer
        {
            get
            {
                if (_pushConsumer != null) return _pushConsumer;
                lock (ObjLock) { return _pushConsumer ?? (_pushConsumer = new RocketMQPushConsumer(_factoryInfo)); }
            }
        }

        /// <summary>
        ///     生产消息
        /// </summary>
        private IRocketMQOrderConsumer _orderConsumer;
        /// <summary>
        ///     消费
        /// </summary>
        public IRocketMQOrderConsumer OrderConsumer
        {
            get
            {
                if (_orderConsumer != null) return _orderConsumer;
                lock (ObjLock) { return _orderConsumer ?? (_orderConsumer = new RocketMQOrderConsumer(_factoryInfo)); }
            }
        }
    }
}