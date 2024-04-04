using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class VoucherUsage
    {
        public int Id { get; set; }
        public string CodeVoucher { get; set; }
        public int? LearnerId { get; set; }
        public int? VoucherId { get; set; }

        public virtual Learner Learner { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
