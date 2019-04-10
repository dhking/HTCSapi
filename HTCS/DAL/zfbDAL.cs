using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class zfbDAL
    {
        
        //支付宝转账
        public AlipayFundTransToaccountTransferResponse Zfbzz(T_PayMentAcount model,string content)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", model.app_id, model.private_key, "json", "1.0", "RSA2", model.public_key_zf, "GBK", false);
            AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();
            request.BizContent = content;
            AlipayFundTransToaccountTransferResponse response = client.Execute(request);
            Console.WriteLine(response.Body);
            return response;
        }
        //支付宝授权
        public AlipaySystemOauthTokenResponse oauthtoken(T_PayMentAcount model)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", model.app_id, model.private_key, "json", "1.0", "RSA2", model.public_key_zf, "GBK", false);
            AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest();
            request.GrantType = "authorization_code";
          
            AlipaySystemOauthTokenResponse response = client.Execute(request);
            return response;
        }
        //获取支付宝信息
        public AlipayUserInfoShareResponse getzfbuser(T_PayMentAcount model,string token)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", model.app_id, model.private_key, "json", "1.0", "RSA2", model.public_key_zf, "GBK", false);
            AlipayUserInfoShareRequest request = new AlipayUserInfoShareRequest();
            AlipayUserInfoShareResponse response = client.Execute(request, token);
            return response;
        }
    }
}
