﻿using System.Collections.Generic;
using System.Reflection;
using Dcp.Net.Configuration;
using Dcp.Net.DI;
using Dcp.Net.Modules;
using Dcp.Net.MQ.RocketMQ.Configuration;
using Dcp.Net.MQ.RocketMQ.SDK;

namespace Dcp.Net.MQ.RocketMQ
{
    /// <summary>
    ///     RocketMQ模块
    /// </summary>
    public class RocketMQModule : DcpModule
    {
        /// <inheritdoc />
        public override void PreInitialize()
        {
            // 如果Redis配置没有创建，则创建它
            var configResolver = IocManager.Resolve<IConfigResolver>();
            InitConfig(configResolver);
        }

        /// <inheritdoc />
        private void InitConfig(IConfigResolver configResolver)
        {
            var config = configResolver.RocketMQConfig();
            if (config == null || config.Items.Count == 0)
            {
                configResolver.Set(new RocketMQConfig {Items = new List<RocketMQItemConfig> {new RocketMQItemConfig {Name = "test", Server = "", AccessKey = "AccessKey", SecretKey = "SecretKey", ProducerID = "ProducerID", ConsumerID = "ConsumerID", Topic = "Topic", Channel = ONSChannel.CLOUD, ConsumeThreadNums = 16}}});
                configResolver.Save();
            }
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            //模块初始化，实现IOC信息的注册
            IocManager.Container.Install(new RocketMQInstaller());
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(), new ConventionalRegistrationConfig {InstallInstallers = false});
        }
    }
}