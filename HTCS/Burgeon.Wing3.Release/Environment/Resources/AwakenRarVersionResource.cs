using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Environment.Commands;

namespace Burgeon.Wing3.Release.Environment.Resources
{
    /// <summary>
    /// 唤醒方式版本压缩资源文件
    /// </summary>
    public class AwakenRarVersionResource : RarVersionResource
    {
        /// <summary>
        /// 自动将指定文件或者目录打包成 版本资源文件 并且可以讲版本信息发送到
        /// 客户版本 客户版本自主下载版本 进行升级
        /// </summary>
        /// <param name="fileOrDIR"></param>
        public AwakenRarVersionResource(string fileOrDIR)
            : base(fileOrDIR)
        {

        }

        public override ICommand SendCommandHandler
        {
            get
            {
                return new AwakenSendResourceCommand();
            }
        }

        public override ICommand ReceiveCommandHandler
        {
            get
            {
                return new AwakenReceiveResourceCommand();
            }
        }
    }
}
