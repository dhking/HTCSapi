using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerHelper
{
    public class IsExitException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public IsExitException() : base()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public IsExitException(string message) : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public IsExitException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
