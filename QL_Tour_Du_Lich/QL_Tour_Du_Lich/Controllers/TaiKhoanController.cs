using QL_Tour_Du_Lich.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        Context_Database db = new Context_Database();
        public ActionResult Index()
        {
            return RedirectToAction("DangNhap");
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
                    if (kiemtraTK(tk) == false)
                    {
                        string matkhau = MD5Hash(tk.Mat_Khau);
                        tk.Mat_Khau = matkhau;
                        tk.Loai_Nguoi_Dung_Id = 1;
                        db.Tai_Khoans.Add(tk);
                        db.SaveChanges();
                        return RedirectToAction("DangNhap", tk);
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập đã tồn tại hoặc email đã tồn tại";
                        return View(tk);
                    }
                }
                catch (Exception)
                {
                    return HttpNotFound();
                }
            }
            return View(tk);
        }
        public ActionResult DangNhap()
        {
            LoadDefaulData();
            Session["thongbao"] = "";
            return View(new Tai_Khoan());
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
                        Session["adminlogin"] = true;
                        Session["emailadminlogin"] = tk.Email;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeLogin");
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
            Session["loaixacnhan"] = "resetmk";
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
            try
            {
                LoadDefaulData();
                if (Session["loaixacnhan"].ToString().Equals("resetmk"))
                {
                    Session["ValueButton"] = "Lấy mật khẩu";
                    return View();
                }
                if (Session["loaixacnhan"].ToString().Equals("dattour"))
                {
                    Session["ValueButton"] = "Xác nhận";
                    return View();
                }
            }
            catch (Exception) { return HttpNotFound(); }

            return View();
        }
        [HttpPost]
        public ActionResult XacNhan(string xacnhan)
        {
            LoadDefaulData();
            if (xacnhan != "")
            {
                string ma = (string)Session["Ma"];
                if (xacnhan.Equals(ma.Trim()))
                {
                    if (Session["loaixacnhan"].ToString().Equals("resetmk"))
                    {
                        string email = (string)Session["emailreset"];
                        Tai_Khoan tk = db.Tai_Khoans.Where(x => x.Email.Equals(email)).FirstOrDefault();
                        tk.Mat_Khau = MD5Hash("12345678");
                        db.Entry(tk).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["thongbaoreset"] = "Mật khẩu mới của bạn là: 12345678";
                        return RedirectToAction("DangNhap");
                    }
                    if (Session["loaixacnhan"].ToString().Equals("dattour"))
                    {
                        Chi_Tiet_Hoa_Don hd = (Chi_Tiet_Hoa_Don)Session["hoadonsendmail"];
                        Chi_Tiet_Hoa_Don ct = db.Chi_Tiet_Hoa_Dons.Find(hd.Id);
                        ct.Trang_Thai = "Xác nhận";
                        db.Entry(ct).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.ThongBao = "Xác nhận đơn hàng thành công";
                        string body = "Đơn hàng đã được xác nhận. Nếu có bất kỳ sự thay đổi nào vui lòng liên lệ Hotline (0397611751) để được hỏ trợ";
                        string subject = "Xác nhận đơn hàng từ Easy tour booking";
                        sendMail(ct.Email, subject, body);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Session["thongbao"] = "";
                    ViewBag.ThongBao = "Mã xác nhận không đúng";
                    return View();
                }

            }
            Session["thongbao"] = "";
            ViewBag.ThongBao = "Bạn chưa nhập mã xác nhận";
            return View();
        }
        private Tai_Khoan getTaiKhoan(string ten, string mk)
        {
            Tai_Khoan tk = db.Tai_Khoans.Where(x => x.Ten_Dang_Nhap.Equals(ten) && x.Mat_Khau.Equals(mk)).FirstOrDefault();
            return tk;
        }
        private bool kiemtraTkDN(string ten, string mk)
        {
            foreach (var x in db.Tai_Khoans.ToList())
            {
                if (x.Ten_Dang_Nhap.Equals(ten) && x.Mat_Khau.Equals(mk))
                    return true;
            }
            return false;
        }
        private bool kiemtraTK(Tai_Khoan tk)
        {
            foreach (var x in db.Tai_Khoans.ToList())
            {
                if (x.Ten_Dang_Nhap.Equals(tk.Ten_Dang_Nhap) || x.Email.Equals(tk.Email))
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
        private void sendMail(string email, string subject, string body)
        {
            try
            {
                string from = ConfigurationManager.AppSettings["email"];
                string pass = ConfigurationManager.AppSettings["password"];
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(from, pass);

                MailMessage mm = new MailMessage(from, email, subject, body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.IsBodyHtml = true;
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
            for (int i = 0; i < 6; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    xacnhan += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                    xacnhan += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
            }
            return xacnhan;
        }
        private List<Tour> GetListTourTrongNuoc()
        {
            Loai_Tour loai = db.Loai_Tours.Where(x => x.Ten_Loai_Tour.Equals("Trong nước")).FirstOrDefault();

            int loaitourin = loai.Loai_Tour_Id;

            List<Tour> listtrongnuoc = db.Tours.Where(x => x.Loai_Tour_Id == loaitourin && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
            return listtrongnuoc;
        }
        private List<Tour> GetListTourNgoaiNuoc()
        {
            Loai_Tour loai1 = db.Loai_Tours.Where(x => x.Ten_Loai_Tour.Equals("Ngoài nước")).FirstOrDefault();
            int loaitourout = loai1.Loai_Tour_Id;
            List<Tour> listngoainuoc = db.Tours.Where(x => x.Loai_Tour_Id == loaitourout && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
            return listngoainuoc;
        }
        private List<Tour> GetListByThanhPho(string thanhpho)
        {
            List<Tour> listtrongnuoc = db.Tours.Where(x => x.Thanh_Pho.Equals(thanhpho) && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
            return listtrongnuoc;
        }
        private void LoadDefaulData()
        {
            Session["DSThanhPhoTrongNuoc"] = GetListTourTrongNuoc();
            Session["DSThanhPhoNgoaiNuoc"] = GetListTourNgoaiNuoc();
        }
    }
}