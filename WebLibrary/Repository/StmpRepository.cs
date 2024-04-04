using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Repository;

namespace WebLibrary.Models
{
    public class StmpRepository : ISmtpRepository
    {
        public void sendMail(string EmailOfUser, string TieuDe, string NoiDung) => StmpConfigDAO.Instance.sendMail(EmailOfUser, TieuDe, NoiDung);
        
    }

}