namespace BookRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookRentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookRents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        BookId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        ActualEndDate = c.DateTime(),
                        ScheduleEndDate = c.DateTime(),
                        AdditionalCharge = c.Double(),
                        Price = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookRents");
        }
    }
}
