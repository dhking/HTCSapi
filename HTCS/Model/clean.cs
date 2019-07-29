using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class clean : BasicModel
    {
        public long Id { get; set; }

        public DateTime expectedtime { get; set; }

        public long houseid { get; set; }
        [NotMapped]
        public string content { get; set; }
        public int status { get; set; }
        public int ugent { get; set; }
        public DateTime apply { get; set; }

        public DateTime appointment { get; set; }

        public string applyperson { get; set; }
       

        public string phone { get; set; }

        public string project { get; set; }

        public string remark { get; set; }

        public long executor { get; set; }

        public long companyid { get; set; }
    }

    public class wrapcleanRZ : BasicModel
    {
        public long Id { get; set; }

        public DateTime createtime { get; set; }

        public long cleanid { get; set; }

        public string content { get; set; }

        public long opera { get; set; }
        [NotMapped]
        public string operastr { get; set; }
        public long companyid { get; set; }
    }
    public class cleanRZ : BasicModel
    {
        public long Id { get; set; }

        public DateTime createtime { get; set; }

        public long cleanid { get; set; }

        public string content { get; set; }

        public long opera { get; set; }
        [NotMapped]
        public string  operastr { get; set; }
        public long companyid { get; set; }
    }

    public class Wrapclean:BasicModel
    {
        public long Id { get; set; }

        public long houseid { get; set; }
        public DateTime expectedtime { get; set; }
        public long companyid { get; set; }
        public string content { get; set; }
        public string house { get; set; }

        public string cityname { get; set; }
        public string cellname { get; set; }
        public string areaname { get; set; }
        public int status { get; set; }
        public int ugent { get; set; }

        public int RecentType { get; set; }
        
        public DateTime apply { get; set; }

        public DateTime appointment { get; set; }

       

        public string applyperson { get; set; }

        public string phone { get; set; }

        public string project { get; set; }

        public string remark { get; set; }

        public long executor { get; set; }
        public string executorstr { get; set; }
       
        public string CellNames { get; set; }

        public string[] arrCellNames { get; set; }
        public List<cleanRZ> listcleanRZ { get; set; }
    }
}
