using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Model.Base;
using ControllerHelper;

namespace Service
{
    public  class PushService
    {
        PushDAL dal = new PushDAL();
        public SysResult Push(ParamPhsh param)
        {
            SysResult result = new SysResult();
            string restr = dal.ExecutePushExample(param);
            PushResult model = JsonConvert.DeserializeObject<PushResult>(restr);
            if (model.error==null)
            {
                result = result.SuccessResult("发送成功");
            }
            else
            {
                result = result.FailResult("发送失败");
            }
            return result;
        }
        public SysResult Zhuce(ParamPhsh param)
        {
            SysResult result = new SysResult();
            dal.ExecuteDeviceEample(param);
            return result.SuccessResult("注册极光成功");
        }

        public SysResult<List<T_SysMessage>> ListMessage(T_SysMessage model, OrderablePagination orderablePagination)
        {
            SysResult<List<T_SysMessage>> result = new SysResult<List<T_SysMessage>>();
            result.numberData= dal.Queryfy(model, orderablePagination);
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
    }
}
