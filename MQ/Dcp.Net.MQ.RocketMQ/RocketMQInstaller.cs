using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Dcp.Net.Configuration;
using Dcp.Net.MQ.RocketMQ.Configuration;

namespace Dcp.Net.MQ.RocketMQ
{
    /// <summary>
    ///     RocketMQ的IOC注册
    /// </summary>
    public class RocketMQInstaller : IWindsorInstaller
    {
        /// <inheritdoc />
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var localConfigResolver = container.Resolve<IConfigResolver>();
            if (localConfigResolver.RocketMQConfig().Items.Count == 0) return;

            //注册所有的消息队列的Topic消费者
            localConfigResolver.RocketMQConfig().Items.ForEach(c => container.Register(Component.For<IRocketMQManager>().Named(c.Name).ImplementedBy<RocketMQManager>().DependsOn(Dependency.OnValue<RocketMQItemConfig>(c)).LifestyleSingleton()));
        }
    }
}