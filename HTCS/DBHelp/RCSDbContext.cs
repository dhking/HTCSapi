using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public abstract class RCSDbContext : DbContext
    {
        public RCSDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }
        public RCSDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {

        }

        public RCSDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {

        }

        public RCSDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)
        {

        }

        public RCSDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {

        }

        protected abstract void CreateModelMap(DbModelBuilder modelBuilder);
    }
}
