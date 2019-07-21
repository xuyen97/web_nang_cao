using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Tour_Du_Lich.Models;
using System.Data.Entity;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace QL_Tour_Du_Lich.Controllers
{
    public class HomeController : Controller
    {
        Context_Database db = new Context_Database();
        // GET: Home
        public ActionResult Index()
        {

            LoadDefaulData();
            return View(db.Tours.ToList());
        }
        public ActionResult QueryGia(int id)
        {
            LoadDefaulData();
            List<Tour> list = GetListByPrice(id).ToList();
            return View(list);
        }
        public ActionResult QueryNgoaiNuoc(string tp)
        {
            LoadDefaulData();
            return View(GetListByThanhPho(tp));
        }
        public ActionResult QueryTrongNuoc(string tp)
        {
            LoadDefaulData();
            return View(GetListByThanhPho(tp));
        }
        private List<Tour> GetListTourTrongNuoc()
        {
            List<Tour> listtrongnuoc = db.Tours.Where(x => x.Loai_Tour_Id == 1).ToList();
            return listtrongnuoc;
        }
        private List<Tour> GetListTourNgoaiNuoc()
        {
            List<Tour> listngoainuoc = db.Tours.Where(x => x.Loai_Tour_Id == 2).ToList();
            return listngoainuoc;
        }
        private List<Tour> GetListByThanhPho(string thanhpho)
        {
            List<Tour> listtrongnuoc = db.Tours.Where(x => x.Thanh_Pho.Equals(thanhpho)).ToList();
            return listtrongnuoc;
        }
        private void LoadDefaulData()
        {
            ViewBag.DSThanhPhoTrongNuoc = GetListTourTrongNuoc();
            ViewBag.DSThanhPhoNgoaiNuoc = GetListTourNgoaiNuoc();
        }
        private List<Tour> GetListByPrice(int value)
        {
            List<Tour> list = null;
            switch (value)
            {
                case 1:
                    list = db.Tours.Where(x => x.Gia >= 2000000 && x.Gia < 3500000).ToList();
                    break;
                case 2:
                    list = db.Tours.Where(x => x.Gia >= 3500000 && x.Gia < 5000000).ToList();
                    break;
                case 3:
                    list = db.Tours.Where(x => x.Gia >= 5000000 && x.Gia < 6500000).ToList();
                    break;
                case 4:
                    list = db.Tours.Where(x => x.Gia >= 6500000 && x.Gia < 8000000).ToList();
                    break;
                case 5:
                    list = db.Tours.Where(x => x.Gia > 8000000).ToList();
                    break;
            }
            return list;
        }
        public ActionResult ChiTietTour(int id)
        {
            LoadDefaulData();
            Tour tour = db.Tours.Find(id);
            ViewBag.SLMax = tour.So_Luong_Tham_Gia;
            ViewBag.SLTG = tour.So_Luong_Da_Tham_Gia;
            return View(tour);
        }
        public ActionResult DatTour(int id)
        {
            LoadDefaulData();
            Tour tour = db.Tours.Find(id);
            Session["TourId"] = id;
            SetDataTour(tour);
            return View();
        }
        private void SetDataTour(Tour tour)
        {
            ViewBag.TenTour = tour.Ten_Tour;
            ViewBag.GioiThieu = tour.Gioi_Thieu;
            ViewBag.NgayDi = tour.Thoi_Gian_Di.ToShortDateString();
            ViewBag.NgayVe = tour.Thoi_Gian_Ve.ToShortDateString();
            ViewBag.SLMax = tour.So_Luong_Tham_Gia;
            ViewBag.SLHT = tour.So_Luong_Da_Tham_Gia;
            ViewBag.Hinh = tour.Hinh_Anh;
            ViewBag.Gia = tour.Gia;
            ViewBag.ThanhPho = tour.Thanh_Pho;
            ViewBag.LichTrinh = tour.Lich_Trinh;
            ViewBag.TourID = tour.Tour_Id;
        }
        [HttpPost]
        public ActionResult DatTour(Chi_Tiet_Hoa_Don hd)
        {
            LoadDefaulData();
            TempData["Layout"] = "_LayoutPageNguoiDung";
            int tourid= (int)Session["TourId"];
            Tour tour = db.Tours.Find(tourid);
            if (ModelState.IsValid)
            {
                if (hd.SoLuong <= (tour.So_Luong_Tham_Gia - tour.So_Luong_Da_Tham_Gia))
                {
                    if (hd.SoLuong > 0)
                    {
                        hd.Ngay_Lap = DateTime.Parse(DateTime.Now.ToShortDateString());
                        hd.Tong_Don_Gia = hd.SoLuong * tour.Gia;
                        hd.Tour_Id = tourid;
                        tour.So_Luong_Da_Tham_Gia = tour.So_Luong_Da_Tham_Gia + hd.SoLuong;
                        db.Chi_Tiet_Hoa_Dons.Add(hd);
                        db.Entry(tour).State = EntityState.Modified;
                        db.SaveChanges();
                        Chi_Tiet_Hoa_Don cthd = db.Chi_Tiet_Hoa_Dons.Where(x => x.Ngay_Lap.Equals(hd.Ngay_Lap) && x.Ten_Khach_Hang.Equals(hd.Ten_Khach_Hang)).FirstOrDefault();
                        ViewBag.MaHoaDon = "Lưu lại mã hóa đơn và gọi hổ trợ khách hàng nếu muốn hủy (mã hóa đơn " + cthd.Id + ")";
                        ViewBag.ThongBao = "Đã đặt thành công";
                        SetDataTour(tour);
                        sendMail(hd.Email,"Đặt tour từ Easy tour booking","Bạn đã đặt tour thành công. Mã số hóa đơn của bạn là: "+cthd.Id);
                        return View(hd);
                    }
                    else
                    {
                        ViewBag.ThongBao = "Số lượng phải >=0";
                        SetDataTour(tour);
                        return View();
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Số lượng không hợp lệ";
                    SetDataTour(tour);
                    return View();
                }
            }
            SetDataTour(tour);
            return View();
        }
        public ActionResult DangKy()
        {
            LoadDefaulData();
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(Tai_Khoan tk)
        {
            LoadDefaulData();
            if (ModelState.IsValid)
            {
                try
                {
                    if(kiemtraTK(tk)==false)
                    {
                        string matkhau = MD5Hash(tk.Mat_Khau);
                        tk.Mat_Khau = matkhau;
                        tk.Loai_Nguoi_Dung_Id = 1;
                        db.Tai_Khoans.Add(tk);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                        return View(tk);
                    }
                }catch(Exception )
                {
                    return HttpNotFound();
                }
            }
            return View(tk);
        }
        public ActionResult DangNhap()
        {
            LoadDefaulData();
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string Ten_Dang_Nhap, string Mat_Khau)
        {
            LoadDefaulData();
            if (ModelState.IsValid)
            {
                string matkhau = MD5Hash(Mat_Khau);
                if (kiemtraTkDN(Ten_Dang_Nhap, matkhau) == true)
                {
                    Tai_Khoan tk = getTaiKhoan(Ten_Dang_Nhap, matkhau);
                    Session["Email"] = tk.Email;
                    Loai_Nguoi_Dung loai = db.Loai_Nguoi_Dungs.Find(tk.Loai_Nguoi_Dung_Id);
                    if (loai.Ten_Loai.Equals("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index","HomeLogin");
                    }
                }
                else
                {
                    Session["thongbao"] = "";
                    ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không chính xác";
                    return View();
                }
            }
            return View();
        }
        public ActionResult QuenMatKhau()
        {
            LoadDefaulData();
            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(string emailaddress)
        {
            LoadDefaulData();
            if (emailaddress != "")
            {
                LoadDefaulData();
                Session["Ma"] = randomMa();
                Session["emailreset"] = emailaddress;
                Tai_Khoan tk = db.Tai_Khoans.Where(x => x.Email.Equals(emailaddress)).FirstOrDefault();
                if (tk != null)
                {
                    sendMail(emailaddress, "Reset mật khẩu từ Easy tour booking", "Mã xác nhận của bạn là: " + (string)Session["Ma"]);
                    Session["thongbao"] = "Mã xác nhận đã được tới email của bạn";
                    return RedirectToAction("XacNhan");
                }
                else
                {
                    ViewBag.ThongBao = "Email này chưa được đăng ký";
                    return View();
                }
            }
            ViewBag.ThongBao = "Bạn chưa nhập địa chỉ email";
            return View();
            
        }
        public ActionResult XacNhan()
        {
            LoadDefaulData();
            return View();
        }
        [HttpPost]
        public ActionResult XacNhan(string xacnhan)
        {
            LoadDefaulData();
            if (xacnhan!="")
            {
                string ma = (string)Session["Ma"];
                if(xacnhan.Equals(ma.Trim()))
                {
                    string email = (string)Session["emailreset"];
                    Tai_Khoan tk = db.Tai_Khoans.Where(x => x.Email.Equals(email)).FirstOrDefault();
                    tk.Mat_Khau = MD5Hash("12345678");
                    db.Entry(tk).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["thongbao"]= "Mật khẩu mới của bạn là: 12345678";
                    return RedirectToAction("DangNhap");
                }
                else
                {
                    ViewBag.ThongBao = "Mã xác nhận không đúng";
                    return View();
                }
                
            }
            ViewBag.ThongBao = "Bạn chưa nhập mã xác nhận";
            return View();
        }
        private Tai_Khoan getTaiKhoan(string ten,string mk)
        {
            Tai_Khoan tk = db.Tai_Khoans.Where(x => x.Ten_Dang_Nhap.Equals(ten) && x.Mat_Khau.Equals(mk)).FirstOrDefault();
            return tk;
        }
        private bool kiemtraTkDN(string ten,string mk)
        {
            foreach (var x in db.Tai_Khoans.ToList())
            {
                if (x.Ten_Dang_Nhap.Equals(ten)&&x.Mat_Khau.Equals(mk))
                    return true;
            }
            return false;
        }
        private bool kiemtraTK(Tai_Khoan tk)
        {
            foreach (var x in db.Tai_Khoans.ToList())
            {
                if (x.Ten_Dang_Nhap.Equals(tk.Ten_Dang_Nhap))
                    return true;
            }
            return false;
        }
        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        private void sendMail(string email,string subject,string body)
        {
            try
            {
                string from= ConfigurationManager.AppSettings["email"];
                string pass= ConfigurationManager.AppSettings["password"];
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(from, pass);

                MailMessage mm = new MailMessage(from, email,subject,body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
            }
            catch (Exception)
            {
                ViewBag.ThongBao = "Không gửi được mail";
            }
        }
        private string randomMa()
        {
            string UpperCase = "ABCDEFGHJKLOIUYZCMNVN";
            string LowerCase = "abcdefghijklmnopqxyzs";
            string Digits = "1234567890";
            string allCharacters = UpperCase + LowerCase + Digits;
            Random r = new Random();
            string xacnhan = "";
            for(int i=0;i<6;i++)
            {
                double rand = r.NextDouble();
                if(i==0)
                {
                    xacnhan += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                    xacnhan += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
            }
            return xacnhan;
        }
    }
}