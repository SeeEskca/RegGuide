using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RegistrationAdvisory.Models
{
    public interface IRepositoryAdvisory
    {
        List<AdvisoryModel> getMainCourses(string studentDept, string semester);
        List<AdvisoryModel> getElectives();
        List<AdvisoryModel> getCourseBySemester(int semesterNo, int dept);
        List<PreReqModel> getPreRequistes(string studentId);
        List<AdvisoryModel> getRegStatus(string studentId, string dept);
        List<SummaryModel> getStudentGradeBySemester(int semester, string studentid, int deptid);

        DataTable getStudentSummaryCredentials(string studentId);//returns table of student's name and dept
        DataTable getCourseById(string courseId, string courseCat);
        DataTable getStudentGradeByCourse(string studentid, string courseId);

        int addCourse(string courseName, string courseId, string offering, string department,
                       string preReqId, string courseType, int credit, int semesterNo);
        int Deletecourse(string courseId, string courseType);
       
        int modifyCourse(string courseCat, string courseId, string courseDesc, string offering, string credit);
        int updateGrade(string studentId, string courseId, string semesterNo, string grade, string updateType);

        int addDeleteStudentPrereq(string cmd, string studentId, string courseId);
        List<int> getCompletedCredits(string studentId);
        DataTable getStudentInfo(string studentId);

        bool registerStudentForCourse(string studentId, string courseId, string semesterNo);
        bool isGenuineCourse(string courseId);//verify if the course exist in any of the course tables
        List<RegistrationModel> getRegisteredCourseGradeStatus(string studentId);//retrive status of currenly registered courses
        void updateRegisteredAndIncompleteCourses(string studentId);//check time of the year and gives course that are overdue for grade an incomplete status
    }
}
