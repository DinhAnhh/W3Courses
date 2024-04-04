using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class StmpConfigDAO
    {
        private static StmpConfigDAO instance = null;
        private static readonly object instanceLock = new object();
        public static StmpConfigDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StmpConfigDAO();

                    }
                    return instance;
                }
            }
        }  

        public void sendMail(string emailuser, string TieuDe, string NoiDung){
            SmtpConfig smtpConfig = new SmtpConfig();
            SmtpClient smtpClient = new SmtpClient{
                  Host = smtpConfig.Host,
                Port = smtpConfig.Port,
                EnableSsl = smtpConfig.EnableSsl,
                Credentials = new NetworkCredential(smtpConfig.UserName, smtpConfig.Password)
            };

            smtpClient.Send(smtpConfig.UserName, emailuser, TieuDe, NoiDung);
        }     
    }
}