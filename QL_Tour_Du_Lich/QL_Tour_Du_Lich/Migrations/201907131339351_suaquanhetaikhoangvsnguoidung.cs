namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class suaquanhetaikhoangvsnguoidung : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chi_Tiet_Hoa_Don",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ten_Khach_Hang = c.String(nullable: false),
                        Ngay_Lap = c.DateTime(nullable: false),
                        Tong_Don_Gia = c.Double(nullable: false),
                        Tour_Id = c.Int(nullable: false),
                        Hoa_Don_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hoa_Don", t => t.Hoa_Don_Id)
                .ForeignKey("dbo.Tours", t => t.Tour_Id, cascadeDelete: true)
                .Index(t => t.Tour_Id)
                .Index(t => t.Hoa_Don_Id);
            
            CreateTable(
                "dbo.Hoa_Don",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ngay_Lap = c.DateTime(nullable: false),
                        Don_Gia = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Tour_Id = c.Int(nullable: false, identity: true),
                        Ten_Tour = c.String(nullable: false),
                        Hinh_Anh = c.String(),
                        Thoi_Gian_Di = c.DateTime(nullable: false),
                        Thoi_Gian_Ve = c.DateTime(nullable: false),
                        Ma_Loai_Tour = c.Int(nullable: false),
                        Lich_Trinh = c.String(nullable: false),
                        Trang_Thai = c.String(nullable: false),
                        So_Luong_Tham_Gia = c.Int(nullable: false),
                        So_Luong_Da_Tham_Gia = c.Int(nullable: false),
                        Loai_Tour_Loai_Tour_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Tour_Id)
                .ForeignKey("dbo.Loai_Tour", t => t.Loai_Tour_Loai_Tour_Id)
                .Index(t => t.Loai_Tour_Loai_Tour_Id);
            
            CreateTable(
                "dbo.Loai_Tour",
                c => new
                    {
                        Loai_Tour_Id = c.Int(nullable: false, identity: true),
                        Ten_Loai_Tour = c.String(nullable: false),
                        Ghi_Chu = c.String(),
                    })
                .PrimaryKey(t => t.Loai_Tour_Id);
            
            CreateTable(
                "dbo.Loai_Nguoi_Dung",
                c => new
                    {
                        Loai_Nguoi_Dung_Id = c.Int(nullable: false, identity: true),
                        Ten_Loai = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Loai_Nguoi_Dung_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Loai_Nguoi_Dung", t => t.Loai_Nguoi_Dung_Id, cascadeDelete: true)
                .Index(t => t.Loai_Nguoi_Dung_Id);
            
            CreateTable(
                "dbo.Tai_Khoan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ten_Dang_Nhap = c.String(nullable: false),
                        Mat_Khau = c.String(nullable: false),
                        Loai_Nguoi_Dung_Id = c.Int(nullable: false),
                        Nguoi_Dung_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nguoi_Dung", t => t.Nguoi_Dung_Id)
                .Index(t => t.Nguoi_Dung_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tai_Khoan", "Nguoi_Dung_Id", "dbo.Nguoi_Dung");
            DropForeignKey("dbo.Nguoi_Dung", "Loai_Nguoi_Dung_Id", "dbo.Loai_Nguoi_Dung");
            DropForeignKey("dbo.Chi_Tiet_Hoa_Don", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Tours", "Loai_Tour_Loai_Tour_Id", "dbo.Loai_Tour");
            DropForeignKey("dbo.Chi_Tiet_Hoa_Don", "Hoa_Don_Id", "dbo.Hoa_Don");
            DropIndex("dbo.Tai_Khoan", new[] { "Nguoi_Dung_Id" });
            DropIndex("dbo.Nguoi_Dung", new[] { "Loai_Nguoi_Dung_Id" });
            DropIndex("dbo.Tours", new[] { "Loai_Tour_Loai_Tour_Id" });
            DropIndex("dbo.Chi_Tiet_Hoa_Don", new[] { "Hoa_Don_Id" });
            DropIndex("dbo.Chi_Tiet_Hoa_Don", new[] { "Tour_Id" });
            DropTable("dbo.Tai_Khoan");
            DropTable("dbo.Nguoi_Dung");
            DropTable("dbo.Loai_Nguoi_Dung");
            DropTable("dbo.Loai_Tour");
            DropTable("dbo.Tours");
            DropTable("dbo.Hoa_Don");
            DropTable("dbo.Chi_Tiet_Hoa_Don");
        }
    }
}
