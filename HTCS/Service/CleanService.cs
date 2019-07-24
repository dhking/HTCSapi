using ControllerHelper;
using DAL;
using Model;
using Model.Base;
using Model.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{

    public  class CleanService
    {
        CleanDAL dal = new CleanDAL();
        public SysResult<List<Wrapclean>> Query(Wrapclean model, OrderablePagination orderablePagination)
        {
            List<Wrapclean> list = new List<Wrapclean>();
            if (model.CellNames != null)
            {
                model.arrCellNames = model.CellNames.Split(',');
            }
            list = dal.Querylist(model, orderablePagination);
            SysResult<List<Wrapclean>> result = new SysResult<List<Wrapclean>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        public SysResult<Wrapclean> Queryxq(clean model)
        {
            Wrapclean list = new Wrapclean();
            SysResult<Wrapclean> result = new SysResult<Wrapclean>();
            list = dal.Queryxq(model);
            result.numberData = list;
            return result;

        }
        public SysResult<List<wrapcleanRZ>> QueryRz(cleanRZ model)
        {
            List<wrapcleanRZ> list = new List<wrapcleanRZ>();
            list = dal.QuerylistRz(model);
            SysResult<List<wrapcleanRZ>> result = new SysResult<List<wrapcleanRZ>>();
            result.numberData = list;
            result.numberCount =list.Count();
            return result;

        }
        public SysResult add(clean model,long userid)
        {
            SysResult result = new SysResult();
            try
            {
                if (model.ugent == 0)
                {
                    model.ugent = 1;
                }
                dal.Save(model);
                if (model.Id != 0&& !string.IsNullOrEmpty(model.content))
                {
                   
                    addrz(new cleanRZ() { content = model.content, cleanid = model.Id, opera = userid, companyid = model.companyid });
                }
                result = result.SuccessResult("保存成功");
            }
            catch(Exception ex)
            {
                result = result.FailResult("保存失败" + ex.ToString());
            }
            return result;
        }
        public SysResult addrz(cleanRZ model)
        {
            SysResult result = new SysResult();
            try
            {
                dal.Saverz(model);
                result = result.SuccessResult("添加成功");
            }
            catch (Exception ex)
            {
                result = result.FailResult("添加失败" + ex.ToString());
            }
            return result;
        }
        //查询报修科目
        public SysResult<List<T_Basics>> subject()
        {
            T_Basics model = new T_Basics();
            model.Code = "baojie";
            BaseDataDALL datadal = new BaseDataDALL();
            List<T_Basics> list = new List<T_Basics>();
            SysResult<List<T_Basics>> result = new SysResult<List<T_Basics>>();
            list = datadal.Querylistbytype(model);
            result.numberData = list;
            return result;
        }
    }
}
