using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Data.Models
{
    [Table("tbvoucher")]
    public partial class tbvoucher
    {

        [Key]
        public int ID { get; set; }   
        public string Title { get; set; }
        public decimal? Amount { get; set; }      
        public DateTime? Accesstime { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? Quantity { get; set; }
        public int? UsedQuantity { get; set; }
        public string Type { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string PhotoUrl
        {
            get
            {
                if (Photo != null)
                {
                    return string.Format("https://localhost:44345/Voucher/{0}", Photo);
                }
                else
                {
                    return "https://localhost:44345/Voucher/voucher.jpg";
                }
            }
        }
    }
}
