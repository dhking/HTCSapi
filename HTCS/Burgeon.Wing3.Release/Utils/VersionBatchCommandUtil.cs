using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 版本资源文件批处理工具命令生成
    /// </summary>
    public class VersionBatchCommandUtil
    {
        /// <summary>
        /// 获取指定文件xcopy的批处理命令 默认为 /c 错误继续 /y 存在直接覆盖
        /// </summary>
        /// <param name="file">源文件</param>
        /// <param name="destination">目标文件</param>
        /// <returns></returns>
        public static string XCopyFile(string file, string destination, string args = "/c /y")
        {
            return string.Format("echo f|xcopy \"{0}\" \"{1}\" {2}", file, destination, args);
        }

        /// <summary>
        /// 获取指定目录xcopy的批处理命令 默认为 /c 错误继续 /y 存在直接覆盖
        /// </summary>
        /// <param name="dir">源目录</param>
        /// <param name="destination">目标目录</param>
        /// <returns></returns>
        public static string XCopyDIR(string dir, string destination, string args = "/c /y")
        {
            return string.Format("echo d|xcopy \"{0}\" \"{1}\" {2}", dir, destination, args);
        }

        /// <summary>
        /// 获取指定目录或者文件xcopy的批处理复制命令 默认为 /c 错误继续 /y 存在直接覆盖
        /// </summary>
        /// <param name="fileOrDIR">源目录或者文件</param>
        /// <param name="destination">目标目录或者文件</param>
        /// <param name="args">默认为 /c 错误继续 /y 存在直接覆盖</param>
        /// <returns></returns>
        public static string XCopy(string fileOrDIR, string destination, string args = "/c /y")
        {
            if (IOUtil.ExistsDIR(fileOrDIR))
            {
                return XCopyDIR(fileOrDIR, destination, args);
            }
            else if (IOUtil.ExistsFile(fileOrDIR))
            {
                return XCopyFile(fileOrDIR, destination, args);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取cmd move移动指定文件到目标文件的批处理命令
        /// </summary>
        /// <param name="file">源文件</param>
        /// <param name="destination">目标文件</param>
        /// <returns></returns>
        public static string Move(string file, string destination)
        {
            return string.Format("move /y \"{0}\" \"{1}\"", file, destination);
        }

        /// <summary>
        /// 删除指定文件或者目录 cmd命令 
        /// </summary>
        /// <param name="fileOrDir">要删除的文件或者目录</param>
        /// <param name="args">默认为 /s 删除子文件与目录 /q 不提示是否要删除</param>
        /// <returns></returns>
        public static string Del(string fileOrDir, string args = " /s /q")
        {
            return string.Format("del \"{0}\" {1}", fileOrDir, args);
        }

        /// <summary>
        /// 获取删除指定目录的cmd命令
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string Rd(string dir)
        {
            return string.Format("rd \"{0}\" /s /q ", dir);
        }
    }
}
