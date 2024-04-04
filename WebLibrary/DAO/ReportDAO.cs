using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class ReportDAO
    {
        private static ReportDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ReportDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ReportDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Report> GetReportlist()
        {
            var reports = new List<Report>();
            try
            {
                using var context = new DBContext();
                reports = context.Reports.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reports;
        }

        public Report GetReportByID(int ReportID)
        {
            Report report = null;
            try
            {
                using var context = new DBContext();
                report = context.Reports.SingleOrDefault(c => c.ReportId.Equals(ReportID));
            }
            catch (System.Exception)
            {
                throw;
            }
            return report;
        }

        public void AddNew(Report Report)
        {
            try
            {
                Report existingReport = GetReportByID(Report.ReportId);
                if (existingReport == null)
                {
                    using (var context = new DBContext())
                    {
                        context.Reports.Add(Report);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Report already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Report Report)
        {
            try
            {
                Report existingReport = GetReportByID(Report.ReportId);
                if (existingReport != null)
                {
                    using (var context = new DBContext())
                    {
                        context.Reports.Update(Report);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Report does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int ReportID)
        {
            try
            {
                Report Report = GetReportByID(ReportID);
                if (Report != null)
                {
                    using (var context = new DBContext())
                    {
                        context.Reports.Remove(Report);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Report does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Report GetReportByEmail(string email)
        {
            Report Report = null;
            try
            {
                using var context = new DBContext();
                Report = context.Reports.SingleOrDefault(c => c.Email == email);
            }
            catch (System.Exception)
            {
                throw;
            }
            return Report;
        }
    }
}