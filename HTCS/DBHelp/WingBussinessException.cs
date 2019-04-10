using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    [Serializable]
    public class WingBussinessException : Exception
    {
        public WingBussinessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 淘宝接口异常
        /// </summary>
        /// <param name="message">接口名称</param>
        /// <param name="errorCode">错误</param>
        /// <param name="ErrorMessage">错误信息</param>
        public WingBussinessException(string message, string errorCode, string ErrorMessage)
            : base(string.Format("获取{2}接口异常!错误编码:{0},错误信息:{1}", errorCode, ErrorMessage, message))
        {


        }
    }
}
