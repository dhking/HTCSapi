using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    public class TypeGenericUtil
    {
        public static T ConvertType<T>(object v)
        {
            if (v == null)
            {
                return default(T);
            }
            else
            {
                return (T)Convert.ChangeType(v, typeof(T));
            }
        }
    }
}
