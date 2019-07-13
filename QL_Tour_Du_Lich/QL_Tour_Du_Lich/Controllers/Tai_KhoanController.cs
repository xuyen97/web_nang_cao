using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QL_Tour_Du_Lich.Models;

namespace QL_Tour_Du_Lich.Controllers
{
    public class Tai_KhoanController : Controller
    {
        private Context_Database db = new Context_Database();

        // GET: Tai_Khoan
        public ActionResult Index()
        {
            return View(db.Tai_Khoans.ToList());
        }

        // GET: Tai_Khoan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tai_Khoan tai_Khoan = db.Tai_Khoans.Find(id);
            if (tai_Khoan == null)
            {
                return HttpNotFound();
            }
            return View(tai_Khoan);
        }

        // GET: Tai_Khoan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tai_Khoan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ten_Dang_Nhap,Mat_Khau,Loai_Nguoi_Dung_Id")] Tai_Khoan tai_Khoan)
        {
            if (ModelState.IsValid)
            {
                db.Tai_Khoans.Add(tai_Khoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tai_Khoan);
        }

        // GET: Tai_Khoan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tai_Khoan tai_Khoan = db.Tai_Khoans.Find(id);
            if (tai_Khoan == null)
            {
                return HttpNotFound();
            }
            return View(tai_Khoan);
        }

        // POST: Tai_Khoan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ten_Dang_Nhap,Mat_Khau,Loai_Nguoi_Dung_Id")] Tai_Khoan tai_Khoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tai_Khoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tai_Khoan);
        }

        // GET: Tai_Khoan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tai_Khoan tai_Khoan = db.Tai_Khoans.Find(id);
            if (tai_Khoan == null)
            {
                return HttpNotFound();
            }
            return View(tai_Khoan);
        }

        // POST: Tai_Khoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tai_Khoan tai_Khoan = db.Tai_Khoans.Find(id);
            db.Tai_Khoans.Remove(tai_Khoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
