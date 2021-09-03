using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("tbcustomer")]
    public partial class tbcustomer
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public int GiftUserLimit { get; set; }
        public int MaxLimit { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public DateTime? Accesstime { get; set; }
    }
}
