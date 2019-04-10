using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Models
{
    /// <summary>
    /// 项目发布 创建项目版本信息
    /// </summary>
    public class LocalVersion
    {
        /// <summary>
        /// 获取或者设置当前版本根目录
        /// </summary>
        public string VersionDIR { get; set; }

        /// <summary>
        /// 获取或者设置当前版本要生成的部分版本路径
        /// </summary>
        public string PartVersionDIR { get; set; }

        /// <summary>
        /// 获取版本库顶级目录
        /// </summary>
        public string TopDIR
        {
            get
            {
                return Environment.ResourceEnvironment.Environment.TopVersionDIR;
            }
        }

        /// <summary>
        /// 获取部分版本存放目录
        /// </summary>
        public string TopPartVersionDIR
        {
            get
            {
                return Environment.ResourceEnvironment.Environment.TopBuilVerionDIR;
            }
        }
    }
}
