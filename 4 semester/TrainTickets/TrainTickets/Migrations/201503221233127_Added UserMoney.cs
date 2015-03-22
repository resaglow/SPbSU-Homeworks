namespace TrainTickets.MigrationsUsers
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserMoney : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserMoney", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserMoney");
        }
    }
}
