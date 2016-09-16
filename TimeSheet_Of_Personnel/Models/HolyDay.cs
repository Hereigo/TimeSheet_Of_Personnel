using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

//[DisplayName("День табелюється як :")]

namespace TimeSheet_Of_Personnel.Models
{
    [Table("HR_HolyDays")]
    public class HolyDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HolyDayID { get; set; }

        [DisplayName("Назва свята чи вихідного")]
        public HolyDaysNamesEnum HolyDayName { get; set; }

        [DisplayName("Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HolyDayDate { get; set; }
    }
}