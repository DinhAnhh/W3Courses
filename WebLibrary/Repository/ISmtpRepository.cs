using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibrary.Repository
{
    public interface ISmtpRepository
    {
        void sendMail(string EmailOfUser, string TieuDe, string NoiDung);
    }
}