using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Group3.Models
{
    public class CheckoutViewModel
    {
        public decimal? Price { get; set; }
        public string CourseName { get; set; }
        public DateTime Datetime { get; set; }
        public string LearnerName { get; set; }
    }
}