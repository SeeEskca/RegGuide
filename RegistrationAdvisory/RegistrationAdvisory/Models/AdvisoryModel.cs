using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RegistrationAdvisory.Models
{
    [Bind(Include = "courseId, courseName, offering, credit")]
    public class AdvisoryModel: IEntity
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string offering { get; set; }
        public string credit { get; set; }
        public string grade { get; set; }
        

        public void setFields(DataRow dr)
        {
            this.courseId=dr["CourseId"].ToString();
            this.courseName = dr["CourseDesc"].ToString();
            this.offering = dr["SemesterOffered"].ToString();
            this.credit = dr["Credit"].ToString();
        }
    }
}
