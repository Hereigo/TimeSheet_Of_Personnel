using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSheet_Of_Personnel.Models
{
    [Table("HR_DayTypes")]
    public class DayType
    {
        [Key]
        public int DayTypeID { get; set; }

        [DisplayName("День табелюється як :")]
        public string DayTypeName { get; set; }

        [DisplayName("Умов.познач.")]
        public string SymbolName { get; set; }

        [DisplayName("Латиницею")]
        public string SymbolNameLatin { get; set; }

        [DisplayName("Годин")]
        public int WorkHours { get; set; }
    }
}