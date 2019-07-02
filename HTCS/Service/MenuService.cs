using DAL;
using Model;
using Model.Menu;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public   class MenuService
    {
        MenuDAL dal = new MenuDAL();
        public SysResult<List<T_Menu>> Querymenu()
        {
            SysResult<List<T_Menu>> sysresult = new SysResult<List<T_Menu>>();
            sysresult.Code = 0;
            sysresult.Message = "查询成功";
            sysresult.numberData =dal.Query();
            return sysresult;
        }
        public SysResult<List<T_Menu>> Querylist(T_SysUser model)
        {
            int[] sign =new int[] {1 };
            if (model.CompanyId == 1)
            {
                sign= new int[] { 1,3 };
            }
            SysResult<List<T_Menu>> sysresult = new SysResult<List<T_Menu>>();
            sysresult.Code = 0;
            sysresult.Message = "查询成功";
            sysresult.numberData = dal.Querylist(model, sign);
            return sysresult;
        }
        public SysResult<List<T_Menu>> Querylist1(T_SysUser model, int sign)
        {
            SysResult<List<T_Menu>> sysresult = new SysResult<List<T_Menu>>();
            sysresult.Code = 0;
            sysresult.Message = "查询成功";
            sysresult.numberData = dal.Querylist1(model, sign);
            return sysresult;
        }
        public SysResult<List<T_Menu>> Querymenufy()
        {
            SysResult<List<T_Menu>> sysresult = new SysResult<List<T_Menu>>();
            List<T_Menu> list = dal.Query();
            sysresult.numberData = list;
            sysresult.numberCount = list.Count;
            return sysresult;
        }
        public SysResult Savemenu(T_Menu model)
        {
            SysResult sysresult = new SysResult();
            if (dal.SaveMenu(model) > 0)
            {
                sysresult = sysresult.SuccessResult("保存成功");
            };
            return sysresult;
        }
        public SysResult deleteData(long ids)
        {

            SysResult result = new SysResult();
            if (dal.deletedata(ids) > 0)
            {
                return result = result.SuccessResult("删除成功");
            }
            else
            {
                return result = result.FailResult("删除失败");
            }
        }
        public SysResult<T_Menu> Queryid(T_Menu model)
        {
            SysResult<T_Menu> result = new SysResult<T_Menu>();

            result.numberData = dal.QueryId(model);
            return result;
        }
    }
}
