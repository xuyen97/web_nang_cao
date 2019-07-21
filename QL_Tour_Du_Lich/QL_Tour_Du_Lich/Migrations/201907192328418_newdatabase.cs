namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newdatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chi_Tiet_Hoa_Don", "Hoa_Don_Id", "dbo.Hoa_Don");
            DropForeignKey("dbo.Nguoi_Dung", "Loai_Nguoi_Dung_Id", "dbo.Loai_Nguoi_Dung");
            DropIndex("dbo.Chi_Tiet_Hoa_Don", new[] { "Hoa_Don_Id" });
            DropIndex("dbo.Nguoi_Dung", new[] { "Loai_Nguoi_Dung_Id" });
            AddColumn("dbo.Chi_Tiet_Hoa_Don", "SoLuong", c => c.Int(nullable: false));
            AddColumn("dbo.Tai_Khoan", "Email", c => c.String(nullable: false));
            DropColumn("dbo.Chi_Tiet_Hoa_Don", "Hoa_Don_Id");
            DropTable("dbo.Hoa_Don");
            DropTable("dbo.Nguoi_Dung");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Nguoi_Dung",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ho_Dem = c.String(nullable: false),
                        Ten = c.String(nullable: false),
                        Sdt = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Ngay_Sinh = c.DateTime(nullable: false),
                        Dia_Chi = c.String(nullable: false),
                        Loai_Nguoi_Dung_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hoa_Don",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ngay_Lap = c.DateTime(nullable: false),
                        Don_Gia = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Chi_Tiet_Hoa_Don", "Hoa_Don_Id", c => c.Int());
            DropColumn("dbo.Tai_Khoan", "Email");
            DropColumn("dbo.Chi_Tiet_Hoa_Don", "SoLuong");
            CreateIndex("dbo.Nguoi_Dung", "Loai_Nguoi_Dung_Id");
            CreateIndex("dbo.Chi_Tiet_Hoa_Don", "Hoa_Don_Id");
            AddForeignKey("dbo.Nguoi_Dung", "Loai_Nguoi_Dung_Id", "dbo.Loai_Nguoi_Dung", "Loai_Nguoi_Dung_Id", cascadeDelete: true);
            AddForeignKey("dbo.Chi_Tiet_Hoa_Don", "Hoa_Don_Id", "dbo.Hoa_Don", "Id");
        }
    }
}
