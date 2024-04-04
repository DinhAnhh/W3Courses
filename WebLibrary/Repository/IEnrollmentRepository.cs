using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public interface IEnrollmentRepository
    {
        IEnumerable<Enrollment> GetEnrollment();
        Enrollment GetEnrollemnentByID(int learnerId, int courseId);
        void InsertEnrollment(int learnerId, int courseId);

    }
}