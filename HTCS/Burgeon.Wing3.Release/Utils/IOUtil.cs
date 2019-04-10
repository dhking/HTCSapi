using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 文件操作辅助类
    /// </summary>
    public class IOUtil
    {
        /// <summary>
        /// 去掉指定父级路径
        /// </summary>
        /// <param name="path">待替换的路径</param>
        /// <param name="baseDIR">要替换的父级路径</param>
        /// <returns></returns>
        public static string TrimBaseDIR(string path, string baseDIR)
        {
            if (path == null || baseDIR == null)
            {
                return null;
            }
            else
            {


                baseDIR = FormatPath(baseDIR);
                path = FormatPath(path);
                int ind = path.IndexOf(baseDIR);
                if (ind > -1)
                {
                    path = path.Substring(ind + baseDIR.Length);
                }
                return path;
            }
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        /// <summary>
        /// 获取指定路径的目录名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetNameOfDIR(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 将目录地址标准化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPath(string path)
        {
            return path.Replace("//", "\\").Replace("/", "\\");
        }

        /// <summary>
        /// 删除一个文件或者目录
        /// </summary>
        /// <param name="fileOrDir"></param>
        public static void Delete(string fileOrDir, bool recursive = true)
        {
            if (File.Exists(fileOrDir))
            {
                File.Delete(fileOrDir);
            }
            else if (Directory.Exists(fileOrDir))
            {
                Directory.Delete(fileOrDir, recursive);
            }
        }

        /// <summary>
        /// 渲染指定目录，如果指定目录不存在 则自动创建目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string AutoCreateDIR(string dir)
        {
            if (!ExistsDIR(dir))
            {
                CreateDIR(dir);
            }

            return dir;
        }

        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDIR(string dir)
        {
            System.IO.Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// 判断指定目录是否存在
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool ExistsDIR(string dir)
        {
            return System.IO.Directory.Exists(dir);
        }

        /// <summary>
        /// 判断指定文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool ExistsFile(string file)
        {
            return System.IO.File.Exists(file);
        }

        /// <summary>
        /// 合并指定目录
        /// </summary>
        /// <param name="dirs"></param>
        /// <returns></returns>
        public static string CombineDIR(params string[] dirs)
        {
            return System.IO.Path.Combine(dirs);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">要保存的文件</param>
        /// <param name="content">文件内容</param>
        public static void SaveFile(string file, string content, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            System.IO.File.WriteAllText(file, content, encoding);
        }

        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }
    }
}
