namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class themthuoctinhtrongtour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "Gioi_Thieu", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "Gioi_Thieu");
        }
    }
}
