using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.Menu;
using Mapping.cs.Menu;
using Model.User;
using Mapping.cs;
using Model;
using Model.House;
using System.Linq.Expressions;
using DBHelp;

namespace DAL
{
    public  class kjxDAL : RcsBaseDao
    {
        public List<Wraplocklist> query(List<locklist> list,long houseid,int SearchType)
        {
            var data = from m in HouseQuery select m;
            Expression<Func<HouseLockQuery, bool>> where = m => 1 == 1;
            if (houseid != 0)
            {
                where = where.And(m => m.Id == houseid);
            }
            data = data.Where(where);
            if (SearchType == 1)
            {
                var mo = from m in list join n in data on m.lockId equals n.LocalId into temp from t in temp select new Wraplocklist() { id = m.id, lockId = m.lockId, date = m.date, lockName = m.lockName, lockAlias = m.lockAlias, lockMac = m.lockMac, electricQuantity = m.electricQuantity, keyboardPwdVersion = m.keyboardPwdVersion, specialValue = m.specialValue, HouseName = t == null ? "没有绑定房间" : t.Name, HouseId = t == null ? 0 : t.Id, HouseType = t == null ? 0 : t.RecentType };
                return mo.ToList();
            }
            else
            {
                var mo = from m in list join n in data on m.lockId equals n.LocalId into temp from t in temp.DefaultIfEmpty() select new Wraplocklist() { id = m.id, lockId = m.lockId, date = m.date, lockName = m.lockName, lockAlias = m.lockAlias, lockMac = m.lockMac, electricQuantity = m.electricQuantity, keyboardPwdVersion = m.keyboardPwdVersion, specialValue = m.specialValue, HouseName = t == null ? "没有绑定房间" : t.Name, HouseId = t == null ? 0 : t.Id, HouseType = t == null ? 0 : t.RecentType };
                return mo.ToList();
            }
           
          
            
        }
        //租客端绑定房间
        public List<Wraplocklist> zkquery(List<locklist> list,long HouseId)
        {
            var mo = from m in list join n in HouseQuery  on m.lockId equals n.LocalId into temp  from t in temp where t.Id== HouseId select new Wraplocklist() { id = m.id, lockId = m.lockId, date = m.date, lockName = m.lockName, lockAlias = m.lockAlias, lockMac = m.lockMac, electricQuantity = m.electricQuantity, keyboardPwdVersion = m.keyboardPwdVersion, specialValue = m.specialValue, HouseName = t == null ? "没有绑定房间" : t.Name, HouseId = t == null ? 0 : t.Id, HouseType = t == null ? 0 : t.RecentType };
            return mo.ToList();
        }
        public int QueryCount(string LocalId)
        {
            var mo = (from m in HouseQuery where m.LocalId == LocalId select m).AsNoTracking();
            return mo.Count();
        }

        public int QueryCount1(string elecid)
        {
            var mo = (from m in HouseQuery where m.ElecId == elecid select m).AsNoTracking();
            return mo.Count();
        }
        public HouseLockQuery Querybylocal(string LocalId)
        {
            var mo = from m in HouseQuery where m.LocalId == LocalId select m;
            return mo.FirstOrDefault();
        }
        public long SaveorUpdate(UserKey model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('TZ_USERKEY')");
                AddModel(model);
            }
            else
            {
                ModifiedModel1(model, false, new string[] { "KeyId" });
            }
            return model.Id;
        }
        //通过房源编号查询锁列表
        public List<HouseLockQuery> Querylockbyhouse(long Houseid)
        {
            List<HouseLockQuery> list = new List<HouseLockQuery>();
            HouseLockQuery list1 = new HouseLockQuery();
            list1= (from m in HouseQuery where m.Id == Houseid &&m.LocalId!=null select m).FirstOrDefault();
            if (list1 != null)
            {
                HouseLockQuery list2 = new HouseLockQuery();
                var mo = from m in HouseQuery where m.Id == list1.ParentId && m.LocalId != null select m;
                list2 = mo.FirstOrDefault();
                list.Add(list1);
                if (list2 != null)
                {
                    list.Add(list2);
                }
            }
            return list;
        }
        public DbSet<HouseLockQuery> HouseQuery { get; set; }
        public DbSet<T_kjx> Bbkjx { get; set; }
        public DbSet<UserKey> BbUserKey { get; set; }

        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new kjxMapping());
            modelBuilder.Configurations.Add(new HouseQueryLockMapping());
            modelBuilder.Configurations.Add(new UserKeyMapping());
        }
    }
}
