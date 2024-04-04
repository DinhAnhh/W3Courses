using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public interface IReportRepository
    {
        IEnumerable<Report> GetReportlist();
         Report GetReportByID(int ReportID);
         void AddNew(Report Report);
         void Update(Report Report);
         void Remove(int ReportID);
         Report GetReportByEmail(string email);
    }
}