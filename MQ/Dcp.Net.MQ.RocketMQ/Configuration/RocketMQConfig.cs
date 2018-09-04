using System.Collections.Generic;
using Dcp.Net.Configuration;

namespace Dcp.Net.MQ.RocketMQ.Configuration
{
    /// <summary>
    ///     RocketMQ配置,支持多个集群配置
    /// </summary>
    public class RocketMQConfig : IDcpConfig
    {
        /// <summary>
        ///     多个配置集合
        /// </summary>
        public List<RocketMQItemConfig> Items { get; set; } = new List<RocketMQItemConfig>();
    }
}