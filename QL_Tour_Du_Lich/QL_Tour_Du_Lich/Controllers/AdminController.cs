using QL_Tour_Du_Lich.App_Start;
using QL_Tour_Du_Lich.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Controllers
{
    public class AdminController : Controller
    {
        private Context_Database db = new Context_Database();
        // GET: Admin
        
        public ActionResult Index()
        {
            Session["thongbaoreset"] = "";
            if ((bool)Session["adminlogin"]==false)
            {
                return RedirectToAction("DangNhap","TaiKhoan");
            }
            return View();
        }
        public ActionResult QLTour()
        {
            if ((bool)Session["adminlogin"] == false)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            return View(db.Tours.ToList());
        }
        private void ListLoaiTour()
        {
            List<Loai_Tour> listloaitour = db.Loai_Tours.ToList();
            SelectList selectlist = new SelectList(listloaitour, "Loai_Tour_ID", "Ten_Loai_Tour", "Loai_Tour_ID");
            ViewBag.ListLoaiTour = selectlist;
        }
        private void ListTrangThai()
        {
            List<string> listtrangthai = new List<string>();
            listtrangthai.Add("Mở");
            listtrangthai.Add("Đóng");
            SelectList selecttt = new SelectList(listtrangthai);
            ViewBag.ListTrangThai = selecttt;
        }
        public ActionResult CreateTour()
        {
            if ((bool)Session["adminlogin"] == false)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            ListLoaiTour();
            ListTrangThai();
            return View();
        }
        [HttpPost]
        public ActionResult CreateTour(Tour tour)
        {
            ListLoaiTour();
            ListTrangThai();
            if ((bool)Session["adminlogin"] == false)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (ModelState.IsValid)
            {
                int sosanh = DateTime.Compare(tour.Thoi_Gian_Di, tour.Thoi_Gian_Ve);
                int sosanhdi = DateTime.Compare(tour.Thoi_Gian_Di, DateTime.Now.Date);
                int sosanhve = DateTime.Compare(tour.Thoi_Gian_Ve, DateTime.Now.Date);
                if (sosanh <= 0 && sosanhdi >= 0 && sosanhve >= 0)
                {
                    if (tour.So_Luong_Tham_Gia >= 0)
                    {
                        //Save hình
                        var fhinh = Request.Files["Hinh_Anh"];
                        if (fhinh.FileName != "")
                        {
                            var pathhinh = Server.MapPath("~/Hinh/" + fhinh.FileName);
                            fhinh.SaveAs(pathhinh);
                            tour.Hinh_Anh = fhinh.FileName;
                            tour.So_Luong_Da_Tham_Gia = 0;
                            db.Tours.Add(tour);
                            db.SaveChanges();
                            ViewBag.ThongBao = "Tạo thành công";
                            return View(new Tour());
                        }
                        else
                        {
                            ViewBag.ThongBao = "Bạn cần phải chọn hình";
                            return View(tour);
                        }
                    }
                    else
                    {
                        ViewBag.ThongBao = "Số lượng phải >=0";
                        return View(tour);
                    }
                    
                }
                else
                {
                    ViewBag.ThongBaoNgay = "Thời gian về phải bằng hoặc sau thời gian đi và lớn bằng ngày hiện tại";
                    return View(tour);
                }
            }
            return View(tour);
        }
        public ActionResult EditTour(int? id)
        {
            if ((bool)Session["adminlogin"] == false)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            Session["hinhhientai"] = tour.Hinh_Anh;
            if (tour == null)
            {
                return HttpNotFound();
            }
            ListTrangThai();
            ListLoaiTour();
            return View(tour);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTour([Bind(Include = "Tour_Id,Ten_Tour,Hinh_Anh,Thoi_Gian_Di,Thoi_Gian_Ve,Loai_Tour_Id,Lich_Trinh,Trang_Thai,So_Luong_Tham_Gia,So_Luong_Da_Tham_Gia,Gioi_Thieu,Thanh_Pho,Gia")]Tour tour)
        {
            ListLoaiTour();
            ListTrangThai();
            if (ModelState.IsValid)
            {
                int sosanh = DateTime.Compare(tour.Thoi_Gian_Di, tour.Thoi_Gian_Ve);
                int sosanhdi = DateTime.Compare(tour.Thoi_Gian_Di, DateTime.Now.Date);
                int sosanhve = DateTime.Compare(tour.Thoi_Gian_Ve, DateTime.Now.Date);
                try
                {
                    if (sosanh <= 0 && sosanhdi >= 0 && sosanhve >= 0)
                    {
                        var fhinh = Request.Files["Hinh_Anh"];
                        var hinh = fhinh.FileName;
                        Tour t = db.Tours.Find(tour.Tour_Id);
                        if (hinh != "")
                        {
                            Session["hinhthaydoi"] = hinh;
                            var pathdelete = Server.MapPath("~/Hinh/" + tour.Hinh_Anh);
                            System.IO.File.Delete(pathdelete);
                            var pathhinh = Server.MapPath("~/Hinh/" + hinh);
                            fhinh.SaveAs(pathhinh);
                            t.So_Luong_Da_Tham_Gia = 0;
                            t.Hinh_Anh = hinh;
                            t.Loai_Tour_Id = tour.Loai_Tour_Id;
                            t.Thoi_Gian_Di = tour.Thoi_Gian_Di;
                            t.Thoi_Gian_Ve = tour.Thoi_Gian_Ve;
                            t.Thanh_Pho = tour.Thanh_Pho;
                            t.Ten_Tour = tour.Ten_Tour;
                            t.Trang_Thai = tour.Trang_Thai;
                            t.Gioi_Thieu = tour.Gioi_Thieu;
                            t.Lich_Trinh = tour.Lich_Trinh;
                            t.Gia = tour.Gia;
                            t.So_Luong_Da_Tham_Gia = tour.So_Luong_Da_Tham_Gia;
                            t.So_Luong_Tham_Gia = tour.So_Luong_Tham_Gia;
                            db.Entry(t).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.ThongBao = "Tạo thành công";
                            return RedirectToAction("QLTour");
                        }
                        else
                        {
                            Session["hinhhientai"] = t.Hinh_Anh;
                            t.Loai_Tour_Id = tour.Loai_Tour_Id;
                            t.Thoi_Gian_Di = tour.Thoi_Gian_Di;
                            t.Thoi_Gian_Ve = tour.Thoi_Gian_Ve;
                            t.Thanh_Pho = tour.Thanh_Pho;
                            t.Ten_Tour = tour.Ten_Tour;
                            t.Trang_Thai = tour.Trang_Thai;
                            t.Gioi_Thieu = tour.Gioi_Thieu;
                            t.Lich_Trinh = tour.Lich_Trinh;
                            t.Gia = tour.Gia;
                            t.So_Luong_Da_Tham_Gia = tour.So_Luong_Da_Tham_Gia;
                            t.So_Luong_Tham_Gia = tour.So_Luong_Tham_Gia;
                            db.Entry(t).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.ThongBao = "Tạo thành công";
                            return RedirectToAction("QLTour");
                        }

                    }
                    else
                    {
                        ViewBag.ThongBaoNgay = "Thời gian về phải bằng hoặc sau thời gian đi và lớn hơn bằng thời gian hiện tại";
                        return View(tour);
                    }

                }
                catch (Exception)
                {
                    return HttpNotFound();
                }
            }
            return View(tour);
        }
        public ActionResult DeleteTour(int? id)
        {
            if ((bool)Session["adminlogin"] == false)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }
        [HttpPost]
        public ActionResult DeleteTour(int id)
        {
            try
            {
                Tour tour = db.Tours.Find(id);
                var pathdelete = Server.MapPath("~/Hinh/" + tour.Hinh_Anh);
                foreach(var hd in db.Chi_Tiet_Hoa_Dons.ToList())
                {
                    if(hd.Tour_Id==id)
                    {
                        ViewBag.Delete = "Không thể xóa tour này. Chỉ được xóa những tour đã đóng hoặc chưa được đặt";
                        return View(tour);
                    }
                }
                System.IO.File.Delete(pathdelete);
                db.Tours.Remove(tour);
                db.SaveChanges();
                return RedirectToAction("QLTour");
            }catch(Exception)
            {
                return HttpNotFound();
            }
        }
        public ActionResult QLHoaDon()
        {
            return View(db.Chi_Tiet_Hoa_Dons.ToList());
        }
        public ActionResult EditHoaDon(int? id)
        {
            if ((bool)Session["adminlogin"] == false)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chi_Tiet_Hoa_Don hd = db.Chi_Tiet_Hoa_Dons.Find(id);
            Session["hoadonhientai"] = hd;
            if (hd == null)
            {
                return HttpNotFound();
            }
            setDataDropEditHoaDon();
            return View(hd);
        }
        [HttpPost]
        public ActionResult EditHoaDon(Chi_Tiet_Hoa_Don hd)
        {
            setDataDropEditHoaDon();
            if (ModelState.IsValid)
            {
                Tour tour = getTour(hd.Tour_Id);
                Chi_Tiet_Hoa_Don hdcu = (Chi_Tiet_Hoa_Don)Session["hoadonhientai"];
                int slcu = hdcu.SoLuong;
                int sldathamgia = tour.So_Luong_Da_Tham_Gia;
                int soluong = hd.SoLuong;
                int slchophep = tour.So_Luong_Tham_Gia - tour.So_Luong_Da_Tham_Gia;
                if (soluong > slchophep)
                {
                    ViewBag.ThongBao = "Số lượng gnuoiwf tham gia vượt mức co phép";
                    return View(hd);
                }
                else
                {
                    tour.So_Luong_Da_Tham_Gia = sldathamgia - slcu;
                    db.Entry(tour).State = EntityState.Modified;
                    db.SaveChanges();
                    tour.So_Luong_Da_Tham_Gia = tour.So_Luong_Da_Tham_Gia + hd.SoLuong;
                    db.Entry(tour).State = EntityState.Modified;
                    hd.Ngay_Lap = hdcu.Ngay_Lap;
                    hd.Tong_Don_Gia = hd.SoLuong * tour.Gia;
                    db.Entry(hd).State = EntityState.Modified;
                    db.SaveChanges();
                    string to = hd.Email.Trim();
                    Session["thongbao"] = "Kiểm tra email của bạn để xác nhận đơn hàng";
     
                    string bodysendmail ="<br/><table class='table'><tr><th>Mã hóa đơn</th><th>Ngày lập</th><th>Đơn giá</th><th>Người lập đơn hàng</th></tr><tr><td>" + hd.Id + "</td><td>" + hd.Ngay_Lap + "</td><td>" + hd.Tong_Don_Gia + "</td><td>" + hd.Ten_Khach_Hang + "</td></tr></table>";
                    sendMail(to, "Thay đổi đơn hàng từ Easy tour booking", bodysendmail);
                    return RedirectToAction("QLHoaDon");
                }
            }
            return View(hd);
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
        private void setDataDropEditHoaDon()
        {
            List<string> listtrangthai = new List<string>();
            listtrangthai.Add("Hủy");
            listtrangthai.Add("Xác nhận");
            listtrangthai.Add("Chờ");
            SelectList selecttt = new SelectList(listtrangthai);
            ViewBag.ListTTHoaDon = selecttt;
            List<Tour> listtour = db.Tours.ToList();
            SelectList selectlist = new SelectList(listtour, "Tour_Id", "Ten_Tour", "Tour_Id");
            ViewBag.ListLoaiTourHD = selectlist;
        }
        private Tour getTour(int id)
        {
            return db.Tours.Find(id);
        }
    }
}