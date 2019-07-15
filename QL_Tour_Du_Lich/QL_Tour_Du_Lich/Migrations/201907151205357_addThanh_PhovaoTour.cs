namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addThanh_PhovaoTour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "Thanh_Pho", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "Thanh_Pho");
        }
    }
}
