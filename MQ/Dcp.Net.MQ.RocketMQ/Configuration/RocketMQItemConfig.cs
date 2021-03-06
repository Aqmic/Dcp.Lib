﻿using Dcp.Net.MQ.RocketMQ.SDK;

namespace Dcp.Net.MQ.RocketMQ.Configuration
{
    /// <summary>
    ///     RocketMQ配置信息
    /// </summary>
    public class RocketMQItemConfig
    {
        /// <summary> 配置名称 </summary>
        public string Name { get; set; }
        /// <summary> 鉴权用AccessKey </summary>
        public string AccessKey { get; set; }
        /// <summary> 鉴权用SecretKey </summary>
        public string SecretKey { get; set; }
        /// <summary> 消费者ID </summary>
        public string ConsumerID { get; set; }
        /// <summary> 生产者ID </summary>
        public string ProducerID { get; set; }
        /// <summary> 消息队列主题名称 </summary>
        public string Topic { get; set; }
        /// <summary> 集群地址,多个地址用逗号隔开 </summary>
        public string Server { get; set; }
        /// <summary> 聚石塔用户必须设置为CLOUD，阿里云用户不需要设置 </summary>
        public ONSChannel Channel { get; set; }
        /// <summary> 消费端的线程数 </summary>
        public int ConsumeThreadNums { get; set; }
        /// <summary> 是否输出日志 </summary>
        public bool IsWriteLog { get; set; }
    }
}