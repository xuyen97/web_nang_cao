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
    public class HomeLoginController : Controller
    {
        // GET: HomeLogin
        Context_Database db = new Context_Database();
        public ActionResult Index()
        {

            LoadDefaulData();
            return View(getListTour());
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
        private List<Tour> getListTour()
        {
            List<Tour> list = db.Tours.Where(x => x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
            return list;
        }
        private List<string> GetListTourTrongNuoc()
        {
            Loai_Tour loai = db.Loai_Tours.Where(x => x.Ten_Loai_Tour.Equals("Trong nước")).FirstOrDefault();

            int loaitourin = loai.Loai_Tour_Id;

            List<Tour> listtrongnuoc = db.Tours.Where(x => x.Loai_Tour_Id == loaitourin && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
            List<string> listloc = new List<string>();
            foreach (var x in listtrongnuoc)
            {
                listloc.Add(x.Thanh_Pho);
            }
            listloc.Distinct().ToList();
            return listloc;
        }
        private List<string> GetListTourNgoaiNuoc()
        {
            Loai_Tour loai1 = db.Loai_Tours.Where(x => x.Ten_Loai_Tour.Equals("Ngoài nước")).FirstOrDefault();
            int loaitourout = loai1.Loai_Tour_Id;
            List<Tour> listngoainuoc = db.Tours.Where(x => x.Loai_Tour_Id == loaitourout && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
            List<string> listloc = new List<string>();
            foreach (var x in listngoainuoc)
            {
                listloc.Add(x.Thanh_Pho);
            }
            listloc.Distinct().ToList();
            return listloc;
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
        private List<Tour> GetListByPrice(int value)
        {
            List<Tour> list = null;
            switch (value)
            {
                case 1:
                    list = db.Tours.Where(x => x.Gia >= 2000000 && x.Gia < 3500000 && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
                    break;
                case 2:
                    list = db.Tours.Where(x => x.Gia >= 3500000 && x.Gia < 5000000 && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
                    break;
                case 3:
                    list = db.Tours.Where(x => x.Gia >= 5000000 && x.Gia < 6500000 && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
                    break;
                case 4:
                    list = db.Tours.Where(x => x.Gia >= 6500000 && x.Gia < 8000000 && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
                    break;
                case 5:
                    list = db.Tours.Where(x => x.Gia > 8000000 && x.So_Luong_Da_Tham_Gia < x.So_Luong_Tham_Gia).ToList();
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
            Session["loaixacnhan"] = "dattour";
            SetDataTour(tour);
            return View();
        }
        [HttpPost]
        public ActionResult DatTour(Chi_Tiet_Hoa_Don hd)
        {
            LoadDefaulData();
            int tourid = (int)Session["TourId"];
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
                        hd.Trang_Thai = "Chờ";
                        tour.So_Luong_Da_Tham_Gia = tour.So_Luong_Da_Tham_Gia + hd.SoLuong;
                        db.Chi_Tiet_Hoa_Dons.Add(hd);
                        db.Entry(tour).State = EntityState.Modified;
                        db.SaveChanges();
                        Chi_Tiet_Hoa_Don ct = getChiTietHoaDon(hd.Ten_Khach_Hang, hd.Email, hd.Ngay_Lap);
                        Session["hoadonsendmail"] = ct;
                        string to = hd.Email.Trim();
                        Session["thongbao"] = "Kiểm tra email của bạn để xác nhận đơn hàng";
                        Session["Ma"] = randomMa();
                        string bodysendmail = "Mã xác nhận của bạn là: " + Session["Ma"].ToString() + "<br/><table class='table'><tr><th>Mã hóa đơn</th><th>Ngày lập</th><th>Đơn giá</th><th>Người lập đơn hàng</th></tr><tr><td>" + ct.Id + "</td><td>" + ct.Ngay_Lap + "</td><td>" + ct.Tong_Don_Gia + "</td><td>" + ct.Ten_Khach_Hang + "</td></tr></table>";
                        sendMail(to, "Đặt tour từ Easy tour booking", bodysendmail);
                        SetDataTour(tour);
                        return RedirectToAction("XacNhan");
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
        private Chi_Tiet_Hoa_Don getChiTietHoaDon(string ten, string email, DateTime ngay)
        {
            Chi_Tiet_Hoa_Don hd = db.Chi_Tiet_Hoa_Dons.Where(x => x.Ngay_Lap.Equals(ngay) && x.Ten_Khach_Hang.Equals(ten) && x.Email.Equals(email)).FirstOrDefault();
            return hd;
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
        public ActionResult XacNhan()
        {
            LoadDefaulData();
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
                        Session["thongbao"] = "Mật khẩu mới của bạn là: 12345678";
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
                    ViewBag.ThongBao = "Mã xác nhận không đúng";
                    return View();
                }

            }
            ViewBag.ThongBao = "Bạn chưa nhập mã xác nhận";
            return View();
        }
        
    }
}