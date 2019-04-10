using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Security
{
    /// <summary>
    /// Wing2.0 项目发布 相关请求安全验证接口
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// 验证指定验证上下文是否符合安全验证准则
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        SecurityResult Check(object state);
    }
}
