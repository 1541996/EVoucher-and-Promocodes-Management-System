using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    [Table("tbpaymentmethod")]
    public partial class tbpaymentmethod
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? Accesstime { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? Discount { get; set; }
    }
}
