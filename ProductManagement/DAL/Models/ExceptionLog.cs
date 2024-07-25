using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ExceptionLog
    {
        [Key]
        public int ExceptionID { get; set; }
        public String LogText { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
