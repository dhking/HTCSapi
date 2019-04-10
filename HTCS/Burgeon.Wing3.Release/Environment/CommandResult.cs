using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// Wing2.0 项目发布 命令执行结果
    /// </summary>
    public class CommandResult : IReadAs
    {
        private string code = "0";

        public CommandResult(ResultStatus status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        public CommandResult(ResultStatus status, string message, object rows)
        {
            this.Status = status;
            this.Message = message;
            this.Rows = rows;
        }

        public CommandResult(ResultStatus status, Exception ex)
        {
            this.Status = status;
            if (ex != null)
            {
                this.Message = ex.Message;
                this.SubMessage = ex.ToString();
            }
        }

        public CommandResult() { }

        public string Code
        {
            get { return this.code; }
            set
            {
                this.code = value;
                if (this.code != "0")
                {
                    this.Status = ResultStatus.Error;
                }
                else
                {
                    this.Status = ResultStatus.Success;
                }
            }
        }

        public ResultStatus Status { get; set; }

        public object Rows { get; set; }

        public string Message { get; set; }

        public string SubMessage { get; set; }

        public T ReadAs<T>()
        {
            if (this.Rows is T)
            {
                return (T)this.Rows;
            }
            else
            {
                return default(T);
            }
        }
    }
}
