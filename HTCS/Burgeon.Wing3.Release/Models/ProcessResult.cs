using System;
using System.Collections.Generic;
using System.Text;

namespace Burgeon.Wing3.Release.Models
{
    /// <summary>
    /// 执行进程完毕的返回结果
    /// </summary>
    public class ProcessResult
    {
        public ProcessResult()
        {

        }

        public ProcessResult(string output)
        {
            this.Output = output;
        }

        public ProcessResult(Exception ex)
        {
            this.Exception = ex;
        }

        public ProcessResult(string output, Exception ex)
        {
            this.Exception = ex;
            this.Output = output;
        }

        /// <summary>
        /// 获取当前执行进程是否存在异常
        /// </summary>
        public bool IsError
        {
            get
            {
                return Exception != null;
            }
        }

        /// <summary>
        /// 获取或者设置当前进程异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 获取或者设置当前进程执行完毕后的反馈信息
        /// </summary>
        public string Output { get; set; }

        public override string ToString()
        {
            if (this.IsError)
            {
                return string.Format("{0}\r\n\r\n{1}", this.Output ?? "", this.Exception.ToString());
            }
            else
            {
                return this.Output;
            }
        }
    }
}
