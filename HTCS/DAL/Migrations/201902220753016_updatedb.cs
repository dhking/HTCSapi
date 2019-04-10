namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SYSTEM.T_OMSREGION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        REGIONNAME = c.String(),
                        REGTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISREMEN = c.Decimal(nullable: false, precision: 10, scale: 0),
                        COMPANYID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("SYSTEM.T_OMSREGION");
        }
    }
}
