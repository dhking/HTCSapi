using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public abstract class PagingModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("pageindex")]
        [NotMapped]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        [JsonProperty("pagesize")]
        [NotMapped]
        public int PageSize { get; set; }
       
    }
  
}
