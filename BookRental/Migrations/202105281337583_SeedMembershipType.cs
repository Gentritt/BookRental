namespace BookRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedMembershipType : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [dbo].[MembershipTypes](Name,SignUpFee,ChargeRateOneMonth,ChargeRateSixMonth) VALUES ('PAY per rental',0,50,25)");
            Sql("INSERT INTO [dbo].[MembershipTypes](Name,SignUpFee,ChargeRateOneMonth,ChargeRateSixMonth) VALUES ('Member',15,10,25)");
            Sql("INSERT INTO [dbo].[MembershipTypes](Name,SignUpFee,ChargeRateOneMonth,ChargeRateSixMonth) VALUES ('Admin',0,0,0)");
        }
        
        public override void Down()
        {
        }
    }
}
