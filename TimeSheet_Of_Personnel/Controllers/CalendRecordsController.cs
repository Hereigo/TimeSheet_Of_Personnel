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
            int currMonth = DateTime.Now.Month;

            // TODO :
            // NOT IMPLEMENTED YET !!!
            // NOT IMPLEMENTED YET !!!
            // NOT IMPLEMENTED YET !!!
            // NOT IMPLEMENTED YET !!!
            if (year.HasValue && month.HasValue)
            {
                currYear = Convert.ToInt32(year);
                currMonth = Convert.ToInt32(month);
            }

            ViewBag.MonthDate = new DateTime(currYear, currMonth, 1);

            int daysInMon = DateTime.DaysInMonth(currYear, currMonth);

            DateTime firstDayOfMonth = new DateTime(currYear, currMonth, 01);
            DateTime lastDayOfMonth = new DateTime(currYear, currMonth, daysInMon);

            // EMPLOYEES with WORK.END == NULL OR >= firstDayOfMonth :
#if DEBUG
            List<Employee> workingEmployees = (from e in db.Employees
                                               where e.WorkEnd == null || e.WorkEnd >= firstDayOfMonth
                                               orderby e.EmployeeName
                                               select e).Take(12).ToList();
#else
            List<Employee> workingEmployees = (from e in db.Employees
                                              where e.WorkEnd == null || e.WorkEnd >= firstDayOfMonth
                                              orderby e.EmployeeName
                                              select e).ToList();
#endif
            // CURRENT MONTH CALENDAR RECORDS FOR ALL :
            List<CalendRecord> currMonCalRecords = (from r in db.CalendRecords.Distinct()
                                                    where r.CalendRecordName >= firstDayOfMonth &&
                                                    r.CalendRecordName <= lastDayOfMonth
                                                    select r).Distinct().ToList();

            // HOLYDAYS LIST FOR CURRENT MONTH:
            int[] holyDays = (from h in db.HolyDays
                              where h.HolyDayDate.Month == currMonth
                              select h.HolyDayDate.Day).ToArray();

            // (daysInCurrMonth) 
            // + 1.Num + 2.Name + 3.EditField + 4.Position + 5.IsWoman + 6.TimeSheetNum
            int firstColsShift = 6;

            int shiftFromEnd = 10;

            int factDaysSum = 0;  //-10. Фактично відпрац.
            int vacationSum = 0;  // -9. Відпустка    -   В, Ч, Н, ДД
            int holyChildSum = 0; // -8. Відп.(вагіт, дог.за дит) - ВП, ДО
            int holyFreeSum = 0;  // -7. Відп.(не оплач) - НБ, БЗ, ЗС
            int workTripSum = 0;  // -6. Відрядження - ВД
            int dayOffSum = 0;    // -5. Відгул   -   ДВ
            int unknownSum = 0;   // -4. Незясовано - НЗ
            int seminarSum = 0;   // -3. Семінар/підвищ.кваліф. - С
            int hospitalSum = 0;  // -2. Хвороба - ТН, НН
            int weekendsSum = 0;  // -1. Вихідні, святкові дні

            int isWomanSum = 0;

            int colsCnt = firstColsShift + daysInMon + shiftFromEnd;

            // PLUS ONE ROW FOR SUMMARY :
            int rowsCnt = workingEmployees.Count() + 1;

            // MONTH.VIEW MATRIX :
            string[,] rows = new string[rowsCnt, colsCnt];

            // MINUS ONE ROW FOR SUMMARY :
            for (int row = 0; row < rowsCnt - 1; row++)
            {
                // FILL FIRST 5 COLUMNS :
                rows[row, 0] = (row + 1).ToString();
                rows[row, 1] = workingEmployees[row].EmployeeName;
                rows[row, 2] = ""; // EDITOR FIELD
                rows[row, 3] = workingEmployees[row].EmployPosition;
                rows[row, 4] = workingEmployees[row].IsAWoman ? "+" : "";
                rows[row, 5] = workingEmployees[row].EmployeeID.ToString();

                if (workingEmployees[row].IsAWoman) isWomanSum++;

                int factDays = daysInMon;
                int vacation = 0;
                int holyChild = 0;
                int holyFree = 0;
                int workTrip = 0;
                int dayOff = 0;
                int unknown = 0;
                int seminar = 0;
                int hospital = 0;
                int weekends = 0;
                int notWorkYet = 0;

                // START FROM 6 - WHILE DaysInMonthCount - FILL ROWS WITH - "8"
                for (int col = firstColsShift; col < firstColsShift + daysInMon; col++)
                {
                    int dayInMonth = col - firstColsShift + 1;

                    if (holyDays.Contains(dayInMonth))
                    {
                        rows[row, col] = "";
                        factDays--;
                    }
                    else
                    {
                        rows[row, col] = "8";
                    }
                }

                // WRITE EVERY CALENDAR-RECORD INTO ITS RIGHT PLACE AT CALENDAR:
                foreach (CalendRecord rec in currMonCalRecords.Where(e => e.EmployeeID == workingEmployees[row].EmployeeID))
                {
                    int dayInMonth = rec.CalendRecordName.Day;

                    // COLUMN WITH NUMBER OF DAY IN MONTH (with shift) 
                    // = CALEND.RECORD WITH THE SAME DAY IN CURRENT MONTH
                    rows[row, dayInMonth + firstColsShift - 1] = rec.DayType.SymbolName;

                    // COUNT DIFFERENT TYPES OF NOT-WORKING-DAYS :
                    if (rec.DayType.SymbolName == "-")
                    {
                        notWorkYet++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "в" ||
                             rec.DayType.SymbolName == "ч" ||
                             rec.DayType.SymbolName == "н" ||
                             rec.DayType.SymbolName == "дд")
                    {
                        vacation++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "по" ||
                             rec.DayType.SymbolName == "до")
                    {
                        holyChild++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "бз" ||
                             rec.DayType.SymbolName == "нб" ||
                             rec.DayType.SymbolName == "зс"
                            )
                    {
                        holyFree++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "вд")
                    {
                        workTrip++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "дв")
                    {
                        dayOff++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "нз")
                    {
                        unknown++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "с")
                    {
                        seminar++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }
                    else if (rec.DayType.SymbolName == "тн" ||
                             rec.DayType.SymbolName == "нн")
                    {
                        hospital++;
                        if (!holyDays.Contains(dayInMonth)) factDays--;
                    }

                    if (vacation > 0) rows[row, colsCnt - 9] = vacation.ToString();   // -9. Відпустка    -   В, Ч, Н, ДД
                    if (holyChild > 0) rows[row, colsCnt - 8] = holyChild.ToString(); // -8. Відп.(вагіт, дог.за дит) - ВП, ДО
                    if (holyFree > 0) rows[row, colsCnt - 7] = holyFree.ToString();   // -7. Відп.(не оплач) - НБ, БЗ, ЗС
                    if (workTrip > 0) rows[row, colsCnt - 6] = workTrip.ToString();   // -6. Відрядження - ВД
                    if (dayOff > 0) rows[row, colsCnt - 5] = dayOff.ToString();       // -5. Відгул   -   ДВ
                    if (unknown > 0) rows[row, colsCnt - 4] = unknown.ToString();     // -4. Незясовано - НЗ
                    if (seminar > 0) rows[row, colsCnt - 3] = seminar.ToString();     // -3. Семінар/підвищ.кваліф. - С
                    if (hospital > 0) rows[row, colsCnt - 2] = hospital.ToString();   // -2. Хвороба - ТН, НН


                }
                weekends = daysInMon
                        - factDays
                        - vacation
                        - holyChild
                        - holyFree
                        - workTrip
                        - dayOff
                        - unknown
                        - seminar
                        - hospital
                        - notWorkYet;

                rows[row, colsCnt - 1] = weekends.ToString();                          // -1. Вихідні, святкові дні

                rows[row, colsCnt - 10] = factDays.ToString();                         //-10. Фактично відпрац.

                factDaysSum += factDays;
                vacationSum += vacation;
                holyChildSum += holyChild;
                holyFreeSum += holyFree;
                workTripSum += workTrip;
                dayOffSum += dayOff;
                unknownSum += unknown;
                seminarSum += seminar;
                hospitalSum += hospital;
                weekendsSum += weekends;
            }

            // ADD SUMMARY :
            rows[rowsCnt - 1, 4] = isWomanSum.ToString();

            rows[rowsCnt - 1, colsCnt - 10] = factDaysSum.ToString();
            rows[rowsCnt - 1, colsCnt - 9] = vacationSum.ToString();
            rows[rowsCnt - 1, colsCnt - 8] = holyChildSum.ToString();
            rows[rowsCnt - 1, colsCnt - 7] = holyFreeSum.ToString();
            rows[rowsCnt - 1, colsCnt - 6] = workTripSum.ToString();
            rows[rowsCnt - 1, colsCnt - 5] = dayOffSum.ToString();
            rows[rowsCnt - 1, colsCnt - 4] = unknownSum.ToString();
            rows[rowsCnt - 1, colsCnt - 3] = seminarSum.ToString();
            rows[rowsCnt - 1, colsCnt - 2] = hospitalSum.ToString();
            rows[rowsCnt - 1, colsCnt - 1] = weekendsSum.ToString();

            // COUNT MATRIX SIZE FOR SOME ACTIONS IN THE VIEW :
            ViewBag.calendMatrix = rows;
            ViewBag.rows = rowsCnt;
            ViewBag.columns = colsCnt;

            return View();
        }


        // GET: CalendRecords
        public ActionResult Index()
        {
            // GET RECORDS FOR CURRENT MONTH & YEAR ONLY !!!

            var calendRecords = db.CalendRecords.Include(c => c.DayType).Include(c => c.Employee).
                Where(c => c.CalendRecordName.Year == DateTime.Now.Year &&
                c.CalendRecordName.Month == DateTime.Now.Month);

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
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emplo = db.Employees.Find(id);
            if (emplo == null)
            {
                return HttpNotFound();
            }
            ViewBag.DayTypeID = new SelectList(db.DayTypes, "DayTypeID", "DayTypeName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeName", emplo.EmployeeID);
            return View();
        }

        // POST: CalendRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CalendRecordID,CalendRecordName,EmployeeID,DayTypeID")] CalendRecord calRecSaving, int daysCount)
        {
            if (ModelState.IsValid)
            {
                // TO SAVE A FEW DAYS TOGETHER IN THE SAME TYPE :

                for (int i = 0; i < daysCount; i++)
                {
                    // DO NOT ADD NEW IF EXISTS !!!
                    // OVERWRITE IT !!!

                    DateTime currentRecordDate = calRecSaving.CalendRecordName.AddDays(i);

                    // REMOVING =ALL= DUPLICATES !!!!!!!!
                    // REMOVING =ALL= DUPLICATES !!!!!!!!

                    List<CalendRecord> records = (from r in db.CalendRecords
                                                  where r.CalendRecordName == currentRecordDate &&
                                                  r.EmployeeID == calRecSaving.EmployeeID
                                                  select r).ToList();
                    // TODO:
                    // REFACTORE THIS !!!!!!!!!
                    // REFACTORE THIS !!!!!!!!!
                    // REFACTORE THIS !!!!!!!!!
                    // REFACTORE THIS !!!!!!!!!

                    if (records.Capacity > 0)
                    {
                        foreach (var item in records)
                        {
                            db.CalendRecords.Remove(item);
                        }
                    }

                    db.CalendRecords.Add(new CalendRecord
                    {
                        EmployeeID = calRecSaving.EmployeeID,
                        DayTypeID = calRecSaving.DayTypeID,
                        CalendRecordID = calRecSaving.CalendRecordID,
                        CalendRecordName = calRecSaving.CalendRecordName.AddDays(i)
                    });

                }
                db.SaveChanges();
                return RedirectToAction("MonthView");
            }
            ViewBag.DayTypeID = new SelectList(db.DayTypes, "DayTypeID", "DayTypeName", calRecSaving.DayTypeID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeName", calRecSaving.EmployeeID);
            return View(calRecSaving);
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

        //// GET: CalendRecords/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CalendRecord calendRecord = db.CalendRecords.Find(id);
        //    if (calendRecord == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(calendRecord);
        //}

        //// POST: CalendRecords/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CalendRecord calendRecord = db.CalendRecords.Find(id);
        //    db.CalendRecords.Remove(calendRecord);
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
