using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Bill
{
    public class PlAction<T,T1>
    {
        public List<T> dataadd { get; set; }
        public List<T> update { get; set; }
        public List<T> delete { get; set; }
        public T1  main{get;set;}
    }
}
