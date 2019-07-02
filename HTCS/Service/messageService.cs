using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using Model.User;
using ControllerHelper;
using Model.Menu;
using Model.Bill;
using Model.Base;
using Model.Contrct;

namespace Service
{
    public  class messageService
    {
        SysUserService service = new SysUserService();

        public int gettype(int type)
        {
            if (type == 2)
            {
                return 7;
            }
            return 0;
        }
        public SysResult sendmessage(int type,long contract)
        {
            SysResult result = new SysResult();
            MessageDAL messagedal = new MessageDAL();
            string url = "";
            List<T_MessageQueue> listmessage = messagedal.Query(new T_MessageQueue() { type=type,status=0,contractid= contract });
            //发送短信
            foreach (var mo in listmessage)
            {
                yzRequest req = new yzRequest();
                req = service.GetRequest(gettype(mo.type), mo.phone,mo.name);
                req.CompanyId = mo.companyid;
                if (mo.type == 2)
                {
                   
                    ContrctDAL condal = new ContrctDAL();
                    WrapContract wrapcont= condal.QueryId(new Model.Contrct.WrapContract() { Id = mo.contractid });
                    string base64 = PuclicDataHelp.EncodeBase64("utf-8", wrapcont.Id.ToStr());
                    req.Temp = "{\"name\":\"" + wrapcont.Teant.Name + "\",\"recent\":\"" + wrapcont.Recent + "\",\"house\":\""+ wrapcont.HouseName + "\",\"begin\":\""+ wrapcont.BeginTime + "\",\"end\":\""+ wrapcont.EndTime + "\",\"contractid\":\""+ base64 + "\"}";
                }
                string message = "";
                SendMessageDAL dal = new SendMessageDAL();
                
                if (dal.SendMessage(req, out message))
                {
                    mo.status = 1;
                    messagedal.save(mo);
                    result = result.SuccessResult("发送成功");
                }
                else
                {
                    mo.status = 2;
                    mo.message = message;
                    messagedal.save(mo);
                    result = result.FailResult("发送失败" + message);
                }
            }
            return result;
        }
    }
}
