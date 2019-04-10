using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayOpenPublicPayeeBindDeleteModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOpenPublicPayeeBindDeleteModel : AopObject
    {
        /// <summary>
        /// 收款账号，需要解除绑定的收款支付宝账号
        /// </summary>
        [XmlElement("login_id")]
        public string LoginId { get; set; }
    }
}
