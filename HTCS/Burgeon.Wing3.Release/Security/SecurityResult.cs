using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Environment;

namespace Burgeon.Wing3.Release.Security
{
    /// <summary>
    /// Wing2.0 项目发布 相关请求安全验证结果
    /// </summary>
    public class SecurityResult
    {
        /// <summary>
        /// 创建一个 安全验证结果
        /// </summary>
        /// <param name="isGreen"></param>
        /// <param name="message"></param>
        public SecurityResult(bool isGreen, string message)
        {
            this.IsGreen = isGreen;
            this.Message = message;
        }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        public bool IsGreen { get; private set; }

        /// <summary>
        /// 验证反馈消息
        /// </summary>
        public string Message { get; private set; }
    }
}
