namespace BookRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToMembershipTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Firstname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Lastname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Birthdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Disable", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "MembershipTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MembershipTypeId");
            DropColumn("dbo.AspNetUsers", "Disable");
            DropColumn("dbo.AspNetUsers", "Birthdate");
            DropColumn("dbo.AspNetUsers", "Lastname");
            DropColumn("dbo.AspNetUsers", "Firstname");
        }
    }
}
