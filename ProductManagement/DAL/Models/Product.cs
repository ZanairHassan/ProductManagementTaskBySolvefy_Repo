using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }
        [MaxLength(50)]
        public string? ProductDescription { get; set; }
        [Precision(16,2)]
        public decimal? ProductPrice { get; set; }
        [MaxLength(50)]
        public string? ProductType { get; set; }
        public DateTime? ProductCreatedDate { get; set; }
        public DateTime? ProductModifiedDate { get; set; }
        public DateTime? ProductDeletedDate { get; set; }
        public bool IsDeleted { get; set; }= false;
        public bool IsModified { get; set; } = false;
    }
}
