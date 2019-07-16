using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Tour_Du_Lich.Models;
namespace QL_Tour_Du_Lich.Controllers
{
    public class HomeController : Controller
    {
        Context_Database db = new Context_Database();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Tours.ToList());
        }
        public ActionResult ChiTietTour(int id)
        {
            Tour tour = db.Tours.Find(id);
            return View(tour);
        }
        public ActionResult DatTour()
        {
            return View();
        }
    }
}