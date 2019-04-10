using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Models;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 文件搜索辅助工具
    /// </summary>
    public class FileSeachUtil
    {
        /// <summary>
        /// 搜索指定目录下的相关文件或者目录
        /// </summary>
        /// <param name="dir">待搜索的目录绝对路径</param>
        /// <param name="filter">搜索过滤条件 默认为 *.*</param>
        /// <param name="isSeachChild">是否搜索子目录 鉴于性能考虑 不推荐使用搜索子目录</param>
        /// <returns></returns>
        public static string[] Seach(string dir, string filter = "*.*", bool isSeachChild = false)
        {
            if (isSeachChild)
            {
                return SeachAll(dir, filter);
            }
            else
            {
                return SeachOnlyChild(dir, filter);
            }
        }

        /// <summary>
        /// 搜索指定目录下所有子目录以及目录文件
        /// </summary>
        /// <param name="filter">搜索过滤条件 默认为 *.*</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static string[] SeachAll(string dir, string filter = "*.*")
        {
            if (Utils.IOUtil.ExistsDIR(dir))
            {
                return System.IO.Directory.GetFileSystemEntries(dir, filter, System.IO.SearchOption.AllDirectories);
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// 搜索指定目录下目录以及文件
        /// </summary>
        /// <param name="filter">搜索过滤条件 默认为 *.*</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static string[] SeachOnlyChild(string dir, string filter = "*.*")
        {
            if (Utils.IOUtil.ExistsDIR(dir))
            {
                return System.IO.Directory.GetFileSystemEntries(dir, filter, System.IO.SearchOption.TopDirectoryOnly);
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// 将指定的文件或者目录列表转换成资源映射结构列表
        /// </summary>
        /// <param name="resources">文件或者目录列表</param>
        /// <returns></returns>
        public static List<Models.ResourceMapping> ConvertToMapping(string[] resources, bool isTop = false, string baseDIR = "")
        {
            List<Models.ResourceMapping> mappings = new List<Models.ResourceMapping>();
            Models.ResourceMapping mapping = null;

            if (resources != null)
            {
                foreach (string resource in resources)
                {
                    if (string.Equals(Utils.IOUtil.GetFileName(resource), "Web.config",StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    mapping = ConvertToMapping(resource, isTop, baseDIR);
                    if (mapping != null)
                    {
                        mappings.Add(mapping);
                    }
                }
            }

            return mappings.OrderByDescending<Models.ResourceMapping, ResourceType>(e => e.ResourceType).ToList();
        }

        /// <summary>
        /// 将制定的文件或者目录转换成映射结构
        /// </summary>
        /// <param name="resource">文件或者目录</param>
        /// <returns></returns>
        public static Models.ResourceMapping ConvertToMapping(string resource, bool isTop, string baseDIR)
        {
            Models.ResourceMapping mapping = null;

            if (Utils.IOUtil.ExistsDIR(resource))
            {
                mapping = new Models.ResourceMapping();
                DirectoryInfo info = new DirectoryInfo(resource);
                mapping.FullPath = info.FullName.Replace(baseDIR, "");
                mapping.Name = info.Name;
                mapping.Extension = "folder";
                mapping.Size = 0;
                mapping.Date = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                mapping.ResourceType = Models.ResourceType.Directory;
                if (!isTop)
                {
                    mapping.Parent = info.Parent.FullName.Replace(baseDIR, "");
                }
            }
            else if (Utils.IOUtil.ExistsFile(resource))
            {
                mapping = new Models.ResourceMapping();
                FileInfo info = new FileInfo(resource);
                mapping.FullPath = info.FullName.Replace(baseDIR, "");
                mapping.Name = info.Name;
                mapping.Extension = info.Extension.ToLower().Replace(".", "");
                mapping.Date = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                mapping.ResourceType = Models.ResourceType.File;
                mapping.Size = info.Length;
            }

            return mapping;
        }
    }
}
