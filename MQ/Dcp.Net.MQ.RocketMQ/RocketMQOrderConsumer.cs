// ********************************************
// 作者：huajunsoft
// 时间：2017-03-28 9:51
// ********************************************

using Dcp.Net.MQ.RocketMQ.SDK;

namespace Dcp.Net.MQ.RocketMQ
{
    internal class RocketMQOrderConsumer : IRocketMQOrderConsumer
    {
        private readonly ONSFactoryProperty _factoryInfo;
        private OrderConsumer _consumer;
        public RocketMQOrderConsumer(ONSFactoryProperty factoryInfo) { _factoryInfo = factoryInfo; }

        /// <summary>
        ///     消费订阅
        /// </summary>
        /// <param name="listen">消息监听处理</param>
        /// <param name="subExpression">标签</param>
        public void Start(MessageOrderListener listen, string subExpression = "*")
        {
            _consumer = ONSFactory.getInstance().createOrderConsumer(_factoryInfo);
            _consumer.subscribe(_factoryInfo.getPublishTopics(), subExpression, listen);
            _consumer.start();
        }

        /// <summary>
        ///     关闭消费
        /// </summary>
        public void Close() => _consumer.shutdown();
    }
}