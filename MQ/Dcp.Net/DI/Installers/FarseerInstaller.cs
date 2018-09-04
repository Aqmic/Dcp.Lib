using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Dcp.Net.Configuration;
using Dcp.Net.Configuration.Startup;
using Dcp.Net.Modules;
using Dcp.Net.Reflection;

namespace Dcp.Net.DI.Installers
{
    /// <summary>
    ///     系统核心组件注册类
    /// </summary>
    public class DcpInstaller : IWindsorInstaller
    {
        /// <summary>
        ///     注册
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // 注册核心组件到依赖注入容器中，包括配置。
            if (!IocManager.Instance.IsRegistered<IModuleConfigurations>()) { container.Register(Component.For<IModuleConfigurations, ModuleConfigurations>().ImplementedBy<ModuleConfigurations>().LifestyleSingleton()); }
            if (!IocManager.Instance.IsRegistered<IDcpStartupConfiguration>()) { container.Register(Component.For<IDcpStartupConfiguration, DcpStartupConfiguration>().ImplementedBy<DcpStartupConfiguration>().LifestyleSingleton()); }
            if (!IocManager.Instance.IsRegistered<ITypeFinder>()) { container.Register(Component.For<ITypeFinder, TypeFinder>().ImplementedBy<TypeFinder>().LifestyleSingleton()); }
            if (!IocManager.Instance.IsRegistered<IDcpModuleManager>()) { container.Register(Component.For<IDcpModuleManager, DcpModuleManager>().ImplementedBy<DcpModuleManager>().LifestyleSingleton()); }
            if (!IocManager.Instance.IsRegistered<IAssemblyFinder>()) { container.Register(Component.For<IAssemblyFinder, AssemblyFinder>().ImplementedBy<AssemblyFinder>().LifestyleSingleton()); }
        }
    }
}