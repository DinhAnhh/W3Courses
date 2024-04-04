using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            InverseAdmin = new HashSet<Voucher>();
            VoucherUsages = new HashSet<VoucherUsage>();
        }

        public int VoucherId { get; set; }
        public int? AdminId { get; set; }
        public int? PercentDiscount { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string CodeVoucher { get; set; }
        public bool? IsActive { get; set; }

        public virtual Voucher Admin { get; set; }
        public virtual ICollection<Voucher> InverseAdmin { get; set; }
        public virtual ICollection<VoucherUsage> VoucherUsages { get; set; }
    }
}
