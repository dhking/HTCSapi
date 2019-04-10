using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PushModel
    {
        //必填 推送设备指定
        public string platform { get; set; }
        public string audience { get; set; }
        public string notification { get; set; }// 可选  通知内容体。是被推送到客户端的内容。与 message 一起二者必须有其一，可以二者并存
        public string message { get; set; } //可选 消息内容体。是被推送到客户端的内容。与 notification 一起二者必须有其一，可以二者并存
        public string sms_message { get; set; }//可选 短信渠道补充送达内容体
        public string options { get; set; } //可选 推送参数
        public string cid { get; set; } //可选 用于防止 api 调用端重试造成服务端的重复推送而定义的一个标识符。
    }

    public class ParamPhsh
    {
        public string Content { get; set; }

        public string Url { get; set; }

        public string Alias { get; set; }

        public string Mobile { get; set; }

        public string deviceid { get; set; }

       
    }
    public class PushResult
    {
        public string msg_id { get; set; }

        public error error { get; set; }

      
    }
    public class error
    {
        public int code { get; set; }

        public string message { get; set; }
    }
}