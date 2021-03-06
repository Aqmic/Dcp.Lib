﻿// ********************************************
// 作者：huajunsoft
// 时间：2017-03-28 9:51
// ********************************************

using Dcp.Net.MQ.RocketMQ.SDK;

namespace Dcp.Net.MQ.RocketMQ
{
    internal class RocketMQPushConsumer : IRocketMQConsumer
    {
        private readonly ONSFactoryProperty _factoryInfo;
        private PushConsumer _consumer;
        public RocketMQPushConsumer(ONSFactoryProperty factoryInfo) { _factoryInfo = factoryInfo; }

        /// <summary>
        ///     消费订阅
        /// </summary>
        /// <param name="listen">消息监听处理</param>
        /// <param name="subExpression">标签</param>
        public void Start(MessageListener listen, string subExpression = "*")
        {
            if (_consumer != null) throw new DcpException("当前已开启过该消费，无法重新开启，需先关闭上一次的消费（调用Close()）。");
            _consumer = ONSFactory.getInstance().createPushConsumer(_factoryInfo);
            _consumer.subscribe(_factoryInfo.getPublishTopics(), subExpression, listen);
            _consumer.start();
        }

        /// <summary>
        ///     关闭订阅消费
        /// </summary>
        public void Close()
        {
            _consumer?.shutdown();
            _consumer = null;
        }
    }
}