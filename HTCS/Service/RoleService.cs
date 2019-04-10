using ControllerHelper;
using DAL;
using Model;
using Model.Bill;
using Model.House;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class RoleService
    {
        RoleDAL dal = new RoleDAL();
        public SysResult<List<T_Button>> Querybutton(T_SysUserRole model)
        {
            SysResult<List<T_Button>> result = new SysResult<List<T_Button>>();
            List<T_Button> list = dal.Query(model);
            
            result.numberData = list;
            result.numberCount = list.Count();
            return result;
        }
        public SysResult<List<T_CellName>> Querystore(T_CellName model, OrderablePagination orderablePagination,int[] arr)
        {
            SysResult<List<T_CellName>> result = new SysResult<List<T_CellName>>();
        
            List<T_CellName> list = dal.storeQuery(model, orderablePagination,arr);
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //查询门店详情
        public SysResult<T_CellName> Queryid(T_CellName model)
        {
            SysResult<T_CellName> result = new SysResult<T_CellName>();
            T_CellName cell = dal.storeQueryid(model);
            if (cell.regtype == 4)
            {
                model.Id = cell.parentid;
                T_CellName area = dal.storeQueryid(model);
                model.Id = area.parentid;
                T_CellName city = dal.storeQueryid(model);
                cell.AreaName = area.Name;
                cell.CityName = city.Name;
                result.numberData = cell;
            }
            if (cell.regtype ==5)
            {
                model.Id = cell.parentid;
                T_CellName area = dal.storeQueryid(model);
                model.Id = area.parentid;
                cell.CityName = area.Name;
                result.numberData = cell;
            }
            return result;
        }

        public SysResult storesave(T_CellName savemodel)
        {
            SysResult result = new SysResult();
            try
            {
                if (savemodel.Id == 0)
                {
                    savemodel.regtype = 4;
                    savemodel.Type = 4;
                    ProceDAL dal = new ProceDAL();
                    result = dal.CmdProce4(savemodel);
                }
                else
                {
                    T_CellName cell = dal.storeQueryid(new T_CellName() {Id=savemodel.Id });
                    cell.parentid = savemodel.parentid;
                    T_CellName areaname = dal.storeQueryid(new T_CellName() { Name = savemodel.AreaName });
                    cell.parentid = areaname.Id;
                    cell.Name = savemodel.Name;
                    dal.storesave(cell);
                }
              
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        public SysResult areasave(T_CellName savemodel)
        {
            SysResult result = new SysResult();
            try
            {
                if (savemodel.Id == 0)
                {
                    savemodel.regtype = 5;
                    savemodel.Type = 4;
                    ProceDAL pdal = new ProceDAL();
                    result = pdal.CmdProce5(savemodel);

                }
                else
                {
                    long parentid = 0;
                    T_CellName areaname = dal.storeQueryid(new T_CellName() { Name = savemodel.CityName });
                    T_CellName cell = dal.storeQueryid(savemodel);
                    if (areaname == null)
                    {
                        parentid= dal.storesave(new T_CellName() { Name=savemodel.CityName,Type=4,regtype=1,CompanyId= savemodel .CompanyId});
                    }
                    else
                    {
                        parentid = areaname.Id;
                    }
                    cell.parentid = parentid;
                    cell.Name = savemodel.AreaName;
                    dal.storesave(cell);
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        public SysResult save(T_SysRole model)
        {
            SysResult result = new SysResult();
            try
            {

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
        public SysResult<T_SysRole> QueryUser(T_SysRole model)
        {
            BaseDataDALL bdal = new BaseDataDALL();
            SysResult<T_SysRole> result = new SysResult<T_SysRole>();
            T_SysRole user = dal.QueryUerbyid(model);
            result.numberData = user;
            return result;
        }
        //删除
        public SysResult delete(iids model)
        {
           
            SysResult result = new SysResult();
            dal.delete(model);
            return result;
        }

    }
}
