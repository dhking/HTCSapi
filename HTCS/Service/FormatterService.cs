using DAL;
using Model;
using Model.House;
using Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FormatterService
    {
        FormatterDAL dal = new FormatterDAL();
        public string Query(Formatter model)
        {
            return dal.Query(model);
        }
        public T Queryxy<T>(Formatter model)
        {
            return dal.Queryxq<T>(model);
        }
        public List<WrapCell>  Querycell(WrapCell model, string[] arr,string[] cellname, T_SysUser user)
        {
            List<WrapCell> wraplist = new List<WrapCell>();
            model.regtype = 3;
            wraplist =dal.QueryOne(model, arr, cellname, user);
            return wraplist;
        }
        public List<WrapCellBuilding> Querycellbuilding(WrapCell model, string[] arr, string[] cellname,T_SysUser user)
        {
            List<WrapCellBuilding> wraplist = new List<WrapCellBuilding>();
            model.regtype = 3;
            model.Type = 3;
            wraplist = dal.QueryOnebuilding(model, arr, cellname, user);
            return wraplist;
        }
        

        public List<WrapCell> Querycell1(WrapCell model, string[] arr, string[] cellname, T_SysUser user)
        {
            List<WrapCell> wraplist = new List<WrapCell>();
            model.regtype = 3;
            wraplist = dal.QueryOne1(model, arr, cellname, user);
            return wraplist;
        }
        public List<WrapCell> Querystore(WrapCell model, string[] arr,string[] cellname, T_SysUser user)
        {
            List<WrapCell> wraplist = new List<WrapCell>();
            model.regtype = 4;
            wraplist = dal.QueryOne3(model, arr, cellname, user);
            return wraplist;
        }
    
        public List<WrapCity> Querycity(WrapCell model)
        {
            List<WrapCity> wraplist = new List<WrapCity>();
            wraplist = dal.Querycity1( model);
            return wraplist;
        }
        
    }
}
