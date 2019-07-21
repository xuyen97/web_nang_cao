using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Configuration;

namespace QL_Tour_Du_Lich.Models
{
    public class Context_Database:DbContext
    {
        public Context_Database() : base("Context_DatabaseString")
        {

        }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Loai_Tour> Loai_Tours { get; set; }
        public DbSet<Tai_Khoan> Tai_Khoans { get; set; }
        public DbSet<Chi_Tiet_Hoa_Don> Chi_Tiet_Hoa_Dons { get; set; }
        public DbSet<Loai_Nguoi_Dung> Loai_Nguoi_Dungs { get; set; }
    }
}