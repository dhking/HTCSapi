using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    public class AssemblyAttributeUtil
    {
        public static T GetAttributeFirsOfDefault<T>(System.Reflection.PropertyInfo p) where T : Attribute
        {
            object[] attrs = p.GetCustomAttributes(false);
            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0] as T;
            }
            else
            {
                return default(T);
            }
        }

        public static T GetAttributeFirsOfDefault<T>(Type type) where T : Attribute
        {
            object[] attrs = type.GetCustomAttributes(false);
            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0] as T;
            }
            else
            {
                return default(T);
            }
        }
    }
}
