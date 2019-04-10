using Model;
using Model.Base;
using Model.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs.Menu
{
    public class MenuMapping : BaseEntityTypeMap<T_Menu>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_MENU");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.title).HasColumnName("TITLE");
            Property(m => m.icon).HasColumnName("ICON");
            Property(m => m.sign).HasColumnName("SIGN");
            Property(m => m.Jishu).HasColumnName("JISHU");
            Property(m => m.ParentId).HasColumnName("PARENTID");
            Property(m => m.SystemId).HasColumnName("SYSTEMID");
            Property(m => m.jump).HasColumnName("JUMP");
            Property(m => m.orderby).HasColumnName("ORDERBY");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class RoleMenuMapping : BaseEntityTypeMap<T_SysRoleMenu>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_SYSROLEMENU");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.ButtonId).HasColumnName("BUTTONID");
            Property(m => m.RoleId).HasColumnName("ROLEID");
            Property(m => m.MenuId).HasColumnName("MENUID");
            Property(m => m.Ptype).HasColumnName("PTYPE");
        }
    }
}
