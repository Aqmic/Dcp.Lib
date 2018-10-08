namespace Geek.Net.MQ.Config
{
    using System;

    public enum MessageSendType
    {
        /// <summary>
        /// 简单模式（直接发送到quque）
        /// </summary>
        Simple,
        /// <summary>
        /// 能者多劳模式（直接发送到quque）
        /// </summary>
        Worker,
        /// <summary>
        /// 发布订阅模式（直接发送到exchange）
        /// </summary>
        PublishOrder,
        /// <summary>
        /// 路由键模式（直接发送到exchange）
        /// </summary>
        Router,
        /// <summary>
        /// 路由建模糊匹配模式（直接发送到exchange）
        /// </summary>
        TopicLike
    }
}

