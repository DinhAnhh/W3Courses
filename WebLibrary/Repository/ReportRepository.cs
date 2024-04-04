using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class ReportRepository : IReportRepository
    {
        public void AddNew(Report Report) => ReportDAO.Instance.AddNew(Report);
      

        public Report GetReportByEmail(string email) => ReportDAO.Instance.GetReportByEmail(email);
       

        public Report GetReportByID(int ReportID) => ReportDAO.Instance.GetReportByID(ReportID);
        
        public IEnumerable<Report> GetReportlist() =>  ReportDAO.Instance.GetReportlist();
      

        public void Remove(int ReportID) => ReportDAO.Instance.Remove(ReportID);
     

        public void Update(Report Report) => ReportDAO.Instance.Update(Report);
    
    }
}