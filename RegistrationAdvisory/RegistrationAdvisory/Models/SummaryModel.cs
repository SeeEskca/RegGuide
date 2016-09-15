using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RegistrationAdvisory.Models
{
   
    public class SummaryModel: IEntity
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string displaySemester { get; set; }
        public string credit { get; set; }
        public string grade { get; set; }
        public int repeatCourse = 0;

        public void setFields(DataRow dr)
        {
            this.courseId=dr["CourseId"].ToString();
            this.courseName = dr["CourseDesc"].ToString();
            this.displaySemester = dr["SemesterNo"].ToString();
            this.credit = dr["Credit"].ToString();
            this.grade = dr["grade"].ToString();
        }
    }
}
