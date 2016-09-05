using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSheet_Of_Personnel.Models
{
    [Table("HR_Employees")]
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [DisplayName("П.І.Б.")]
        public string EmployeeName { get; set; }

        [DisplayName("Посада")]
        public string EmployPosition { get; set; }

        [DisplayName("жін.")]
        public bool IsAWoman { get; set; }

        [DisplayName("Таб. №")]
        public int TimesheetNum { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        [DisplayName("Прийнято")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime WorkStart { get; set; }

        [DisplayName("Звільнено")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? WorkEnd { get; set; }
    }
}