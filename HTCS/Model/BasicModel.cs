using Model.User;
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
    public abstract class BasicModel : PagingModel
    {
        [NotMapped]
        public string access_token { get; set; }
        [NotMapped]
        public string[] NotUpdatefield { get; set; }
        ///// <summary>
        ///// 存储过程名称
        ///// </summary>
        //[JsonProperty("Spname")]
        //[NotMapped]
        //public String Spname { get; set; }
        ///// <summary>
        ///// 最后修改时间
        ///// </summary>
        //[JsonProperty("lastModifyTime")]
        //public Nullable<System.DateTime> LastModifyTime { get; set; }
        ///// <summary>
        ///// 创建人
        ///// </summary>
        //[JsonProperty("creator")]
        //public String Creator { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //[JsonProperty("createTime")]
        //public Nullable<System.DateTime> CreateTime { get; set; }

        //private String _validity = "1"; // 有效性
        ///// <summary>
        ///// 有效性
        ///// </summary>
        //[JsonProperty("validity")]
        //public String Validity
        //{
        //    get
        //    {
        //        return _validity;
        //    }
        //    set
        //    {
        //        _validity = value;
        //    }
        //}
    }
}
