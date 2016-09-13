using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSheet_Of_Personnel.Models
{
    [Table("HR_CalendRecord")]
    public class CalendRecord
    {
        [Key]
        public int CalendRecordID { get; set; }

        [DisplayName("Дата :")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CalendRecordName { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int DayTypeID { get; set; }
        public virtual DayType DayType { get; set; }
    }
}