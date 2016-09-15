using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RegistrationAdvisory.Models
{
    public interface IRepositoryAuthentication
    {
        DataTable getCredentials(string studentId, string password);
        DataTable getUserRole(int userId);
        bool registerStudent(RegisterViewModel model);

        bool isValidUser(string userName, string password);
       
    }
}
