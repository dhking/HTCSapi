using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Burgeon.Wing3.Release.Utils;

namespace Burgeon.Wing3.Release.Security
{
    /// <summary>
    /// Wing2.0 项目发布 版本文件传输安全校验器
    /// </summary>
    public class VersionResourceSecurity : ISecurity
    {
        public SecurityResult Check(object state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            SecurityResult cResult = null;
            VersionResourceSecurityContext context = state as VersionResourceSecurityContext;
            if (context != null)
            {
                cResult = CheckResourceHack(context.FileName, context.Key);
            }
            else
            {
                cResult = new SecurityResult(false, "在校验版本文件信息失败,原因：没有上下文");

            }
            return cResult;
        }

        private SecurityResult CheckResourceHack(string file, string key)
        {
            if (!File.Exists(file))
            {
                return new SecurityResult(false, string.Format("在校验版本文件信息时出现异常, 文件<<{0}>>,不存在", file));
            }

            string cpu = ManagementUtil.GetCpuIdentifier();
            string nKey = Encrypt(cpu, file);

            if (string.Equals(nKey, key))
            {
                return new SecurityResult(true, string.Format("版本文件<<{0}>>校验通过", file));
            }
            else
            {
                return new SecurityResult(false, string.Format("版本文件<<{0}>>校验失败,原因:文件密码不匹配", file));
            }
        }

        /// <summary>
        /// 通过发送文件 生成访问key
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string Encrypt(string cpu, string filePath)
        {
            return EncryptUtil.Md5(cpu + File.ReadAllText(filePath));
        }
    }
}
