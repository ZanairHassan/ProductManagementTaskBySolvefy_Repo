using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class TokenVM
    {
        public string JwtToken { get; set; }
        public int UserID { get; set; }
        public bool IsExpired { get; set; }
    }
}
