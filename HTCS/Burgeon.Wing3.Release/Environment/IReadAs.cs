using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    public interface IReadAs
    {
        /// <summary>
        /// 获取指定类型返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ReadAs<T>();
    }
}
