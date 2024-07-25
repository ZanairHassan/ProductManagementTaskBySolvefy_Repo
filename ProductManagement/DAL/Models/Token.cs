using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Token
    {
        [Key]
        public int TokenID { get; set; }

        public string JwtToken { get; set; }
        public int UserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsExpired { get; set; } = false;
        public bool IsDeleted { get; set; }=false;
    }
}
