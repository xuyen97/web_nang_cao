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
            List<string> list = listloc.Distinct().ToList();
            return list;
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
            List<string> list = listloc.Distinct().ToList();
            return list;
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
    }
}