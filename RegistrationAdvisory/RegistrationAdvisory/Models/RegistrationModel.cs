using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RegistrationAdvisory.Models
{
  //  [Bind(Include = "courseId, courseName, offering, credit")]
    public class RegistrationModel: IEntity
    {
        public string courseId { get; set; }
        public string regStatus { get; set; }
        public string semesterNo { get; set; }

        public void setFields(DataRow dr)
        {
            this.courseId=dr["CourseId"].ToString();
            this.regStatus = dr["CStatus"].ToString();
            this.semesterNo = dr["SemesterNo"].ToString();
        }
    }
}
