using DAL;
using Model;
using System;
using Newtonsoft.Json;
using Model.User;
using Aop.Api.Response;
using DBHelp;
using System.Collections.Generic;
using Aop.Api;
using Aop.Api.Request;
using Model.Base;
using Model.Contrct;

namespace Service
{

    public class ThirdService
    {
        ThirdDAL dal = new ThirdDAL();
        UserDAL1 userdal = new UserDAL1();
        //身份证识别 
        public SysResult<string> baiduid(Idcard model,string image,string fimage)
        {
            SysResult<string> result = new SysResult<string>();
            //获取token
            string token = "";
            token = dal.GetbaiduToken();
            //调用身份证识别正面
            string re = dal.idcard(model.image, token, "front");
            returnidcard idmodel = JsonConvert.DeserializeObject<returnidcard>(re);
            //调用身份证反面
            string re1 = dal.idcard(model.fimage, token, "back");
            returnidcard idmodel1 = JsonConvert.DeserializeObject<returnidcard>(re1);
            if(idmodel.image_status!= "normal")
            {
                string statusz = geterr(idmodel.image_status);
                result.Code = 1;
                result.Message = "正面认证失败:" + statusz;
                return result;
            }
            if(idmodel1.image_status != "normal")
            {
                string statusf = geterr(idmodel1.image_status);
                result.Code = 1;
                result.Message = "反面认证失败:" + statusf;
                return result;
            }
            //把身份证信息保存到本地
            if (string.IsNullOrEmpty(idmodel.error_code)&&string.IsNullOrEmpty(idmodel1.error_code))
            {
                DateTime dtime = DateTime.ParseExact(idmodel1.words_result.失效日期.words, "yyyyMMdd", null);
                if (dtime<DateTime.Now)
                {
                    result.Code = 1;
                    result.Message = "认证失败:身份证过期";
                    return result;
                }
                T_CertIfication cert = new T_CertIfication();
                cert.UserId = model.UserId;
                cert.idcard = idmodel.words_result.公民身份号码.words;
                cert.realname= idmodel.words_result.姓名.words;
                cert.idzimg = image;
                cert.idfimg = fimage;
                cert.status =2;
                cert.type = 1;
                userdal.savecertifica(cert);
                result.numberData = image + ";" + fimage;
                result.Code = 0;
                result.Message = "认证成功";
               
            }
            else
            {
                result.Code = 1;
                result.Message = "认证失败" + idmodel.error_msg + ";" + idmodel1.error_msg;
            
            }
            return result;
        }

        public SysResult Authentication(T_account model, long CompanyId,long userid)
        {
            SysResult result = new SysResult();
            SysUserService userservice = new SysUserService();
            if (userservice.Viltyzm(model.yzm, model.phone, 6))
            {
                BaseDataDALL dal = new BaseDataDALL();
                model.CompanyId = CompanyId;
                model.OnlinePay = 1;
                dal.saveaccount(model);
              
            }
            else
            {
                result = result.FailResult("验证码错误");
            }
            return result;
        }

        public SysResult accountsave(T_account model, long CompanyId, long userid)
        {
            SysResult result = new SysResult();
            SysUserService userservice = new SysUserService();
            if (userservice.Viltyzm(model.yzm, model.phone, 6))
            {
                BaseDataDALL dal = new BaseDataDALL();
                model.CompanyId = CompanyId;
                dal.saveaccount(model,new string[] { "zfb", "wx", "zfbname", "wxname", "account", "bank", "name" });

            }
            else
            {
                result = result.FailResult("验证码错误");
            }
            return result;
        }
        public SysResult function(T_account model, long CompanyId, long userid)
        {
            SysResult result = new SysResult();
            result.Message = "设置成功";
            SysUserService userservice = new SysUserService();
            BaseDataDALL dal = new BaseDataDALL();
            model.CompanyId = CompanyId;
            dal.saveaccount(model, new string[] { "rentmessage", "charge", "onlinesign" });
            return result;
        }
        public SysResult setpassword(T_account model, long CompanyId)
        {
            SysResult result = new SysResult();
            SysUserService userservice = new SysUserService();
            if (userservice.Viltyzm(model.yzm, model.phone,8))
            {
                BaseDataDALL dal = new BaseDataDALL();
                T_account account = dal.queryaccount(CompanyId);
                account.password = model.password;
                dal.saveaccount1(account,null);
            }
            else
            {
                result = result.FailResult("验证码错误");
            }

            
            return result;
        }
        //查询认证信息
        public SysResult<T_account> querycompany(long CompanyId)
        {
            SysResult<T_account> result = new SysResult<T_account>();
            SysUserService userservice = new SysUserService();
            BaseDataDALL dal = new BaseDataDALL();
            T_account account = new T_account();
            account.isshangjia = 0;
            account = dal.queryaccount(CompanyId);
            result.numberData= account;
            return result;
        }
        //支付认证模拟支付宝转账
        //public SysResult Authentication(zfbzz model,long userid)
        //{
        //    SysResult result = new SysResult();
        //    GetAcountDAL dal = new GetAcountDAL();
        //    zfbDAL zfdal = new DAL.zfbDAL();
        //    model.amount = "0.1";
        //    model.payee_type = "ALIPAY_LOGONID";
        //    model.payer_show_name = "打款验证";
        //    model.remark = "打款验证";
        //    model.out_biz_no = ConvertHelper.getsuijishu();
        //    string content = JsonConvert.SerializeObject(model);
        //    AlipayFundTransToaccountTransferResponse response=zfdal.Zfbzz(dal.GetAcount(1), content);
        //    if (response.Code == "10000")
        //    {
        //        T_SysUser user = new T_SysUser();
        //        user.Id = userid;
        //        user.Zfrz =1;
        //        user.Zfbzh =model.payee_account;

        //        userdal.saveUser(user, new string[] { "Zfrz", "Zfbzh" });
        //        result = result.SuccessResult("验证成功");
        //    }
        //    else
        //    {
        //        result = result.FailResult(response.SubMsg);
        //    }
        //    return result;
        //}
        //支付宝授权
        public SysResult<string> authtoken()
        {
            SysResult<string> result = new SysResult<string>();
            zfbDAL zfdal = new DAL.zfbDAL();
            GetAcountDAL dal = new GetAcountDAL();
            T_PayMentAcount pay = dal.GetAcount(1);
            AlipaySystemOauthTokenResponse response = zfdal.oauthtoken(pay);
            if(response.Code == "10000")
            {
                AlipayUserInfoShareResponse zfbuser= zfdal.getzfbuser(pay,response.AccessToken);
               
            }
            return result;
        }
        //微信授权
        public SysResult<string> wxauthtoken()
        {
            SysResult<string> result = new SysResult<string>();
            zfbDAL zfdal = new DAL.zfbDAL();
            GetAcountDAL dal = new GetAcountDAL();
            T_PayMentAcount pay = dal.GetAcount(1);
            AlipaySystemOauthTokenResponse response = zfdal.oauthtoken(pay);
            if (response.Code == "10000")
            {
                AlipayUserInfoShareResponse zfbuser = zfdal.getzfbuser(pay, response.AccessToken);

            }
            return result;
        }
        public SysResult certification(T_CertIfication model)
        {
            SysResult result = new SysResult();
            UserDAL1 dal = new UserDAL1();
            model.status = 1;
            model.type = 2;
            dal.savecertifica(model);
            return result.SuccessResult("已提交认证");
        }
        public SysResult<T_account> queryurl(T_account model)
        {
            SysResult<T_account> result = new SysResult<T_account>();
            ContrctDAL cdal = new ContrctDAL();
            BaseDataDALL dal = new BaseDataDALL();
            T_Contrct contract= cdal.querycontract(new T_Contrct() {Id= model.Id });
            T_account account = dal.queryaccount(contract.CompanyId);
            result.numberData = account;
            return result;
        }
        public SysResult<Wrapcert> querycert(T_CertIfication model)
        {
            SysResult<Wrapcert> result = new SysResult<Wrapcert>();
            UserDAL1 dal = new UserDAL1();
            Wrapcert wrap = new Wrapcert();
            List<T_CertIfication> list = dal.Querycertification(model.UserId);
            if (list != null)
            {
               foreach(var mo in list)
                {
                    if (mo.type == 2)
                    {
                        wrap.qiye = mo;
                    }
                    if (mo.type == 1)
                    {
                        wrap.geren = mo;
                    }
                }
            }
            result.numberData = wrap;
            return result;
        }
        
        public string geterr(string str)
        {
            switch (str)
            {
                case "normal":
                    return "识别正常";
                case "reversed_side":
                    return "身份证正反面颠倒";
                case "non_idcard":
                    return "上传的图片中不包含身份证";
                case "blurred":
                    return "身份证模糊";
                case "other_type_card":
                    return "其他类型证照";
                case "over_exposure":
                    return "图片不清晰,关键字段反光或过曝";
                case "unknown":
                    return "未知状态";
                default:
                    return "未知";
            }
                 
        }
    }
         
}
