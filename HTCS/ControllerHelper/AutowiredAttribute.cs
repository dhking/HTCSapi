using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerHelper
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class AutowiredAttribute : Attribute
    {

        public string FullPathName { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public AutowiredAttribute()
        {

        }
    }
}
