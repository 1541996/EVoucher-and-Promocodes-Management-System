using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
   
    [Table("tbpurchase")]
    public partial class tbpurchase
    {
         [Key]
         public int ID { get; set; }
         public int? VoucherID { get; set; }
         public int? CustomerID { get; set; }
         public DateTime? Accesstime { get; set; }
         public Nullable<bool> IsDeleted { get; set; }
         public int? Quantity { get; set; }
         public string PromoCodes { get; set; }
         public int? PaymentMethodID { get; set; }
         public decimal? Amount { get; set; }
         public int? Discount { get; set; }
         public decimal? TotalAmount { get; set; }
         public bool? IsUsed { get; set; }

    }
}
