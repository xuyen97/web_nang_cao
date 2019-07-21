using QL_Tour_Du_Lich.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    }
}