using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignApplication;
using System.Activities.Statements;
using System.Web;
using System.IO;
using System.Configuration;
using Model.Contrct;
using DAL;

namespace Service
{
    public   class SignService
    {

        public SysResult signatureChange()
        {

            SysResult sysresult = new SysResult();
            string name = "";
            string id_card_no = "";
            string moblie = "";
            string user_code = "";
            IDictionary<string, string> parameters = new Dictionary<string, string>();


            parameters.Add("signature", id_card_no);
            parameters.Add("sign_val", moblie);
            parameters.Add("user_code", user_code);
            parameters.Add("zqid", Config.ZQID);
            //MessageBox.Show(moblie);
            string context = RSAUtil.GetSignContent(parameters);
            string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
            sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);

            parameters.Add("sign_val", sign_val);

            string result = HTTPUtil.CreatePostHttpResponse(Config.URL + "signatureChange", parameters);
            return sysresult;
        }
        public SysResult reduce(SignVersion model)
        {
            ProceDAL pdal = new ProceDAL();
            //查询公司编号
            ContrctDAL dal = new ContrctDAL();
            T_Contrct contract = dal.querycontract(long.Parse(model.no));
            SysResult sysresult= pdal.Cmdproce10(new Pure() { Spname = "sp_reducenumber", Ids = contract.CompanyId.ToStr(), Other = "2" });
            return sysresult;
        }
        public SysResult completionContract(SignVersion model)
        {

            SysResult sysresult = new SysResult();
            //减少电子合同数量
            sysresult = reduce(model);
            if (sysresult.Code != 0)
            {
                return sysresult;
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("no", model.no);
            parameters.Add("zqid", Config.ZQID);
            //MessageBox.Show(moblie);
            string context = RSAUtil.GetSignContent(parameters);
            string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
            sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);

            parameters.Add("sign_val", sign_val);

            string result = HTTPUtil.CreatePostHttpResponse(Config.URL + "completionContract", parameters);

            Signresult signresult = Newtonsoft.Json.JsonConvert.DeserializeObject<Signresult>(result);

            sysresult.Code = int.Parse(signresult.code);
            sysresult.Message = signresult.msg;
            return sysresult;
        }
        public SysResult personReg(SignVersion model)
        {
            SysResult sysresult = new SysResult();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", model.name);
            parameters.Add("id_card_no", model.id_card_no);
            parameters.Add("mobile", model.moblie);
            parameters.Add("user_code", model.user_code);
            parameters.Add("zqid", Config.ZQID);
            //MessageBox.Show(moblie);
            string context = RSAUtil.GetSignContent(parameters);
            string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
            sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);

            parameters.Add("sign_val", sign_val);

            string result = HTTPUtil.CreatePostHttpResponse(Config.URL + "personReg", parameters);
            Signresult signresult = Newtonsoft.Json.JsonConvert.DeserializeObject<Signresult>(result);
            sysresult.Code = int.Parse(signresult.code);
            sysresult.Message = signresult.msg;
            return sysresult;
        }
        public SysResult<WrapContract> signAuto(SignVersion model)
        {
            SysResult<WrapContract> result = new SysResult<WrapContract>();
            WrapContract wrap = new WrapContract();
            result.Code = 0;
            result.Message = "成功";

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("no", model.no);
            parameters.Add("zqid", Config.ZQID);


            if (model.sign_type.Equals("自动签署"))
            {
                parameters.Add("signers", model.user_code);
                string context = RSAUtil.GetSignContent(parameters);
                string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
                sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);
                parameters.Add("sign_val", sign_val);
                HTTPUtil.CreatePostHttpResponse(Config.URL + "signAuto", parameters);
                string res = HTTPUtil.CreatePostHttpResponse(Config.URL + "signAuto", parameters);
            

            }
            else
            {
               
                parameters.Add("sign_type", model.sign_type);
                parameters.Add("user_code", model.user_code);
                parameters.Add("notify_url", Config.notify_url);
                parameters.Add("return_url", Config.return_url);
                string context = RSAUtil.GetSignContent(parameters);
                string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
                parameters.Add("sign_val", sign_val);

                string html = HTTPUtil.formatFormatHtml(parameters, Config.URL + "mobileSignView");
                string htmls = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title></title></head><body>" + html + "</body></html>";
                wrap.form = htmls;
                result.numberData = wrap;
            }
            return result;

        }
        public SysResult createForWord(SignVersion model)
        {
            string filepath = ConfigurationManager.AppSettings["signpath"]+"/" +model.filename;
            SysResult sysresult = new SysResult();
            byte[] bt = null;
            using (FileStream fs = File.OpenRead(filepath))
            {
                BinaryReader br = new BinaryReader(fs);
                bt = br.ReadBytes(Convert.ToInt32(fs.Length));
                br.Close();
                br.Dispose();
            }
            string contract_file = Convert.ToBase64String(bt);
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("no", model.no);
            parameters.Add("name",Path.GetFileNameWithoutExtension(filepath) );
            parameters.Add("contract", contract_file);
            parameters.Add("zqid", Config.ZQID);
            //计算签名值
            string context = RSAUtil.GetSignContent(parameters);
            string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
            //将合同文件和签名值进行url编码 放入parameters中
            contract_file = HttpUtility.UrlEncode(contract_file, Encoding.UTF8);
            parameters.Remove("contract");
            parameters.Add("contract", contract_file);
            sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);
            parameters.Add("sign_val", sign_val);
            string result = HTTPUtil.CreatePostHttpResponse(Config.URL + "createForWord", parameters);
            Signresult signresult = Newtonsoft.Json.JsonConvert.DeserializeObject<Signresult>(result);
            sysresult.Code =int.Parse(signresult.code);
            sysresult.Message = signresult.msg;
            return sysresult;
        }
    }
}
