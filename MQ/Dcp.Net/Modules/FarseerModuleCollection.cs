using System.Collections.Generic;
using System.Linq;

namespace Dcp.Net.Modules
{
    /// <summary>
    ///     模块集合
    /// </summary>
    internal class DcpModuleCollection : List<DcpModuleInfo>
    {
        /// <summary>
        ///     获取模块实例
        /// </summary>
        /// <typeparam name="TModule">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public TModule Get<TModule>() where TModule : DcpModule
        {
            var module = this.FirstOrDefault(m => m.Type == typeof(TModule));
            Check.NotNull(module, $"无法找到名为{typeof(TModule).FullName}的模块");
            return (TModule)module.Instance;
        }

        /// <summary>
        ///     根据依赖关系排序模块
        /// </summary>
        public List<DcpModuleInfo> GetListSortDependency()
        {
            var sortedModules = SortByDependencies();
            EnsureKernelModuleToBeFirst(sortedModules);
            return sortedModules;
        }

        /// <summary>
        ///     确认DcpKernelModule模块在模块集合中第一位置
        /// </summary>
        /// <param name="modules"></param>
        public static void EnsureKernelModuleToBeFirst(List<DcpModuleInfo> modules)
        {
            var kernelModuleIndex = modules.FindIndex(m => m.Type == typeof(DcpKernelModule));
            if (kernelModuleIndex > 0)
            {
                var kernelModule = modules[kernelModuleIndex];
                modules.RemoveAt(kernelModuleIndex);
                modules.Insert(0, kernelModule);
            }
        }

        /// <summary>
        ///     列表排序
        /// </summary>
        private List<DcpModuleInfo> SortByDependencies()
        {
            var sorted = new List<DcpModuleInfo>();
            var visited = new Dictionary<DcpModuleInfo, bool>();

            foreach (var item in this) SortByDependenciesVisit(item, sorted, visited);

            return sorted;
        }

        /// <summary>
        ///     根据依赖访问器排序列表
        /// </summary>
        /// <param name="item">元素</param>
        /// <param name="sorted">排序后的列表</param>
        /// <param name="visited">已经访问过的元素字典</param>
        private static void SortByDependenciesVisit(DcpModuleInfo item, ICollection<DcpModuleInfo> sorted, Dictionary<DcpModuleInfo, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited) { Check.IsTure(inProcess, "发现循环依赖！"); }
            else
            {
                visited[item] = true;

                if (item.Dependencies != null) foreach (var dependency in item.Dependencies) SortByDependenciesVisit(dependency, sorted, visited);

                visited[item] = false;
                sorted.Add(item);
            }
        }
    }
}