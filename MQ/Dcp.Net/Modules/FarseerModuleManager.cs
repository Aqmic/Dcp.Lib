using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Logging;
using Dcp.Net.Configuration.Startup;
using Dcp.Net.DI;

namespace Dcp.Net.Modules
{
    /// <summary>
    ///     模块管理器
    /// </summary>
    public class DcpModuleManager : IDcpModuleManager
    {
        /// <summary>
        ///     依赖注入管理器
        /// </summary>
        private readonly IIocManager _iocManager;

        /// <summary>
        ///     模块集合
        /// </summary>
        private readonly DcpModuleCollection _moduleCollection;

        /// <summary>
        ///     启动模块类型
        /// </summary>
        private Type _startupModuleType;

        /// <summary>
        ///     日志
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="iocManager"></param>
        public DcpModuleManager(IIocManager iocManager)
        {
            _moduleCollection = new DcpModuleCollection();
            _iocManager = iocManager;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        ///     模块信息
        /// </summary>
        public DcpModuleInfo StartupModule { get; private set; }

        /// <summary>
        ///     模块列表
        /// </summary>
        public IList<DcpModuleInfo> Modules => _moduleCollection.ToList();

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="startupModule"></param>
        public virtual void Initialize(Type startupModule)
        {
            _startupModuleType = startupModule;
            LoadAllModules();
        }

        /// <summary>
        ///     启动模块
        /// </summary>
        public virtual void StartModules()
        {
            Logger.Debug("开始启动模块...");

            var sortedModules = _moduleCollection.GetListSortDependency();
            sortedModules.ForEach(module => module.Instance.PreInitialize());
            sortedModules.ForEach(module => module.Instance.Initialize());
            sortedModules.ForEach(module => module.Instance.PostInitialize());

            Logger.Debug("模块已成功启动...");
        }

        /// <summary>
        ///     关闭模块
        /// </summary>
        public virtual void ShutdownModules()
        {
            Logger.Debug("开始关闭模块...");

            var sortedModules = _moduleCollection.GetListSortDependency();
            sortedModules.Reverse();
            sortedModules.ForEach(sm => sm.Instance.Shutdown());

            Logger.Debug("模块已关闭...");
        }

        /// <summary>
        ///     加载所有模块
        /// </summary>
        private void LoadAllModules()
        {
            Logger.Debug("正在加载模块...");

            var moduleTypes = FindAllModules();

            Logger.Debug("总共找到 " + moduleTypes.Count + " 个模块");

            RegisterModules(moduleTypes);
            CreateModules(moduleTypes);

            DcpModuleCollection.EnsureKernelModuleToBeFirst(_moduleCollection);

            SetDependencies();

            Logger.DebugFormat("{0} 个模块已经加载", _moduleCollection.Count);
        }

        /// <summary>
        ///     查找所有模块
        /// </summary>
        /// <returns></returns>
        private List<Type> FindAllModules()
        {
            var modules = DcpModule.FindDependedModuleTypesRecursively(_startupModuleType);

            return modules;
        }

        /// <summary>
        ///     创建模块
        /// </summary>
        /// <param name="moduleTypes"></param>
        private void CreateModules(ICollection<Type> moduleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                var moduleObject = _iocManager.Resolve(moduleType) as DcpModule;
                Check.NotNull<DcpModule, DcpInitException>(moduleObject, $"此类型不是一个有效的模块: {moduleType.AssemblyQualifiedName}");

                moduleObject.IocManager = _iocManager;
                moduleObject.Configuration = _iocManager.Resolve<IDcpStartupConfiguration>();

                var moduleInfo = new DcpModuleInfo(moduleType, moduleObject);

                _moduleCollection.Add(moduleInfo);

                if (moduleType == _startupModuleType) StartupModule = moduleInfo;

                Logger.DebugFormat("已经加载模块: " + moduleType.AssemblyQualifiedName);
            }
        }

        /// <summary>
        ///     注册模块
        /// </summary>
        /// <param name="moduleTypes"></param>
        private void RegisterModules(ICollection<Type> moduleTypes)
        {
            foreach (var moduleType in moduleTypes) _iocManager.RegisterIfNot(moduleType);
        }

        /// <summary>
        ///     设置依赖
        /// </summary>
        private void SetDependencies()
        {
            foreach (var moduleInfo in _moduleCollection)
            {
                moduleInfo.Dependencies.Clear();

                foreach (var dependedModuleType in DcpModule.FindDependedModuleTypes(moduleInfo.Type))
                {
                    var dependedModuleInfo = _moduleCollection.FirstOrDefault(m => m.Type == dependedModuleType);
                    if (dependedModuleInfo == null) throw new DcpInitException(moduleInfo.Type.AssemblyQualifiedName + "没有找到依赖的模块 " + dependedModuleType.AssemblyQualifiedName);

                    if (moduleInfo.Dependencies.FirstOrDefault(dm => dm.Type == dependedModuleType) == null) moduleInfo.Dependencies.Add(dependedModuleInfo);
                }
            }
        }
    }
}