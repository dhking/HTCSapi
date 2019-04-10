using ControllerHelper;
using DAL;
using Model;
using Model.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class GuestService
    {
        GuestDAL dal = new GuestDAL();
        public SysResult<List<WrapGuest>> Query(Guest model, OrderablePagination orderablePagination)
        {
            List<WrapGuest> list = new List<WrapGuest>();
            list = dal.Querylist(model, orderablePagination);
            SysResult<List<WrapGuest>> result = new SysResult<List<WrapGuest>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        public SysResult<WrapGuest> Queryxq(Guest model)
        {
            WrapGuest list = new WrapGuest();
            SysResult<WrapGuest> result = new SysResult<WrapGuest>();
            list = dal.Queryxq(model);
            
            result.numberData = list;
            return result;

        }
        public SysResult<List<GuestRz>> QueryRz(GuestRz model)
        {
            List<GuestRz> list = new List<GuestRz>();
            list = dal.QuerylistRz(model);
            SysResult<List<GuestRz>> result = new SysResult<List<GuestRz>>();
            result.numberData = list;
            result.numberCount =list.Count();
            return result;

        }
        public SysResult add(Guest model)
        {
            SysResult result = new SysResult();
            try
            {
                if (model.Ugent == 0)
                {
                    model.Ugent = 1;
                }
               
                dal.Save(model);
              result = result.SuccessResult("添加成功");
            }
            catch(Exception ex)
            {
                result = result.FailResult("添加失败"+ex.ToString());
            }
            
            return result;

        }

        public SysResult gwadd(Appointment model)
        {
            SysResult result = new SysResult();

            HouseDAL housedal = new HouseDAL();
            HouseQuery house = housedal.Queryhouse1(model.HouseId);
            try
            {
                Guest savemodel = new Guest();
                savemodel.Name = model.Name;
                savemodel.AppointDate = model.date;
                savemodel.Sex = model.Sex;
                savemodel.Phone = model.Phone;
                savemodel.Remark = model.Content;
                savemodel.Source = "官网";
                savemodel.Ugent = 1;
                if (house != null)
                {
                    savemodel.HopeAdress = house.Name;
                }
                dal.Save(savemodel);
                result = result.SuccessResult("预约成功");
            }
            catch (Exception ex)
            {
                result = result.FailResult("预约失败" + ex.ToString());
            }

            return result;

        }
        public SysResult addrz(GuestRz model)
        {
            SysResult result = new SysResult();
            try
            {
                //model.Cont = model.CreatePerson + model.Type+model;
                if (model.Fujian!=null&& model.Fujian!="")
                {
                    model.Fujian = model.Fujian.Substring(0, model.Fujian.Length - 1);
                }
               
                dal.Saverz(model);
                result = result.SuccessResult("添加成功");
            }
            catch (Exception ex)
            {
                result = result.FailResult("添加失败" + ex.ToString());
            }

            return result;

        }
        public SysResult giveup(Guest model)
        {
            SysResult result = new SysResult();
            model.Status = 8;

            if (dal.action(model, new string[] { "Status" }) > 0)
            {
                result = result.SuccessResult("操作成功");
            }
            else
            {
                result = result.FailResult("操作失败");
            }
            return result;
        }
        public SysResult distribution(Guest model)
        {
            SysResult result = new SysResult();
            model.Status = 2;
            if (dal.action(model, new string[] { "UserId", "Status" }) > 0)
            {
                result = result.SuccessResult("操作成功");
            }
            else
            {
                result = result.FailResult("操作失败");
            }
            return result;
        }
    }
}
