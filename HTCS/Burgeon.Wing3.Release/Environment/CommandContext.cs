using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// Wing2.0 项目发布 执行命令上下文 
    /// </summary>
    public class CommandContext
    {
        //上下文参数列表
        private readonly IDictionary<string, object> _Params;

        public CommandContext()
        {
            _Params = new Dictionary<string, object>();
        }

        /// <summary>
        /// 获取上下文设置的参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (_Params.ContainsKey(name))
                {
                    return _Params[name];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (_Params.ContainsKey(name))
                {
                    _Params[name] = value;
                }
                else
                {
                    _Params.Add(name, value);
                }
            }
        }

        /// <summary>
        /// 获取上下文设置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T GetParam<T>(string name)
        {
            object v = this[name];
            if (v is T)
            {
                return (T)v;
            }
            else
            {
                return Utils.TypeGenericUtil.ConvertType<T>(v);
            }
        }
    }
}
