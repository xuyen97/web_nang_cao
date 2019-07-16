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
        public ActionResult DSTour()
        {
            return View(db.Tours.ToList());
        }
        public ActionResult CreateTour()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTour(Tour tour)
        {
            if(ModelState.IsValid)
            {
                //Save hình
                var fhinh = Request.Files["Hinh_Anh"];
                var pathhinh = Server.MapPath("~/Hinh/" + fhinh.FileName);
                fhinh.SaveAs(pathhinh);
                tour.Hinh_Anh = fhinh.FileName;
                db.Tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("DSTour");
            }
            return View(tour);
        }
        public ActionResult EditTour(int? id )
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
        public ActionResult EditTour(Tour tour)
        {
            if (ModelState.IsValid)
            {
                var fhinh = Request.Files["Hinh_Anh"];
                var tenhinh = fhinh.FileName;
                Tour t = db.Tours.Find(tour.Tour_Id);
                if (tenhinh!="")
                {
                    var pathhinh = Server.MapPath("~/Hinh/" + tenhinh);
                    fhinh.SaveAs(pathhinh);
                    tour.Hinh_Anh = tenhinh;
                    var pathdelete = Server.MapPath("~/Hinh/" + t.Hinh_Anh);
                    System.IO.File.Delete(pathdelete);
                }
                else
                {
                    tour.Hinh_Anh = t.Hinh_Anh;
                }
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DSTour");
            }
            return View(tour);
        }
        public ActionResult DeleteTour(int id)
        {

            Tour tour = db.Tours.Find(id);
            var pathdelete = Server.MapPath("~/Hinh/" + tour.Hinh_Anh);
            System.IO.File.Delete(pathdelete);
            db.Tours.Remove(tour);
            db.SaveChanges();
            return RedirectToAction("DSTour");
        }
    }
}