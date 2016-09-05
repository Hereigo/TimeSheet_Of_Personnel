namespace TimeSheet_Of_Personnel.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class EDM_TimeSheet : DbContext
    {
        public EDM_TimeSheet()
            : base("name=EDM_TimeSheet")
        {
        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<DayType> DayTypes { get; set; }
        public virtual DbSet<CalendRecord> CalendRecords { get; set; }
    }
    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}