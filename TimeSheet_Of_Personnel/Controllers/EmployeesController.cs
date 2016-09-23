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
    public class EmployeesController : Controller
    {
        private EDM_TimeSheet db = new EDM_TimeSheet();

        // GET: Employees
        public ActionResult Index()
        {
#if DEBUG
            return View(db.Employees.Take(11).ToList().OrderBy(e => e.EmployeeName));
#else
            return View(db.Employees.ToList().OrderBy(e => e.EmployeeName));
#endif
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            // TODO:
            // GET MAXIMUM TIMESHEET NUMBER & SEND FOR NEW EMPLOYEE !!!
            // GET MAXIMUM TIMESHEET NUMBER & SEND FOR NEW EMPLOYEE !!!
            // GET MAXIMUM TIMESHEET NUMBER & SEND FOR NEW EMPLOYEE !!!
            // GET MAXIMUM TIMESHEET NUMBER & SEND FOR NEW EMPLOYEE !!!
            // GET MAXIMUM TIMESHEET NUMBER & SEND FOR NEW EMPLOYEE !!!

            int timeSheetNumNext = (from e in db.Employees select e.EmployeeID).Max();

            ViewBag.NewTimeSheetNum = ++timeSheetNumNext;

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,EmployeeName,EmployPosition,IsAWoman,Comment,WorkStart,WorkEnd")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                // FILL PREVIOUS DAY IN CURRENT MONTH BEFOR 1-ST DAY OF WORK
                int yearOf1stDay = employee.WorkStart.Year;
                int monthOf1stDay = employee.WorkStart.Month;
                int dayOf1stDate = employee.WorkStart.Day;

                for (int i = 1; i < dayOf1stDate; i++)
                {
                    db.CalendRecords.Add(new CalendRecord
                    {
                        EmployeeID = employee.EmployeeID,
                        DayTypeID = 0, // DON NOT WORK (YET\ALREADY) AT THIS DAY
                        CalendRecordName = new DateTime(yearOf1stDay, monthOf1stDay, i)
                    });
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,EmployeeName,EmployPosition,IsAWoman,Comment,WorkStart,WorkEnd")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //// GET: Employees/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = db.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        //// POST: Employees/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Employee employee = db.Employees.Find(id);
        //    db.Employees.Remove(employee);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
