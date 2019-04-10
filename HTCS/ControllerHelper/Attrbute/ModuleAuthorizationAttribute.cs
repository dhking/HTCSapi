using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerHelper.Attrbute
{
    /// <summary>
    /// 权限控制标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ModuleAuthorizationAttribute : Attribute
    {
        public ModuleAuthorizationAttribute(params string[] authorization)
        {
            this.Authorizations = authorization;
        }

        /// <summary>
        /// 允许访问角色
        /// </summary>
        public string[] Authorizations { get; set; }
    }
}
