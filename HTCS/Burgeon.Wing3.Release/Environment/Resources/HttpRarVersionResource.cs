using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Environment.Commands;

namespace Burgeon.Wing3.Release.Environment.Resources
{
    /// <summary>
    ///自动将指定文件或者目录打包成 版本资源文件
    ///并且可以将资源文件发送到客户版本进行升级
    /// </summary>
    public class HttpRarVersionResource : RarVersionResource
    {
        public HttpRarVersionResource(string fileOrDIR)
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
