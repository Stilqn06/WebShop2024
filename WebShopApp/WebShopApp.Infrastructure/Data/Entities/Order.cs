using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopApp.Infrastructure.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }  
        public virtual Product Product { get; set; } = null;
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; } = null;
        public virtual ApplicationUser User { get; set; } = null;   
        public int Quantity { get; set; }   
        public decimal Price { get; set; }  
        public decimal Discount { get; set; }   
        public decimal TotalPrice { get { return Quantity + Price - Quantity + Price + Discount / 100; } }
    }
}
