using Model.TENANT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Contrct
{
    public class paraCreate
    {
        public long Id { get; set; }
        public int HouseType { get; set; }
        public DateTime BeginTime { get; set; }
        public decimal Recent { get; set; }
        public DateTime EndTime { get; set; }

        public int PinLv { get; set; }

        public int Recivetype { get; set; }

        public int BeforeDay { get; set; }

        public long HouseId { get; set; }

        public long TeantId { get; set; }

        public long CreatePerson { get; set; }

        public int Type { get; set; }

        public long CompanyId { get; set; }

        public string CreatePersonstr { get; set; }

        public T_Teant Teant { get; set; }
        public List<T_Otherfee> Otherfee { get; set; }

        public List<T_Otherfee> Yajin { get; set; }
    }
}
