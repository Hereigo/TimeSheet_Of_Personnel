namespace TimeSheet_Of_Personnel.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class EDM_TimeSheet : DbContext
    {
        public EDM_TimeSheet()
            : base("name=EDM_TimeSheet")
        {
            // USE THIS FOR TEST SEEDING DB & VIEWING RESULTS :
            // Database.SetInitializer(new EDM_TimeSheet_TEST_Initializer());

            // Database.SetInitializer(new EDM_TimeSheet_Initializer());
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<DayType> DayTypes { get; set; }
        public virtual DbSet<CalendRecord> CalendRecords { get; set; }
        public virtual DbSet<EmploMonthRow> EmploMonthRows { get; set; }
        public virtual DbSet<HolyDay> HolyDays { get; set; }
    }

    [Table("HR_EmploMonthRow")]
    public class EmploMonthRow
    {
        [Key]
        public int EmploMonthRowID { get; set; }
        public Employee Employee { get; set; }
        public string[] MonthDays { get; set; }
    }
}