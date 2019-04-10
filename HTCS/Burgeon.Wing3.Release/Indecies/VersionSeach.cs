using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Indecies
{
    /// <summary>
    /// Wing 2.0 版本检索类
    /// </summary>
    public class VersionSeach
    {
        private readonly string baseDIR;

        private static VersionSeach Default = null;

        /// <summary>
        /// 使用Appsetting配置版本库目录路径创建一个版本检索器
        /// </summary>
        public static VersionSeach Create()
        {
            if (Default == null)
            {
                Default = new VersionSeach();
            }
            return Default;
        }

        /// <summary>
        /// 使用Appsetting配置版本库目录路径创建一个版本检索器
        /// </summary>
        public VersionSeach()
        {
            baseDIR = Utils.ConfigurationUtil.GetAppsetting("TopDIR");
        }

        /// <summary>
        /// 创建一个版本检索类 
        /// </summary>
        /// <param name="baseDir">当前检索版本库根目录</param>
        public VersionSeach(string baseDir)
        {
            if (string.IsNullOrWhiteSpace(baseDir))
            {
                throw new ArgumentNullException("baseDir");
            }

            if (!Utils.IOUtil.ExistsDIR(baseDir))
            {
                throw new ArgumentException(string.Format("baseDir({0})路径不存在", baseDir));
            }

            this.baseDIR = baseDir;
        }

        /// <summary>
        /// 获取版本库目录下所有版本根目录路径
        /// </summary>
        /// <returns></returns>
        public List<Models.ResourceMapping> GetVersions()
        {
            return Utils.FileSeachUtil.ConvertToMapping(Utils.FileSeachUtil.Seach(baseDIR, Utils.ConfigurationUtil.GetAppsetting("VERSIONFILTER", "V*")), true, baseDIR);
        }

        /// <summary>
        /// 根据指定资源目录 检索指定目录下的版本资源文件与文件夹
        /// </summary>
        /// <param name="parent">检索目录</param>
        /// <param name="filter">检索条件 默认*.*</param>
        /// <returns></returns>
        public List<Models.ResourceMapping> SeachResource(string parent, string filter = "*.*")
        {
            if (string.IsNullOrWhiteSpace(parent))
            {
                return GetVersions();
            }
            if (parent.IndexOf(":") < 0)
            {
                parent = Utils.IOUtil.CombineDIR(baseDIR, parent.TrimStart('\\'));
            }
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = "*.*";
            }
            return Utils.FileSeachUtil.ConvertToMapping(Utils.FileSeachUtil.SeachOnlyChild(parent, filter), false, baseDIR);
        }
    }
}
