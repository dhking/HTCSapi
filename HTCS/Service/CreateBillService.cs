using DAL;
using Model;
using Model.Bill;
using Model.Contrct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class CreateBillService
    {
        CreateBillDAL dal = new CreateBillDAL();
        public SysResult<List<T_Bill>> getbill(paraCreate bill)
        {
            SysResult<List<T_Bill>> result = new SysResult<List<T_Bill>>();
            SysResult voliteresult = new SysResult();
            voliteresult = volite(bill);
            if (voliteresult.Code != 0)
            {
                result.Code = 1;
                result.Message = voliteresult.Message;
                return result;
            }
            if (bill.Type == 1)
            {
                result.numberData = dal.getrzbill(bill);
            }
            if (bill.Type ==2)
            {
                result.numberData = dal.getwrapbill(bill);
            }
            return result;
        }
        public SysResult volite(paraCreate model)
        {
            SysResult result = new SysResult();
            if (model.Type== 0)
            {
                return result = result.FailResult("请选择租房类型");
            }
            if (model.Teant == null)
            {
                return result = result.FailResult("填写租客信息");
            }
            if (string.IsNullOrEmpty(model.Teant.Phone))
            {
                return result = result.FailResult("请填写手机号");
            }
            if (string.IsNullOrEmpty(model.Teant.Document))
            {
                return result = result.FailResult("请填写证件信息");
            }
            if (model.BeginTime==DateTime.MinValue)
            {
                return result = result.FailResult("请填写合同开始时间");
            }
           
            if (model.EndTime == DateTime.MinValue)
            {
                return result = result.FailResult("请填写合同结束时间");
            }
            if (model.Recent==0)
            {
                return result = result.FailResult("请填写月租金");
            }
            if (model.Recivetype == 0)
            {
                return result = result.FailResult("请选择支付时间");
            }
            return result;
        }
    }
}
