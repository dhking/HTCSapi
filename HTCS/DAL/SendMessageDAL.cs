using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Model.Base;
using Model.Bill;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class SendMessageDAL
    {
        //查询短信是否充足
        public bool querymessage(long companyid)
        {
            if (companyid == 0)
            {
                return true;
            }
            BaseDataDALL bdal = new DAL.BaseDataDALL();
            T_account account= bdal.queryaccount(companyid);
            if (account != null)
            {
                if (account.smsnumber > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //减少消耗品
        public void reduce(long companyid)
        {
            if (companyid != 0)
            {
                ProceDAL dal = new DAL.ProceDAL();
                dal.Cmdproce10(new Model.Pure() { Spname = "sp_reducenumber", Other = "1", Ids = companyid.ToStr() });
            }
        }
        public bool SendMessage(yzRequest req, out string errmsg)
        {
            errmsg = "";
            bool result = false;
            if (querymessage(req.CompanyId) == false)
            {
                errmsg = "短信数量不足";
                result = true;
                return result;
            }
            string setting= ConfigurationManager.AppSettings["messagekey"];
            if (setting == null)
            {
                errmsg = "短信配置错误";
                return result;
            }
            string[] arr = setting.Trim().Split('|');
            string aaccessKeyId = arr[0];
            string aaccessKeySecret = arr[1];

            //return true;
            String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            String accessKeyId = aaccessKeyId;//你的accessKeyId，参考本文档步骤2
            String accessKeySecret = aaccessKeySecret;//你的accessKeySecret，参考本文档步骤2
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                request.PhoneNumbers = req.Phone.ToStr();
                //必填:短信签名-可在短信控制台中找到
                request.SignName = arr[2];
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode =req.TemplateCode ;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = req.Temp;
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if (sendSmsResponse.Code == "OK")
                {
                    reduce(req.CompanyId);
                    result = true;
                }
                else
                {
                    result = false;
                    errmsg = sendSmsResponse.Message;
                }

            }
            catch (ServerException e)
            {
                result = false;
                errmsg = e.ToString();
            }
            catch (ClientException e)
            {
                result = false;
                errmsg = e.ToString();
            }
            return result;
        }

    }
}
