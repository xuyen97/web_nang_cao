namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecthd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chi_Tiet_Hoa_Don", "Trang_Thai", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chi_Tiet_Hoa_Don", "Trang_Thai");
        }
    }
}
