using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RegistrationAdvisory.Models
{
    public class PreReqModel : IEntity
    {

        public string preReqId { get; set; }
        public string preReqName { get; set; }
        public string credit { get; set; }

        public void setFields(DataRow dr)
        {
            this.preReqId = dr["courseId"].ToString();
            this.preReqName = dr["courseDesc"].ToString();
            this.credit = dr["Credit"].ToString();

        }
    }
}
