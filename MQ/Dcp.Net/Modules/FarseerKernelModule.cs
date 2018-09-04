using System.Net;
using System.Reflection;
using Dcp.Net.Configuration.Startup;
using Dcp.Net.DI;

namespace Dcp.Net.Modules
{
    /// <summary>
    ///     系统核心模块
    /// </summary>
    public sealed class DcpKernelModule : DcpModule
    {
        /// <summary>
        ///     初始化之前
        /// </summary>
        public override void PreInitialize()
        {
            // 如果Redis配置没有创建，则创建它
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());
            //todo:SystemConfigBuilder.LoadConfig();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public override void Initialize()
        {
            foreach (var replaceAction in ((DcpStartupConfiguration)Configuration).ServiceReplaceActions.Values) { replaceAction(); }
#if CORE
			IocManager.RegisterAssemblyByConvention(typeof(DcpKernelModule).GetTypeInfo().Assembly, new ConventionalRegistrationConfig { InstallInstallers = false });
#else
			IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(), new ConventionalRegistrationConfig { InstallInstallers = false });
#endif
		}

        public override void PostInitialize()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
        }
    }
}