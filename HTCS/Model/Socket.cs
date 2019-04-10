using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public   class Socket
    {
        public string UserId { get; set; }

        //1连接心跳2请求权限数据
        public string Type { get; set; }
        //0连接成功1连接失败
        public string Code { get; set; }

        public string Value { get; set; }
    }

}
