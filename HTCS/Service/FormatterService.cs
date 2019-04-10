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
        public List<WrapCell>  Querycell(WrapCell model)
        {
            List<WrapCell> wraplist = new List<WrapCell>();
            model.regtype = 3;
            wraplist =dal.QueryOne(model);
            return wraplist;
        }

        public List<WrapCell> Querycell1(WrapCell model)
        {
            List<WrapCell> wraplist = new List<WrapCell>();
            model.regtype = 3;
            wraplist = dal.QueryOne1(model);
            return wraplist;
        }
        public List<WrapCell> Querystore(WrapCell model)
        {
            List<WrapCell> wraplist = new List<WrapCell>();
            model.regtype = 4;
            wraplist = dal.QueryOne3(model);
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
