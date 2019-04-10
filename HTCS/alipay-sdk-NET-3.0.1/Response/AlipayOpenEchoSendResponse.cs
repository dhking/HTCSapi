using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayOpenEchoSendResponse.
    /// </summary>
    public class AlipayOpenEchoSendResponse : AopResponse
    {
        /// <summary>
        /// hello world
        /// </summary>
        [XmlElement("out_a")]
        public string OutA { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [XmlElement("out_b")]
        public long OutB { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [XmlElement("out_c")]
        public string OutC { get; set; }

        /// <summary>
        /// hello world
        /// </summary>
        [XmlElement("word")]
        public string Word { get; set; }
    }
}
