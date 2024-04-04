using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        public Enrollment GetEnrollemnentByID(int learnerId, int courseId) => EnrollmentDAO.Instance.GetEnrollemnentByID(learnerId, courseId);
        public IEnumerable<Enrollment> GetEnrollment() => EnrollmentDAO.Instance.GetEnrollmentlist();
        public void InsertEnrollment(int learnerId, int courseId) => EnrollmentDAO.Instance.AddNew(learnerId, courseId);
    }
}