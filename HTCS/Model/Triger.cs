using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class JobSchedule
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public string TriggerType { get; set; }
        public string TriggerState { get; set; }

        public string Server { get; set; }
        public string Port { get; set; }
        public string Scheduler { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime NextFire { get; set; }
        public DateTime LastFire { get; set; }
    }
    public class SysAutoTaskHistoryModel : BasicModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the SysAutoTaskId
        /// </summary>
        public Int32? SysAutoTaskId { get; set; }

        /// <summary>
        /// Gets or sets the ExecStatus
        /// </summary>
        public Byte? ExecStatus { get; set; }
        public long CompanyId { get; set; }
        /// <summary>
        /// Gets or sets the ExecMessage
        /// </summary>
        public String ExecMessage { get; set; }

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
        /// Gets or sets the IsActive
        /// </summary>
        public Boolean? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the OwnerId
        /// </summary>
        public String OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the CreationDate
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the InstanceId
        /// </summary>
        public String InstanceId { get; set; }

    }
    public class SysAutoTaskTriggerModel:BasicModel 
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        /// <summary>
        /// Gets or sets the ShowName
        /// </summary>
        public String ShowName { get; set; }

        /// <summary>
        /// Gets or sets the CronExpression
        /// </summary>
        public String CronExpression { get; set; }

        /// <summary>
        /// Gets or sets the IsActive
        /// </summary>
        public Boolean? IsActive { get; set; }

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
    public class SysAutoTaskServiceModel : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        /// <summary>
        /// Gets or sets the ShowName
        /// </summary>
        public String ShowName { get; set; }

        /// <summary>
        /// Gets or sets the CronExpression
        /// </summary>
        public String ServerIp { get; set; }

        /// <summary>
        /// Gets or sets the IsActive
        /// </summary>
        public String Port { get; set; }

        /// <summary>
        /// Gets or sets the OwnerId
        /// </summary>
        public String Scheduler { get; set; }

        /// <summary>
        /// Gets or sets the ModifierId
        /// </summary>
        public int IsActive { get; set; }

        /// <summary>
        /// Gets or sets the CreationDate
        /// </summary>
        public String OwerId { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate
        /// </summary>
        public String ModifierId { get; set; }

        public DateTime? CreationDATE { get; set; }

        /// <summary>
        /// Gets or sets the CreationDate
        /// </summary>
        public DateTime? ModifidDate{ get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate
        /// </summary>
        public long? ShopId { get; set; }
        public string  ServerName { get; set; }

        
       
    }
}
