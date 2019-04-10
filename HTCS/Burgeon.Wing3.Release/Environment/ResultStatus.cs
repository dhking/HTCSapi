using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// 命令指定完毕后的状态
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 执行出现异常
        /// </summary>
        Error = 1,
        /// <summary>
        /// 执行前 没有通过验证
        /// </summary>
        InValid = 2,
        /// <summary>
        /// 没有授权
        /// </summary>
        NoAurthrize = 3,
        /// <summary>
        /// 执行出现未知
        /// </summary>
        Unknow = 3
    }
}
