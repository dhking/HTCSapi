using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// 项目发布 客户端信息
    /// </summary>
    public class Client
    {
        /// <summary>
        /// 客户公司id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///获取或者设置 客户WEB地址
        /// </summary>
        public string COMPANY_WEB { get; set; }

        /// <summary>
        ///获取或者设置 客户API地址
        /// </summary>
        public string COMPANY_API
        {
            get
            {
                return string.Format("api.{0}", COMPANY_WEB);
            }
        }

        /// <summary>
        ///获取或者设置 客户应用程序所在服务器 C盘磁盘唯一编号
        /// </summary>
        public string Disk { get; set; }

        /// <summary>
        /// 获取或者设置 客户应用程序 所在服务器 cpu唯一编号
        /// </summary>
        public string Cpu { get; set; }

        /// <summary>
        /// 获取或者设置客户名称
        /// </summary>
        public string COMPANY_NAME { get; set; }

        /// <summary>
        /// 获取或者设置 客户应用程序 发布根目录
        /// </summary>
        public string BaseDIR { get; set; }
    }
}
