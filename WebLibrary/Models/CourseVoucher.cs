using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class CourseVoucher
    {
        public int CourseVoucherId { get; set; }
        public int? VoucherId { get; set; }
        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
