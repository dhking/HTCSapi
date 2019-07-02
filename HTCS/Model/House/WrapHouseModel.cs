using Model.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class dtmode
    {
        public DateTime dt { get; set; }

        public DateTime dt2 { get; set; }
    }
    public   class WrapHouseModel
    {
        //套数
        public int count { get; set; }
        //间数
        public int count1 { get; set; }
        public WrapHouseModel1 house { get; set; }

        public List<WrapHousePendent> housependent { get; set; }
    }
    public class WrapHouse
    {
        public HouseModel house { get; set; }

        public HousePendent housependent { get; set; }
    }
    public class WrapIndentHouse
    {
        public long Id { get; set; }
        public string tongji { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public List<T_Floor> listfloor { get; set; }
    }

    public class WrapPCIndentHouse
    {
        public long Id { get; set; }

        public int Floor { get; set; }

        public long ParentId { get; set; }


        public List<HouseModel> ListHouseModel { get; set; }
    }
}
