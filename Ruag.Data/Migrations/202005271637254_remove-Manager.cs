namespace Ruag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeManager : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "ManagerId", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "ManagerId" });
            DropColumn("dbo.Employees", "ManagerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "ManagerId", c => c.Int());
            CreateIndex("dbo.Employees", "ManagerId");
            AddForeignKey("dbo.Employees", "ManagerId", "dbo.Employees", "Id");
        }
    }
}
