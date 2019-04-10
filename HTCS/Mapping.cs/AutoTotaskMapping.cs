using Model;
using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class AutoTotaskMapping : BaseEntityTypeMap<SysAutoTaskModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_SYSAUTOTASK");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.JobName).HasColumnName("JOBNAME");
            Property(m => m.JobGroup).HasColumnName("JOBGROUP");
            Property(m => m.JobDesc).HasColumnName("JOBDESC");
            Property(m => m.JobSpName).HasColumnName("JOBSPNAME");
            Property(m => m.JobClassName).HasColumnName("JOBCLASSNAME");
            Property(m => m.TotalCount).HasColumnName("TOTALCOUNT");
            Property(m => m.TotalSeconds).HasColumnName("TOTALSECONDS");
            Property(m => m.JobPara1).HasColumnName("JOBPARA1");
            Property(m => m.JobPara2).HasColumnName("JOBPARA2");
            Property(m => m.JobStatus).HasColumnName("JOBSTATUS");
            Property(m => m.LastExecStatus).HasColumnName("LASTEXECSTATUS");
            Property(m => m.LastExecMessage).HasColumnName("LASTEXECMESSAGE");
            Property(m => m.LastExecDate).HasColumnName("LASTEXECDATE");
            Property(m => m.SysAutoTaskTriggerId).HasColumnName("SYSAUTOTASKTRIGGERID");
            Property(m => m.IsCanMultiThread).HasColumnName("ISCANMULTITHREAD");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.OwnerId).HasColumnName("OWNERID");
            Property(m => m.ModifierId).HasColumnName("MODIFIERID");
            Property(m => m.CreationDate).HasColumnName("CREATIONDATE");
            Property(m => m.ModifiedDate).HasColumnName("MODIFIEDDATE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
    public class AutoTotaskHistoryMapping : BaseEntityTypeMap<SysAutoTaskHistoryModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_SYSAUTOTASKHISTORY");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.SysAutoTaskId).HasColumnName("SYSAUTOTASKID");
            Property(m => m.ExecStatus).HasColumnName("EXECSTATUS");
            Property(m => m.ExecMessage).HasColumnName("EXECMESSAGE");
            Property(m => m.TotalSeconds).HasColumnName("TOTALSECONDS");
            Property(m => m.JobPara1).HasColumnName("JOBPARA1");
            Property(m => m.JobPara2).HasColumnName("JOBPARA2");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.OwnerId).HasColumnName("OWNERID");
            Property(m => m.CreationDate).HasColumnName("CREATIONDATE");
            Property(m => m.InstanceId).HasColumnName("INSTANCEID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class AutoTotaskServiceMapping : BaseEntityTypeMap<SysAutoTaskServiceModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_SYSAUTOTASKSERVICE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.ShowName).HasColumnName("SHOWNAME");
            Property(m => m.ServerIp).HasColumnName("SERVERIP");
            Property(m => m.Port).HasColumnName("PORT");
            Property(m => m.Scheduler).HasColumnName("SCHEDULER");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.OwerId).HasColumnName("OWNERID");
            Property(m => m.ModifierId).HasColumnName("MODIFIERID");
            Property(m => m.CreationDATE).HasColumnName("CREATIONDATE");
            Property(m => m.ModifidDate).HasColumnName("MODIFIEDDATE");
            Property(m => m.ShopId).HasColumnName("SHOPID");
            Property(m => m.ServerName).HasColumnName("SERVERNAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class AutoTotaskTrigerMapping : BaseEntityTypeMap<SysAutoTaskTriggerModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_SYSAUTOTASKTRIGGER");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.ShowName).HasColumnName("SHOWNAME");
            Property(m => m.CronExpression).HasColumnName("CRONEXPRESSION");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.OwnerId).HasColumnName("OWNERID");
            Property(m => m.ModifierId).HasColumnName("MODIFIERID");
            Property(m => m.CreationDate).HasColumnName("CREATIONDATE");
            Property(m => m.ModifiedDate).HasColumnName("MODIFIEDDATE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
}
