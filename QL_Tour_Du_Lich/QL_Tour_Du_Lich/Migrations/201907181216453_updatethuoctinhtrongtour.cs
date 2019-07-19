namespace QL_Tour_Du_Lich.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatethuoctinhtrongtour : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tours", "Loai_Tour_Loai_Tour_Id", "dbo.Loai_Tour");
            DropIndex("dbo.Tours", new[] { "Loai_Tour_Loai_Tour_Id" });
            RenameColumn(table: "dbo.Tours", name: "Loai_Tour_Loai_Tour_Id", newName: "Loai_Tour_Id");
            AlterColumn("dbo.Tours", "Loai_Tour_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Tours", "Loai_Tour_Id");
            AddForeignKey("dbo.Tours", "Loai_Tour_Id", "dbo.Loai_Tour", "Loai_Tour_Id", cascadeDelete: true);
            DropColumn("dbo.Tours", "Ma_Loai_Tour");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tours", "Ma_Loai_Tour", c => c.Int(nullable: false));
            DropForeignKey("dbo.Tours", "Loai_Tour_Id", "dbo.Loai_Tour");
            DropIndex("dbo.Tours", new[] { "Loai_Tour_Id" });
            AlterColumn("dbo.Tours", "Loai_Tour_Id", c => c.Int());
            RenameColumn(table: "dbo.Tours", name: "Loai_Tour_Id", newName: "Loai_Tour_Loai_Tour_Id");
            CreateIndex("dbo.Tours", "Loai_Tour_Loai_Tour_Id");
            AddForeignKey("dbo.Tours", "Loai_Tour_Loai_Tour_Id", "dbo.Loai_Tour", "Loai_Tour_Id");
        }
    }
}
