using DAL;
using Model;
using Model.Map;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class MapService
    {
        MapDAL dal = new MapDAL();
        public SysResult<List<tip>> Query(QueryMap param)
        {
          return  dal.Query(param);
        }
        public SysResult<List<pois>> Querymap(QueryMap param)
        {
            return dal.Querymap(param);
        }
        public SysResult<QueryArea> Querymap(QueryArea model)
        {
            SysResult<QueryArea> result = new SysResult<QueryArea>();
            MapDAL dal = new MapDAL();
            QueryArea area = dal.Querymap1(model);
            if (area.status == 1)
            {
                area.districts.FirstOrDefault().districts1.Insert(0, new districts() { citycode = "", name = "全部" });
                foreach (var mo in area.districts.FirstOrDefault().districts1)
                {
                    if (mo.districts1 != null)
                    {
                        mo.districts1.Insert(0, new districts() { citycode = "", name = "全部" });
                        foreach (var mo1 in mo.districts1)
                        {
                            if (mo1.districts1 != null)
                            {
                                mo1.districts1.Insert(0, new districts() { citycode = "", name = "全部" });
                            }
                        }
                    }

                }
            }
            if (area.status != 1)
            {
                result.Code = area.status;
                result.Message = area.info;

            }
            else
            {
                result.Code = 0;
                result.numberData = area;
            }
            return result;
        }

        public SysResult<List<districts>> PCQuerymap(QueryArea model)
        {
            SysResult<List<districts>> result = new SysResult<List<districts>>();
            MapDAL dal = new MapDAL();
            districts area = dal.Querymap2(model);
            if (area.status != 1)
            {
                result.Code = area.status;
                result.Message = area.info;
            }
            else
            {
                if (area.districts1.FirstOrDefault().level == "province")
                {
                    districts newarea = area.districts1.FirstOrDefault();
                    if (newarea != null)
                    {
                        result.numberData = newarea.districts1;
                        result.Code = 0;
                        return result;
                    }
                }
                result.Code = 0;
                result.numberData = area.districts1;
            }
            return result;
        }

    }
}
