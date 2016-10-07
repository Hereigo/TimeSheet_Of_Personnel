using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeSheet_Of_Personnel.Models;

namespace TimeSheet_Of_Personnel.Controllers
{
    public class HolyDaysController : Controller
    {
        private EDM_TimeSheet db = new EDM_TimeSheet();

        public ActionResult ViewHolyDaysNames()
        {
            List<string> holydNamesEnums = new List<string>();

            foreach (string item in Enum.GetNames(typeof(HolyDaysNamesEnum)))
            {
                holydNamesEnums.Add(item);
            }

            ViewBag.HolydNames = holydNamesEnums;

            return View();
        }

        // GET: HolyDays
        public ActionResult Index()
        {
            // TODO:
            
            // FILTER BY THE YEAR !!!
            // FILTER BY THE YEAR !!!
            // FILTER BY THE YEAR !!!

            return View(db.HolyDays.ToList().OrderBy(h => h.HolyDayDate));
        }

        // GET: HolyDays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolyDay holyDay = db.HolyDays.Find(id);
            if (holyDay == null)
            {
                return HttpNotFound();
            }
            return View(holyDay);
        }

        // GET: HolyDays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HolyDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HolyDayID,HolyDayName,HolyDayDate")] HolyDay holyDay)
        {
            if (ModelState.IsValid)
            {
                db.HolyDays.Add(holyDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holyDay);
        }

        // GET: HolyDays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolyDay holyDay = db.HolyDays.Find(id);
            if (holyDay == null)
            {
                return HttpNotFound();
            }
            return View(holyDay);
        }

        // POST: HolyDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HolyDayID,HolyDayName,HolyDayDate")] HolyDay holyDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holyDay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holyDay);
        }

        // GET: HolyDays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolyDay holyDay = db.HolyDays.Find(id);
            if (holyDay == null)
            {
                return HttpNotFound();
            }
            return View(holyDay);
        }

        // POST: HolyDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolyDay holyDay = db.HolyDays.Find(id);
            db.HolyDays.Remove(holyDay);
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
