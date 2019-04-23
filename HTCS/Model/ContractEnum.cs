using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class CEnum
    {
        public int key { get; set; }
        public string Value { get; set; }
    }
    public class WrapEnum
    {
        public int onlinesign { get; set; }
        public string onlinesignstr { get; set; }
        public List<CEnum> paytype { get; set; }
        public List<CEnum> type { get; set; }
        public List<CEnum> pinlv { get; set; }
        public List<CEnum> zafeipinlv { get; set; }
        public List<CEnum> work { get; set; }
        public List<CEnum> Hobby { get; set; }
        public List<T_ZafeiList> zafei { get; set; }
      
    }
}
