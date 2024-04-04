using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibrary.Models
{
   public partial class SmtpConfig
    {
        public SmtpConfig()
        {
            // Thiết lập giá trị mặc định cho các thuộc tính
            Host = "smtp.gmail.com";
            Port = 587;
            EnableSsl = true;
            UserName = "anhddce171053@fpt.edu.vn";
            Password = "oyly iexx qpgx yiql";
        }

        public string Host{get; set;}
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}