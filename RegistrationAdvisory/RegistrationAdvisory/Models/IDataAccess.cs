using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RegistrationAdvisory.Models
{
    public interface IDataAccess
    {
        DataTable getDataTable(string sql);
        int insertUpdateDelete(string sql);
       object getScarlar(string sql);
    }
}
