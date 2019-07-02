using DAL;
using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public   class ProceService
    {
        ProceDAL dal = new ProceDAL();
        public SysResult CmdProce(Pure model)
        {
            return dal.CmdProce(model);
        }
        public SysResult CmdProce1(Pure model)
        {
            return dal.Cmdproce8(model);
        }
        public SysResult CmdProce2(Pure model)
        {
            return dal.Cmdproce9(model);
        }
        public SysResult CmdProce3(Pure model, T_SysUser user,string key)
        {
            SysResult result = new SysResult();
            SysUserService service = new SysUserService();
            string AreamName, CellName, CityName;
            result =dal.Cmdproce11(model,out CityName, out AreamName, out CellName);
            if (model.Spname== "sp_zhuanyi"&&result.Code==0)
            {
                eidtredis(model.Other1, AreamName, CellName, CityName);
            }
            return result;
        }
        public SysResult CmdProce4(Pure model)
        {
            model.Spname = "sp_billsign";
            return dal.Cmdproce10(model);
        }

        public SysResult eidtredis(string  userid,string AreamName, string CellName, string CityName)
        {
            SysResult result = new SysResult();
            string access_token = DAL.Common.ConvertHelper.GetMd5HashStr(userid);
            string key = "sysuser_key" + access_token;
            RedisHtcs rds = new RedisHtcs();
            T_SysUser user = rds.GetModel<T_SysUser>(key);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(AreamName))
                {
                    user.area = AreamName;
                }
                if (!string.IsNullOrEmpty(CityName))
                {
                    user.city = CityName;
                }
                if (!string.IsNullOrEmpty(CellName))
                {
                    user.cellname = CellName;
                }
                rds.SetModel<T_SysUser>(key, user);
            }
          
            return result;
        }
    }
}
