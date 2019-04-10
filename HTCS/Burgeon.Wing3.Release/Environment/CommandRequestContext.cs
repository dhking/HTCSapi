using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    public class CommandRequestContext : CommandContext
    {
        private System.Web.HttpRequestBase Request = null;

        public CommandRequestContext(System.Web.HttpRequestBase request)
        {
            Request = request;
        }

        public override T GetParam<T>(string name)
        {
            string v = Request[name] ?? "";
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return Utils.TypeGenericUtil.ConvertType<T>(v);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(v);
            }
        }
    }
}
