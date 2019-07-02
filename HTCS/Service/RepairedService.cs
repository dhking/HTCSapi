using ControllerHelper;
using DAL;
using Model;
using Model.Base;
using Model.House;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class RepairedService
    {
        RepaireDAL dal = new RepaireDAL();
        public SysResult<List<WrapRepaire>> Query(Repaire model, OrderablePagination orderablePagination)
        {
            List<WrapRepaire> list = new List<WrapRepaire>();
            list = dal.Querylist(model, orderablePagination);
            //查询维修项目
            if (list != null)
            {
                List<long> arr = list.Select(p => p.Id).ToList();
                List<WrapRepairList> repairlist = dal.Queryrepairelistin(arr);
                foreach (var m in list)
                {
                    m.list = repairlist.Where(p => p.RepairId == m.Id).ToList();
                }
            }
            SysResult<List<WrapRepaire>> result = new SysResult<List<WrapRepaire>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        //查询报修科目
        public SysResult<List<T_Basics>> subject()
        {
            T_Basics model = new T_Basics();
            model.Code = "baoxiu";
            BaseDataDALL datadal = new BaseDataDALL();
            List<T_Basics> list = new List<T_Basics>();
            SysResult<List<T_Basics>> result = new SysResult<List<T_Basics>>();
            list = datadal.Querylistbytype(model);
            result.numberData = list;
            return result;
        }
        //筛选数据
        public SysResult<WrapShaixuan> shaixuan(WrapCell model,string [] citys, string[] cellname, T_SysUser user)
        {
            WrapShaixuan shaixuan = new WrapShaixuan();
            FormatterService formatter = new Service.FormatterService();

            shaixuan.subject = subject().numberData;
            shaixuan.cell = formatter.Querycell(model, citys, cellname,user);
            SysResult<WrapShaixuan> result = new SysResult<WrapShaixuan>();
            result.numberData = shaixuan;
            return result;
        }
        //报修详情
        public SysResult<WrapRepaire> Queryxq(Guest model)
        {
            WrapRepaire list = new WrapRepaire();
            SysResult<WrapRepaire> result = new SysResult<WrapRepaire>();
            list = dal.Queryxq(model);

            result.numberData = list;
            return result;

        }
        //后台报修详情
        public SysResult<WrapRepaire> hQueryxq(Guest model)
        {
            WrapRepaire list = new WrapRepaire();
            SysResult<WrapRepaire> result = new SysResult<WrapRepaire>();
            list = dal.hQueryxq(model);
            result.numberData = list;
            return result;

        }
        //接单
        public SysResult Receipt(RepairList model)
        {
          
            SysResult result = new SysResult();
            result.Message = "接单成功";
            model.Status = 1;
            dal.saverepairlist(model);
            return result;

        }
        //结束
        public SysResult end(RepairList model)
        {

            SysResult result = new SysResult();
            result.Message = "操作成功";
            model.Status = 2;
            dal.saverepairlist(model);
            return result;

        }
        //删除报修
        public SysResult delete(Repaire ids)
        {
            SysResult result = new SysResult();
            ProceDAL dal = new ProceDAL();
            Pure pure = new Pure();
            pure.Id = ids.Id.ToStr();
            pure.Spname = "sp_deleterepaire";
            result = dal.Cmdproce8(pure);
            return result;
        }
        //报修列表
        public SysResult<List<RepairList>> Queryrepairlist(RepairList model)
        {
            List<RepairList> list = new List<RepairList>();
            list = dal.Queryrepairelist(model);
            SysResult<List<RepairList>> result = new SysResult<List<RepairList>>();
            result.numberData = list;
            result.numberCount = list.Count();
            return result;

        }
        //报修列表展示后台 
        public SysResult<List<RepairList>> Queryhrepairlist(RepairList model)
        {
            SysResult<List<RepairList>> result = new SysResult<List<RepairList>>();
            List<RepairList> list = new List<RepairList>();
            list = dal.Queryrepairelist1(model);
            if (list != null)
            {
                //查询表头信息
                List<Repaire> list1 = new List<Repaire>();
                list1 = dal.Queryrepaire(list.Select(p => p.RepairId).ToList());
                foreach(var mo in list)
                {
                    Repaire repa = list1.Where(p => p.Id == mo.RepairId).FirstOrDefault();
                    if (repa != null)
                    {
                        mo.CreateTime = repa.CreateTime;
                        mo.AppiontTime = repa.AppiontTime;
                        mo.House= repa.House;
                        //mo.Id = repa.Id;
                        mo.Adress= repa.Adress;
                        mo.JournaList = repa.JournaList;
                        mo.Phone = repa.Phone;
                    }
                }   
            }
            result.numberData = list;
            result.numberCount = list.Count();
            return result;
        }

        public SysResult save(Repaire model)
        {
            SysResult result = new SysResult();
            try
            {
                HouseDAL housedal = new HouseDAL();
                HouseQuery query = housedal.Queryhouse1(model.HouseId);
                if (query == null)
                {
                    result.Code = 1;
                    result.Message = "房源不存在";
                    return result;
                }
               
                model.City = query.CityName;
                model.Area = query.AreaName;
                model.Adress = query.Adress;
                model.House = query.Name;
                if (dal.save(model) > 0)
                {
                    result = result.SuccessResult("保存成功");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        public SysResult Receipt(Repaire model)
        {
            SysResult result = new SysResult();
            try
            {
                HouseDAL housedal = new HouseDAL();
                
                if (dal.save(model) > 0)
                {
                    result = result.SuccessResult("保存成功");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
    }
}
