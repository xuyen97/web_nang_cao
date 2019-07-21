namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addemailcthd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chi_Tiet_Hoa_Don", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chi_Tiet_Hoa_Don", "Email");
        }
    }
}
