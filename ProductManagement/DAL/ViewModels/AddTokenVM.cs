using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class AddTokenVM
    {
        public string JwtToken { get; set; }
        public int UserId { get; set; }
        public bool IsExpired { get; set; }
    }
}
