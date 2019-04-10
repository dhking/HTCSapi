using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.CommonControllers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControllerFixAttribute : Attribute, IRoutePrefix
    {
        const string PRE_STR = "rcs/";

        public virtual string Prefix
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        protected ControllerFixAttribute()
        {

        }

        public ControllerFixAttribute(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }
            this.Prefix = PRE_STR + prefix;
        }
    }
    //
    // 摘要:
    //     定义路由前缀。
    public interface IRoutePrefix
    {
        //
        // 摘要:
        //     获取路由前缀。
        //
        // 返回结果:
        //     路由前缀。
        string Prefix { get; }
    }
}
