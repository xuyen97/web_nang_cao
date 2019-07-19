namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addgiatour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "Giá", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "Giá");
        }
    }
}
