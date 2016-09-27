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
            // TEMPORARY FOR TEST - MONTH -2
            // TEMPORARY FOR TEST - MONTH -2

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
            List<CalendRecord> currMonCalRecords = (from r in db.CalendRecords
                                                   where r.CalendRecordName >= firstDayOfMonth &&
                                                   r.CalendRecordName <= lastDayOfMonth
                                                   select r).ToList();

            // HOLYDAYS LIST FOR CURRENT MONTH:
            int[] holyDays = (from h in db.HolyDays
                              where h.HolyDayDate.Month == currMonth
                              select h.HolyDayDate.Day).ToArray();

            // (daysInCurrMonth) 
            // + 1.Num + 2.Name + 3.Position + 4.IsWoman + 5.TimeSheetNum
            int firstColsShift = 5;

            int shiftFromEnd = 10;

            int factDaysSum = 0;  //-10. Фактично відпрац.
            int holydaysSum = 0;  // -9. Відпустка    -   В, Ч, Н, ДД
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
                rows[row, 2] = workingEmployees[row].EmployPosition;
                rows[row, 3] = workingEmployees[row].IsAWoman ? "+" : "";
                rows[row, 4] = workingEmployees[row].EmployeeID.ToString();

                if (workingEmployees[row].IsAWoman) isWomanSum++;

                //CalendRecord records =  in actualCalRecords.Where(e => e.EmployeeID == actualEmployees[row].EmployeeID);



                // TODO:
                // DO NOT FORGET ABOUT HOLYDAYS !!!!!
                // DO NOT FORGET ABOUT HOLYDAYS !!!!!
                int factDays = daysInMon;
                // DO NOT FORGET ABOUT HOLYDAYS !!!!!
                // DO NOT FORGET ABOUT HOLYDAYS !!!!!

                int holydays = 0;
                int holyChild = 0;
                int holyFree = 0;
                int workTrip = 0;
                int dayOff = 0;
                int unknown = 0;
                int seminar = 0;
                int hospital = 0;
                int weekends = 0;

                // START FROM 5 & FILL ROWS WITH - "8"
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
                        //CalendRecord rec = from r in 
                        //currMonCalRecords.Contains(r => r. )


                        rows[row, col] = "8";
                    }
                }

                // WRITE EVERY CALENDAR-RECORD INTO ITS RIGHT PLACE AT CALENDAR:
                foreach (CalendRecord rec in currMonCalRecords.Where(e => e.EmployeeID == workingEmployees[row].EmployeeID))
                {
                    int dayInMonth = rec.CalendRecordName.Day;

                    // IF RECORD-DAY IS NOT IN HOLYDAYS LIST :
                    if (holyDays.Contains(dayInMonth))
                    {
                        // TEST += " c." + dayInMonth;
                    }
                    else
                    {
                        // TEST += " N." + dayInMonth;

                        // COLUMN WITH NUMBER OF DAY IN MONTH (with shift) 
                        // = CALEND.RECORD WITH THE SAME DAY IN CURRENT MONTH
                        rows[row, dayInMonth + firstColsShift - 1] = rec.DayType.SymbolName;

                        // COUNT DIFFERENT TYPES OF NOT-WORKING-DAYS :
                        if (rec.DayType.SymbolName == "-")
                        {
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "в" ||
                            rec.DayType.SymbolName == "ч" ||
                            rec.DayType.SymbolName == "н" ||
                            rec.DayType.SymbolName == "дд")
                        {
                            holydays++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "вп" ||
                                rec.DayType.SymbolName == "до")
                        {
                            holyChild++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "бз" ||
                                rec.DayType.SymbolName == "нб" ||
                                rec.DayType.SymbolName == "зс"
                                )
                        {
                            holyFree++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "вд")
                        {
                            workTrip++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "дв")
                        {
                            dayOff++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "нз")
                        {
                            unknown++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "с")
                        {
                            seminar++;
                            factDays--;
                        }
                        else if (rec.DayType.SymbolName == "тн" ||
                                 rec.DayType.SymbolName == "нн")
                        {
                            hospital++;
                            factDays--;
                        }

                        //-10. Фактично відпрац.
                        // -9. Відпустка    -   В, Ч, Н, ДД
                        // -8. Відп.(вагіт, дог.за дит) - ВП, ДО
                        // -7. Відп.(не оплач) - НБ, БЗ, ЗС
                        // -6. Відрядження - ВД
                        // -5. Відгул   -   ДВ
                        // -4. Незясовано - НЗ
                        // -3. Семінар/підвищ.кваліф. - С
                        // -2. Хвороба - ТН, НН
                        // -1. Вихідні, святкові дні

                        if (holydays > 0) rows[row, colsCnt - 9] = holydays.ToString();
                        if (holyChild > 0) rows[row, colsCnt - 8] = holyChild.ToString();
                        if (holyFree > 0) rows[row, colsCnt - 7] = holyFree.ToString();
                        if (workTrip > 0) rows[row, colsCnt - 6] = workTrip.ToString();
                        if (dayOff > 0) rows[row, colsCnt - 5] = dayOff.ToString();
                        if (unknown > 0) rows[row, colsCnt - 4] = unknown.ToString();
                        if (seminar > 0) rows[row, colsCnt - 3] = seminar.ToString();
                        if (hospital > 0) rows[row, colsCnt - 2] = hospital.ToString();
                    }
                }

                rows[row, colsCnt - 10] = factDays.ToString();

                factDaysSum += factDays;
                holydaysSum += holydays;
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
            rows[rowsCnt - 1, 3] = isWomanSum.ToString();

            rows[rowsCnt - 1, colsCnt - 10] = factDaysSum.ToString();
            rows[rowsCnt - 1, colsCnt - 9] = holydaysSum.ToString();
            rows[rowsCnt - 1, colsCnt - 8] = holyChildSum.ToString();
            rows[rowsCnt - 1, colsCnt - 7] = holyFreeSum.ToString();
            rows[rowsCnt - 1, colsCnt - 6] = workTripSum.ToString();
            rows[rowsCnt - 1, colsCnt - 5] = dayOffSum.ToString();
            rows[rowsCnt - 1, colsCnt - 4] = unknownSum.ToString();
            rows[rowsCnt - 1, colsCnt - 3] = seminarSum.ToString();
            rows[rowsCnt - 1, colsCnt - 2] = hospitalSum.ToString();

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
            var calendRecords = db.CalendRecords.Include(c => c.DayType).Include(c => c.Employee).
                Where(c => c.CalendRecordName.Month == 7);

            // JULY MONTH !!!!!!!!!!!!!!
            // JULY MONTH !!!!!!!!!!!!!!
            // JULY MONTH !!!!!!!!!!!!!!
            // JULY MONTH !!!!!!!!!!!!!!

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
