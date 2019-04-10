using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Server
    {
        public string server { get; set; }
        public int port { get; set; }
        public string scheduler { get; set; }

        public string Name { get; set; }
        public string Group { get; set; }
        public string op { get; set; }
    }
    public class GroupStatus
    {
        public string Group { get; set; }
        public bool IsJobGroupPaused { get; set; }
        public bool IsTriggerGroupPaused { get; set; }
    }
    public class SysParaModel : BasicModel
    {
        public long Id { get; set; }
        /// <summary>
        /// SysParaTypeId
        /// </summary>
        //[XmlElement("SysParaTypeId")]
        public Int32? SysParaTypeId { get; set; }

        /// <summary>
        /// ParaName
        /// </summary>
        //[XmlElement("ParaName")]
        public String ParaName { get; set; }

        /// <summary>
        /// ParaDesc
        /// </summary>
        //[XmlElement("ParaDesc")]
        public String ParaDesc { get; set; }

        /// <summary>
        /// ParaValue
        /// </summary>
        //[XmlElement("ParaValue")]
        public String ParaValue { get; set; }

        /// <summary>
        /// IsActive
        /// </summary>
        //[XmlElement("IsActive")]
        public Boolean? IsActive { get; set; }

        /// <summary>
        /// OwnerId
        /// </summary>
        //[XmlElement("OwnerId")]
        public String OwnerId { get; set; }

        /// <summary>
        /// ModifierId
        /// </summary>
        //[XmlElement("ModifierId")]
        public String ModifierId { get; set; }

        /// <summary>
        /// CreationDate
        /// </summary>
        //[XmlElement("CreationDate")]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// ModifiedDate
        /// </summary>
        //[XmlElement("ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// OrderBy
        /// </summary>
        //[XmlElement("OrderBy")]
        public Int32? OrderBy { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        //[XmlElement("Remark")]
        public String Remark { get; set; }

        /// <summary>
        /// DisplayType
        /// </summary>
        //[XmlElement("DisplayType")]
        public Int32? DisplayType { get; set; }

        /// <summary>
        /// DataOption
        /// </summary>
        //[XmlElement("DataOption")]
        public String DataOption { get; set; }

        /// <summary>
        /// DefaultValue
        /// </summary>
        //[XmlElement("DefaultValue")]
        public String DefaultValue { get; set; }

    }
    /// <summary>
	///  SysAutoTaskModel 
	/// </summary>
	public class SysAutoTaskModel :BasicModel
    {
        public long CompanyId { get; set; }
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the JobName
        /// </summary>
        public String JobName { get; set; }

        /// <summary>
        /// Gets or sets the JobGroup
        /// </summary>
        public String JobGroup { get; set; }

        /// <summary>
        /// Gets or sets the JobDesc
        /// </summary>
        public String JobDesc { get; set; }

        /// <summary>
        /// Gets or sets the JobSpName
        /// </summary>
        public String JobSpName { get; set; }

        /// <summary>
        /// Gets or sets the JobClassName
        /// </summary>
        public String JobClassName { get; set; }

        /// <summary>
        /// Gets or sets the TotalCount
        /// </summary>
        public Int32? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the TotalSeconds
        /// </summary>
        public Int32? TotalSeconds { get; set; }

        /// <summary>
        /// Gets or sets the JobPara1
        /// </summary>
        public String JobPara1 { get; set; }

        /// <summary>
        /// Gets or sets the JobPara2
        /// </summary>
        public String JobPara2 { get; set; }

        /// <summary>
        /// Gets or sets the JobStatus
        /// </summary>
        public Byte? JobStatus { get; set; }

        /// <summary>
        /// Gets or sets the LastExecStatus
        /// </summary>
        public Byte? LastExecStatus { get; set; }

        /// <summary>
        /// Gets or sets the LastExecMessage
        /// </summary>
        public String LastExecMessage { get; set; }

        /// <summary>
        /// Gets or sets the LastExecDate
        /// </summary>
        public DateTime? LastExecDate { get; set; }

        /// <summary>
        /// Gets or sets the SysAutoTaskTriggerId
        /// </summary>
        public Int32? SysAutoTaskTriggerId { get; set; }

        /// <summary>
        /// Gets or sets the IsCanMultiThread
        /// </summary>
        public Boolean? IsCanMultiThread { get; set; }

        /// <summary>
        /// Gets or sets the IsActive
        /// </summary>
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Gets or sets the OwnerId
        /// </summary>
        public String OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ModifierId
        /// </summary>
        public String ModifierId { get; set; }

        /// <summary>
        /// Gets or sets the CreationDate
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

    }

    public enum JobStatus
    {
        未执行 = 1,
        执行中 = 2,
        执行完成 = 3
    }

    public enum ExecStatus
    {
        成功 = 1,
        失败 = 2,
        终止 = 3//如果对于不允许多线程执行,但上一次任务没有完成,则我们认为是终止
    }
}
