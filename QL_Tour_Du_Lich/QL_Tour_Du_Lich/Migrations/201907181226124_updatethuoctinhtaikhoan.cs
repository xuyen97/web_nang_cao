namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatethuoctinhtaikhoan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tai_Khoan", "Nguoi_Dung_Id", "dbo.Nguoi_Dung");
            DropIndex("dbo.Tai_Khoan", new[] { "Nguoi_Dung_Id" });
            CreateIndex("dbo.Tai_Khoan", "Loai_Nguoi_Dung_Id");
            AddForeignKey("dbo.Tai_Khoan", "Loai_Nguoi_Dung_Id", "dbo.Loai_Nguoi_Dung", "Loai_Nguoi_Dung_Id", cascadeDelete: true);
            DropColumn("dbo.Tai_Khoan", "Nguoi_Dung_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tai_Khoan", "Nguoi_Dung_Id", c => c.Int());
            DropForeignKey("dbo.Tai_Khoan", "Loai_Nguoi_Dung_Id", "dbo.Loai_Nguoi_Dung");
            DropIndex("dbo.Tai_Khoan", new[] { "Loai_Nguoi_Dung_Id" });
            CreateIndex("dbo.Tai_Khoan", "Nguoi_Dung_Id");
            AddForeignKey("dbo.Tai_Khoan", "Nguoi_Dung_Id", "dbo.Nguoi_Dung", "Id");
        }
    }
}
