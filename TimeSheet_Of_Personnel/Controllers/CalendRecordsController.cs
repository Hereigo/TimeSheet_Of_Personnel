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
    public class CalendRecordsController : Controller
    {
        private EDM_TimeSheet db = new EDM_TimeSheet();

        public ActionResult MonthView(int? year, int? month)
        {
            int currYear = DateTime.Now.Year;

            // TODO:
            // TEMPORARY FOR TEST - MONTH -2
            // TEMPORARY FOR TEST - MONTH -2
            int currMonth = DateTime.Now.Month - 2;

            if (year.HasValue && month.HasValue)
            {
                currYear = Convert.ToInt32(year);
                currMonth = Convert.ToInt32(month);
            }

            ViewBag.MonthDate = new DateTime(currYear, currMonth, 1);

            DateTime firstDayOfMonth = new DateTime(currYear, currMonth, 01);
            DateTime lastDayOfMonth = new DateTime(currYear, currMonth, DateTime.DaysInMonth(currYear, currMonth));
#if DEBUG
            List<Employee> actualEmployees = (from e in db.Employees orderby e.EmployeeName select e).Take(8).ToList();
#else
            List<Employee> actualEmployees = (from e in db.Employees orderby e.EmployeeName select e).ToList();
#endif
            List<CalendRecord> actualCalRecords = (from r in db.CalendRecords
                                                   where r.CalendRecordName >= firstDayOfMonth &&
                                                   r.CalendRecordName <= lastDayOfMonth
                                                   select r).ToList();

            int daysInMon = DateTime.DaysInMonth(currYear, currMonth);
            // (daysInCurrMonth) + 1.Num + 2.Name + 3.Position + 4.IsWoman + 5.TimeSheetNum
            int firstColsShift = 5;
            // 1.Фактично відпрац. дні, 2.відпустка, 3.відрядження, 4.відгул, 
            // 5.неявк з незяс.прич., 6.підвищ.кваліфік., 7.хвороба, 8.Вихідні, святкові дні
            int lastColsShift = 8;

            int colsCnt = daysInMon + firstColsShift + lastColsShift;

            // PLUS ONE ROW FOR SUMMARY :
            int rowsCnt = actualEmployees.Count() + 1;

            string[,] rows = new string[rowsCnt, colsCnt];

            int factDaysSum = 0;
            int holydaysSum = 0;
            int workTripSum = 0;
            int dayOffSum = 0;
            int unknownSum = 0;
            int studyingSum = 0;
            int hospitalSum = 0;
            int weekendsSum = 0;

            // MINUS ONE ROW FOR SUMMARY :
            for (int row = 0; row < rowsCnt - 1; row++)
            {
                // FILL ROW WITH - "8"
                for (int col = 0; col < firstColsShift + daysInMon; col++)
                {
                    rows[row, col] = "8";
                }

                rows[row, 0] = (row + 1).ToString();
                rows[row, 1] = actualEmployees[row].EmployeeName;
                rows[row, 2] = actualEmployees[row].EmployPosition;
                rows[row, 3] = actualEmployees[row].IsAWoman ? "+" : "";
                rows[row, 4] = actualEmployees[row].EmployeeID.ToString();

                // 1.Фактично відпрац. дні, 2.відпустка, 3.відрядження, 4.відгул, 
                // 5.неявк з незяс.прич., 6.підвищ.кваліфік., 7.хвороба, 8.Вихідні, святкові дні
                int factDays = 0;
                int holydays = 0;
                int workTrip = 0;
                int dayOff = 0;
                int unknown = 0;
                int studying = 0;
                int hospital = 0;
                int weekends = 0;

                foreach (CalendRecord rec in actualCalRecords.Where(e => e.EmployeeID == actualEmployees[row].EmployeeID))
                {
                    // COLUMN WITH NUMBER OF DAY IN MONTH (with shift) = CALEND.RECORD WITH THE SAME DAY IN CURRENT MONTH
                    rows[row, rec.CalendRecordName.Day + firstColsShift - 1] = rec.DayType.SymbolName;

                    // COUNT DIFFERENT TYPES OF NOT-WORKING-DAYS :
                    if (rec.DayType.SymbolName == "до" || rec.DayType.SymbolName == "в")
                    {
                        holydays++;
                    }
                    // else if () { }
                    // else if () { }
                    // else if () { }

                    if (holydays > 0)
                    {
                        rows[row, colsCnt - 6] = holydays.ToString();
                    }
                }
                holydaysSum += holydays;
            }
            // ADD SUMMARY :
            rows[rowsCnt - 1, colsCnt - 6] = holydaysSum.ToString();


            ViewBag.calendMatrix = rows;
            ViewBag.rows = rowsCnt;
            ViewBag.columns = colsCnt;

            return View();
        }



        public ActionResult MonthView_OLD()
        {
            // TODO:
            // YEAR & MONTH FOR TESTS ONLY :
            // YEAR & MONTH FOR TESTS ONLY :
            int currYear = 2016;
            int currMonth = 7;

            ViewBag.MonthDate = new DateTime(currYear, currMonth, 1);

            // GET ALL RECORDS FROM DB :
            // TODO:
            // SET PERSONS & PEIODS FILTERS :
            List<CalendRecord> calendRecords =
                (db.CalendRecords.Include(c => c.Employee).Include(c => c.DayType)).ToList();

            // TODO:
            // REFACTOR ME!
            // GET LIST OF ACTUAL EMPLOYEES :
            var employeesList = (from c in calendRecords select c.Employee).Distinct();

            // LIST OF oneEmploRow-s FOR VIEW :
            var rows = new List<EmploMonthRow>();

            foreach (Employee emp in employeesList)
            {
                // EMPTY ROW FOR MONTH OF ONE EMPLOYEE (size 28...31) :
                string[] oneEmploRow = new string[DateTime.DaysInMonth(currYear, currMonth)];

                // FILLING WHOLE ARRAY WITH DEFAULTS :
                for (int i = 0; i < oneEmploRow.Length; i++) oneEmploRow[i] = "8";

                // TODO :
                // FILL WITH WEEKENDS & HOLOYDAYS !!!!!!!!!!!
                // FILL WITH WEEKENDS & HOLOYDAYS !!!!!!!!!!!

                // FILLING CELLS WITH DAYTYPE.VALUES FOR CURR EMPLOYEE :
                List<CalendRecord> daysForCurrEmp = (from d in calendRecords
                                                     where
                            d.Employee.EmployeeName == emp.EmployeeName
                                                     select d).ToList();

                foreach (var day in daysForCurrEmp)
                {
                    oneEmploRow[day.CalendRecordName.Day - 1] = day.DayType.SymbolName;
                }

                var row = new EmploMonthRow()
                {
                    Employee = emp,
                    MonthDays = oneEmploRow
                };

                rows.Add(row);
            }

            return View(rows);
        }


        // GET: CalendRecords
        public ActionResult Index()
        {
            var calendRecords = db.CalendRecords.Include(c => c.DayType).Include(c => c.Employee);
            return View(calendRecords.ToList());
        }

        // GET: CalendRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendRecord calendRecord = db.CalendRecords.Find(id);
            if (calendRecord == null)
            {
                return HttpNotFound();
            }
            return View(calendRecord);
        }

        // GET: CalendRecords/Create
        public ActionResult Create()
        {
            ViewBag.DayTypeID = new SelectList(db.DayTypes, "DayTypeID", "DayTypeName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: CalendRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CalendRecordID,CalendRecordName,EmployeeID,DayTypeID")] CalendRecord calendRecord)
        {
            if (ModelState.IsValid)
            {
                db.CalendRecords.Add(calendRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DayTypeID = new SelectList(db.DayTypes, "DayTypeID", "DayTypeName", calendRecord.DayTypeID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeName", calendRecord.EmployeeID);
            return View(calendRecord);
        }

        // GET: CalendRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendRecord calendRecord = db.CalendRecords.Find(id);
            if (calendRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.DayTypeID = new SelectList(db.DayTypes, "DayTypeID", "DayTypeName", calendRecord.DayTypeID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeName", calendRecord.EmployeeID);
            return View(calendRecord);
        }

        // POST: CalendRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CalendRecordID,CalendRecordName,EmployeeID,DayTypeID")] CalendRecord calendRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DayTypeID = new SelectList(db.DayTypes, "DayTypeID", "DayTypeName", calendRecord.DayTypeID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeName", calendRecord.EmployeeID);
            return View(calendRecord);
        }

        // GET: CalendRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendRecord calendRecord = db.CalendRecords.Find(id);
            if (calendRecord == null)
            {
                return HttpNotFound();
            }
            return View(calendRecord);
        }

        // POST: CalendRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalendRecord calendRecord = db.CalendRecords.Find(id);
            db.CalendRecords.Remove(calendRecord);
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
