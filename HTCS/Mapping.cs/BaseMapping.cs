using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public abstract class BaseEntityTypeMap<TEntity> : EntityTypeMap<TEntity> where TEntity : Model.BasicModel
    {
        public BaseEntityTypeMap()
        {
       
        }
       
    }

    public abstract class BaseTaskEntityTypeMap<TEntity> : EntityTypeMap<TEntity> where TEntity : class
    {

    }
    public abstract class EntityTypeMap<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : class
    {
        public EntityTypeMap()
        {
            this.IniMaps();
        }

        protected abstract void IniMaps();
    }
}
