using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp.Interface
{
    public interface IDataBucket
    {
        object GetValue(string key);
        T GetValue<T>(string key);
        bool AddValue(string key, object value);
        bool SaveValue(string key, object value);
        bool ReplaceValue(string key, object value);
        bool RemoveValue(string key);
    }
}
