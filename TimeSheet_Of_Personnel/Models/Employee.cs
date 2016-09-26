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
        // Neccesary to set ID Manually! Because ID == TimeSheetNumber
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Таб. №")]
        public int EmployeeID { get; set; }

        [DisplayName("П.І.Б.")]
        public string EmployeeName { get; set; }

        [DisplayName("Посада")]
        public string EmployPosition { get; set; }

        [DisplayName("жін.")]
        public bool IsAWoman { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        [DisplayName("Прийнято")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime WorkStart { get; set; }

        [DisplayName("Звільнено")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? WorkEnd { get; set; }
    }
}