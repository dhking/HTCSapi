using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    public class AssemblyPropertyUtil
    {
        public static T GetValueOf<T>(System.Reflection.PropertyInfo p, object instance)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            
            object v = p.GetValue(instance, null);
            
            return Utils.TypeGenericUtil.ConvertType<T>(v);
        }
    }
}
