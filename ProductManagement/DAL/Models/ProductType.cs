using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductTypeName { get; set; }
        public string? ProductTypeDes { get; set; }
        public DateTime? ProductTypeCreated { get; set; }
        public DateTime? ProductTypeUpdated { get; set; }
        public DateTime? ProductTypeDeleted { get; set; }
        public bool IsDeleted { get; set; }=false;
        public bool IsUpdated { get; set; }=false ;
    }
}
