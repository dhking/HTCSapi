using ControllerHelper;
using DAL.Common;
using Model;
using Model.Bill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RzService
    {
        RzDAL dal = new RzDAL();
        public SysResult<List<WrapHouseRz>> Query(HouseRz model, OrderablePagination orderablePagination)
        {
            List<WrapHouseRz> list = new List<WrapHouseRz>();
            list = dal.Query(model, orderablePagination);
            SysResult<List<WrapHouseRz>> result = new SysResult<List<WrapHouseRz>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }

        public SysResult save(long createperson,long houseid,string type,string content,long companyid)
        {
            SysResult result = new SysResult();
            dal.Save(new HouseRz() { createperson = createperson, houseid = houseid, type = type, content = content, companyid = companyid });
            return result;
        }
        public SysResult save1(HouseRz model)
        {
            SysResult result = new SysResult();
            dal.Save(model);
            return result;
        }
        public string getcontent(Dictionary<string, string> dic)
        {
            string content = "";
            if (dic == null)
            {
                return content;
            }
            if (dic.ContainsKey("Price")&& dic.ContainsKey("yPrice"))
            {
                if (dic["Price"] != dic["yPrice"])
                {
                    content += "市场价变动:从" + dic["Price"] + "元到" + dic["yPrice"]+";";
                }
            }
            if (dic.ContainsKey("HouseKeeper") && dic.ContainsKey("yHouseKeeper"))
            {
                if (dic["HouseKeeper"] != dic["yHouseKeeper"])
                {
                    content+= "房管员变动:从" + dic["HouseKeeper"] + "到" + dic["yHouseKeeper"] + ";";
                }
            }
            if (dic.ContainsKey("Adress") && dic.ContainsKey("yAdress"))
            {
                if (dic["Adress"] != dic["yAdress"])
                {
                    content += "地址变动:从" + dic["Adress"] + "元到" + dic["yAdress"] + ";";
                }
            }
            return content;
        }
        //添加房源操作日志
        public SysResult addrz(HouseModel model, long type, long userid, long parentid,  Dictionary<string,string> dic=null)
        {
            SysResult result = new SysResult();
            if (model.RecrntType == 1)
            {
                string typestr = "";
                string content = "";
                if (type == 0)
                {
                    typestr = "新增房源";
                    content = "新增房源";
                }
                else
                {
                    content=getcontent(dic);
                    typestr = "编辑房源";
                }
                save(userid, parentid, typestr, content, model.CompanyId);
            }
            if (model.RecrntType == 2|| model.RecrntType == 3)
            {
                string errmsg = "";
                bool re = dal.addrz(parentid, userid, model.CompanyId, model.RecrntType, out errmsg);
            }
            return result;
        }
        //添加租客合同日志
        public SysResult contractaddrz(long HouseId, long userid, long CompanyId, int type)
        {
            SysResult sysresult = new SysResult();
            RzService rzdal = new RzService();
            string typestr = "租客合同操作";
            string content = "新增合同";
            if (type == 0)
            {
                content = "新增租客合同";
            }
            else
            {
                content = "重置租客合同";
            }
            rzdal.save(userid, HouseId, typestr, content, CompanyId);
            return sysresult;
        }
        //账单日志
        public SysResult addzdrz(T_Bill oribill, T_Bill nowbill,long userid)
        {
            SysResult sysresult = new SysResult();
            string typestr = "";
            string content = "";
            if (oribill.BillType == 0)
            {
                typestr = "租客账单";
            }
            if (oribill.BillType  ==1)
            {
                typestr = "业主账单";
            }
            content = "账单编辑操作-"+oribill.stage+"期;操作前金额" + oribill.Amount + ";操作后金额" + nowbill.Amount;
            save(userid, oribill.HouseId, typestr, content, oribill.CompanyId);
            return sysresult;
        }
        //业主合同日志
        public SysResult ocontractaddrz(long HouseId, long userid, long CompanyId, int type)
        {
            SysResult sysresult = new SysResult();
            RzService rzdal = new RzService();
            string typestr = "业主合同操作";
            string content = "新增业主合同";
            if (type == 0)
            {
                content = "新增业主合同";
            }
            else
            {
                content = "重置业主合同";
            }
            rzdal.save(userid, HouseId, typestr, content, CompanyId);
            return sysresult;
        }
    }
}
