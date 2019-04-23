using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Mapping.cs;
using Model.Base;
using Model;
using ControllerHelper;
using System.Linq.Expressions;
using DBHelp;
using Model.House;

namespace DAL
{
    public class BaseDataDALL : RcsBaseDao
    {

        public SysResult<IList<T_V_basicc>> Querylist(T_Basics model, OrderablePagination orderablePagination)
        {
            SysResult<IList<T_V_basicc>> sysresult = new SysResult<IList<T_V_basicc>>();
            var data = from  a in t_v_basic select a;
            Expression<Func<T_V_basicc, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.IsActive != 999)
            {
                where = where.And(m => m.IsActive == model.IsActive);
            }

            data = data.Where(where);
            IOrderByExpression<T_V_basicc> order = new OrderByExpression<T_V_basicc, decimal>(p => p.Id,true);
            sysresult.numberData = this.QueryableForList<T_V_basicc>(data, orderablePagination, order);
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }

        public IList<T_bankcard> bankcardQuery()
        {
            SysResult<IList<T_bankcard>> sysresult = new SysResult<IList<T_bankcard>>();
            var data = from a in bankcard select a;
            return data.ToList();
        }
        //模板列表
        public List<T_template> templateQuery(T_template model)
        {
            List<T_template> list1 = new List<T_template>();
            //List<T_template> list2 = new List<T_template>();
            var data = from a in template select a;
            //var data1 = from b in template where b.ispublic== 1 select b;
            Expression<Func<T_template, bool>> where = m => 1 == 1;
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
               
            }
            data = data.Where(where);
            list1 = data.ToList();
            //list2 = data1.ToList();
            return list1.ToList();
        }
        public T_template xqQuery(T_template model)
        {
            var data = from a in template select a;
            Expression<Func<T_template, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.ispublic != 0)
            {
                where = where.And(m => m.ispublic == model.ispublic);
            }
            data = data.Where(where);
            return data.FirstOrDefault();
        }
        //默认模板
        public T_template morenQuery(T_template model)
        {
            T_template temp = new T_template();
            var data = from a in template  select a;
            Expression<Func<T_template, bool>> where = m => m.ispublic == 0;
            if (model.isdefault != 0)
            {
                where = where.And(m => m.isdefault == model.isdefault);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (data.Count() > 0)
            {
                temp= data.FirstOrDefault();
            }
            else
            {
                var data1 = from a in template where a.ispublic==1 select a;
                temp = data1.FirstOrDefault();
            }
            return temp;
        }
        //添加模板
        public int templateadd(T_template model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_TEMPLATE')");
                return AddModel<T_template>(model);

            }
            else
            {
                return ModifiedModel<T_template>(model, false, model.NotUpdatefield);
            }

        }
        //更新
        public int templateadd1(T_template model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_TEMPLATE')");
                return AddModel<T_template>(model);

            }
            else
            {
                return ModifiedModel<T_template>(model, false, model.NotUpdatefield);
            }

        }
        public SysResult<IList<T_basicsType>> Querylisttype(T_basicsType model, OrderablePagination orderablePagination)
        {
            SysResult<IList<T_basicsType>> sysresult = new SysResult<IList<T_basicsType>>();
            var data = from a in basicsType select a;
            Expression<Func<T_basicsType, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
           
            data = data.Where(where);
            IOrderByExpression<T_basicsType> order = new OrderByExpression<T_basicsType, decimal>(p => p.Id, true);
            sysresult.numberData = this.QueryForList<T_basicsType>(this.basicsType, where, orderablePagination, order);
            sysresult.numberCount = data.Count();
            return sysresult;
        }
        public SysResult<IList<T_V_basicc>> Querylistdata(T_V_basicc model, OrderablePagination orderablePagination)
        {
            SysResult<IList<T_V_basicc>> sysresult = new SysResult<IList<T_V_basicc>>();
            
            Expression<Func<T_V_basicc, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
          
            IOrderByExpression<T_V_basicc> order = new OrderByExpression<T_V_basicc, decimal>(p => p.Id, true);
            sysresult.numberData = this.QueryForList<T_V_basicc>(this.t_v_basic, where, orderablePagination, order);
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<IList<T_basicsType>> Querylist(T_basicsType model, OrderablePagination orderablePagination)
        {
            SysResult<IList<T_basicsType>> sysresult = new SysResult<IList<T_basicsType>>();
            var data = from a in basicsType select a;
            Expression<Func<T_basicsType, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }

            data = data.Where(where);
            IOrderByExpression<T_basicsType> order = new OrderByExpression<T_basicsType, decimal>(p => p.Id, true);
            sysresult.numberData = this.QueryForList<T_basicsType>(this.basicsType, where, orderablePagination, order);
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public List<T_basicsType> Querylisttype(T_basicsType model)
        {
            SysResult<IList<T_basicsType>> sysresult = new SysResult<IList<T_basicsType>>();
            var data = from a in basicsType select a;
           
          
            return data.ToList();
        }
        public List<T_Basics> Querylistbytype(T_Basics model)
        {
            var data = from a in BbBasic  select a;
            Expression<Func<T_Basics, bool>> where = m => 1 == 1;
            if (model.Code != null)
            {
                where = where.And(m => m.Code == model.Code);
            }
            data = data.Where(where);
            return data.ToList();
        }
        public List<T_Basics> Querylist1(Queryparam model)
        {
            var data = from a in BbBasic where a.IsActive == 1 select a;
            if (model!=null&&model.paratype != null)
            {
                data= from a in BbBasic where  a.ParaType==model.paratype&&a.IsActive==1 select a;
            }
            
            return data.ToList();
        }

        public T_Basics Querylist2(T_Basics model)
        {
            var data = from a in BbBasic select a;
            if (model != null && model.Code != null)
            {
                data = from a in BbBasic where a.Code == model.Code && a.IsActive == 1 select a;
            }
            return data.FirstOrDefault();
        }
        public T_Basics QueryId(T_Basics model)
        {
            var data = from a in BbBasic where a.Id==model.Id select a;
            return data.FirstOrDefault();
        }
        public T_basicsType QueryTypeId(T_basicsType model)
        {
            var data = from a in basicsType where a.Id == model.Id select a;
            return data.FirstOrDefault();
        }
        //public List<T_Tese> Querylisttese(Queryparam model)
        //{
        //    var data = from a in Bbtese select a;
        //    if (model != null && model.type != 0)
        //    {
        //        data = from a in Bbtese where a.Type == model.type select a;
        //    }
        //    return data.ToList();
        //}
        public long Savedata(T_Basics model)
        {
            model.Id = GetNextValNum("GET_WSEQUENCES('T_BASICS')");
            AddModel(model);
            return model.Id;
        }
        public long SaveTypedata(T_basicsType model)
        {
            model.Id = GetNextValNum("GET_WSEQUENCES('T_BASICSTYPE')");
            AddModel(model);
            return model.Id;
        }
        public int Updatedata(T_Basics model)
        {
            return ModifiedModel(model,false); ;
        }
        public int UpdateTypedata(T_basicsType model)
        {
            return ModifiedModel(model, false); ;
        }
        public long deletedata(long model)
        {
            T_Basics basc = new T_Basics();
            basc.Id = model;
            DeleteModel(basc);
            return basc.Id;
        }
        public int QueryCount(string code)
        {
            return (from m in BbBasic where m.ParaType==code select m).Count();
        }
        public List<peipei> getpeibei(HousePendent pent)
        {
            PeibeiTeseDal tese = new DAL.PeibeiTeseDal();
            Queryparam parambase = new Queryparam();
          
            parambase.paratype = "privatepeibei";
            List<T_Basics> list =Querylist1(parambase);
            List<peipei> listpeibei = new List<peipei>();
            List<peipei> arr =new List<peipei>();
            arr = tese.QueryPeibei(pent.ID,(int)type.hezuprivate);
            foreach (var mo in list)
            {
                peipei pe = new peipei();
                pe.Name = mo.Name;
                pe.isCheck = 0;
                if (arr.Select(m=>m.Name==mo.Name).Count()>0)
                {
                    pe.isCheck = 1;
                }
                listpeibei.Add(pe);
            }
            return listpeibei;
        }
        public int savebill(t_tuoguan bill)
        {
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_TUOGUAN')");
                return AddModel<t_tuoguan>(bill);

            }
            else
            {
                return ModifiedModel<t_tuoguan>(bill, false, bill.NotUpdatefield);
            }

        }


        public long saveaccount(T_account model,params string[] str)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_ACCOUNT')");
                model.CompanyId = model.Id;
                AddModel<T_account>(model);
                return model.CompanyId;
            }
            else
            {
                return ModifiedModel1<T_account>(model, false, str);
            }

        }
        public long saveaccount1(T_account model, params string[] str)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_ACCOUNT')");
                model.CompanyId = model.Id;
                AddModel<T_account>(model);
                return model.CompanyId;
            }
            else
            {
                return ModifiedModel<T_account>(model, false, str);
            }

        }

        public T_account queryaccount(long companyid)
        {
            var data = from m in account where m.CompanyId== companyid select m;
            return data.FirstOrDefault();
        }
        public List<T_Tese> gettese(HousePendent pent)
        {
            PeibeiTeseDal tese = new DAL.PeibeiTeseDal();
            Queryparam parambase = new Queryparam();

            parambase.paratype = "privatetese";
            List<T_Basics> list = Querylist1(parambase);
            List<T_Tese> listpeibei = new List<T_Tese>();
            List<T_Tese> arr = new List<T_Tese>();
            arr = tese.QueryTese(pent.ID, (int)type.hezuprivate);
            foreach (var mo in list)
            {
                T_Tese pe = new T_Tese();
                pe.Name = mo.Name;
                pe.IsCheck = 0;
              
                if (arr.Select(m => m.Name == mo.Name).Count() > 0)
                {
                    pe.IsCheck = 1;
                }
                listpeibei.Add(pe);
            }
            return listpeibei;
        }

        public DbSet<t_tuoguan> Bbtuoguan { get; set; }
        public DbSet<T_Basics> BbBasic { get; set; }
        public DbSet<T_basicsType> basicsType { get; set; }
        public DbSet<T_V_basicc> t_v_basic { get; set; }

        public DbSet<T_bankcard> bankcard { get; set; }
        public DbSet<T_account> account { get; set; }

        public DbSet<T_template> template { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new tuoguanMaping());
            modelBuilder.Configurations.Add(new BaseDataMaping());
            modelBuilder.Configurations.Add(new T_basticTypeMaping());
            modelBuilder.Configurations.Add(new T_v_basticTypeMaping());
            modelBuilder.Configurations.Add(new bankcardMaping());
            modelBuilder.Configurations.Add(new accountMaping());
            modelBuilder.Configurations.Add(new templateMaping());
        }
    }
}
