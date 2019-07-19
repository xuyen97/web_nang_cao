namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class suahiatrongtour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "Gia", c => c.Double(nullable: false));
            DropColumn("dbo.Tours", "Giá");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tours", "Giá", c => c.Double(nullable: false));
            DropColumn("dbo.Tours", "Gia");
        }
    }
}
