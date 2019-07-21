using QL_Tour_Du_Lich.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            return View();
        }
        public ActionResult QLTour()
        {
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
            ListLoaiTour();
            ListTrangThai();
            return View();
        }
        [HttpPost]
        public ActionResult CreateTour(Tour tour)
        {
            ListLoaiTour();
            ListTrangThai();
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
                            return View();
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
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
        public ActionResult Delete(int? id)
        {
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
            Tour tour = db.Tours.Find(id);
            var pathdelete = Server.MapPath("~/Hinh/" + tour.Hinh_Anh);
            System.IO.File.Delete(pathdelete);
            db.Tours.Remove(tour);
            db.SaveChanges();
            return RedirectToAction("QLTour");
        }
    }
}