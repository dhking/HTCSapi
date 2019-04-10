using Model;
using Model.Base;
using Model.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mapping.cs
{
    public class ButtonMapping : BaseEntityTypeMap<T_Button>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_SYSBUTTON");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.BtnName).HasColumnName("BTNNAME");
            Property(m => m.BtnNo).HasColumnName("BTNNO");
            Property(m => m.MenuId).HasColumnName("MENUID");
            Property(m => m.ButtonUrl).HasColumnName("BUTTONURL");
            Property(m => m.BtnIcon).HasColumnName("BTNICON");
            Property(m => m.orderby).HasColumnName("ORDERBY");
            Property(m => m.Multiple).HasColumnName("MULTIPLE");
            Property(m => m.btnjishu).HasColumnName("JISHU");
        }
    }
}
