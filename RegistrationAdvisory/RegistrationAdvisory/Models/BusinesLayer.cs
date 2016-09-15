using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace RegistrationAdvisory.Models
{
    [Author("Eskca", "007", version=1.0)]//demo only
    public class BusinesLayer : IBusinessAuthentication, IBusinessAdvisory
    {
        IRepositoryAuthentication ira = null;
        IRepositoryAdvisory irAdv = null;

        public BusinesLayer() : this(GenericFactory<Repository, IRepositoryAuthentication>.createInstanceOf(), GenericFactory<Repository, IRepositoryAdvisory>.createInstanceOf()){}
        public BusinesLayer(IRepositoryAuthentication irauth, IRepositoryAdvisory irAd)
        {
            ira = irauth;
            irAdv = irAd;
        }

        public int addCourse(string courseName, string courseId, string offering, string department, string preReqId, string courseType, int credit, int semesterNo)
        {
            return irAdv.addCourse(courseName, courseId, offering, department, preReqId, courseType, credit, semesterNo);
        }

        public int addDeleteStudentPrereq(string cmd, string studentId, string courseId)
        {
            return irAdv.addDeleteStudentPrereq(cmd, studentId, courseId);
        }
        
        public int Deletecourse(string courseId, string courseType)
        {
            return irAdv.Deletecourse(courseId, courseType);
        }

        public List<int> getCompletedCredits(string studentId)
        {
            return irAdv.getCompletedCredits(studentId);
        }

        public DataTable getCourseById(string courseId, string courseCat)
        {
            return irAdv.getCourseById(courseId, courseCat);
        }

        public List<AdvisoryModel> getCourseBySemester(int semesterNo, int dept)
        {
            List<AdvisoryModel> adModel = new List<AdvisoryModel>();
            return adModel = irAdv.getCourseBySemester(semesterNo, dept);
        }

        public DataTable getCredentials(string studentId, string password)
        {
            DataTable dt = new DataTable();
            dt = ira.getCredentials(studentId, password);

            return dt;
        }

        public List<AdvisoryModel> getElectives()
        {
            List<AdvisoryModel> adModel = new List<AdvisoryModel>();
            return adModel = irAdv.getElectives();
        }

        
        public List<AdvisoryModel> getMainCourses(string studentDept, string semester)
        {
            List<AdvisoryModel> adModel = new List<AdvisoryModel>();
            return adModel = irAdv.getMainCourses(studentDept, semester);
        
        }

        public List<PreReqModel> getPreRequistes(string studentId)
        {
           return irAdv.getPreRequistes(studentId);
        }

        public List<RegistrationModel> getRegisteredCourseGradeStatus(string studentId)
        {
            return irAdv.getRegisteredCourseGradeStatus(studentId);
        }

        public List<AdvisoryModel> getRegStatus(string studentId, string dept)
        {
            List<AdvisoryModel> adModel = new List<AdvisoryModel>();
            return adModel = irAdv.getRegStatus(studentId, dept);
        }

       

        public DataTable getStudentGradeByCourse(string studentid, string courseId)
        {
            return irAdv.getStudentGradeByCourse(studentid, courseId);;
        }

        public List<SummaryModel> getStudentGradeBySemester(int semester, string studentid, int deptid)
        {
            return irAdv.getStudentGradeBySemester(semester, studentid, deptid);
        }

        public DataTable getStudentInfo(string studentId)
        {
            return irAdv.getStudentInfo(studentId);
        }

        public DataTable getStudentSummaryCredentials(string studentId)
        {
            return irAdv.getStudentSummaryCredentials(studentId);
        }

        public DataTable getUserRole(int userId)
        {
            return ira.getUserRole(userId);
        }

        public bool isGenuineCourse(string courseId)
        {
            return irAdv.isGenuineCourse(courseId);
        }

        public int modifyCourse(string courseCat, string courseId, string courseDesc, string offering, string credit)
        {
            return irAdv.modifyCourse(courseCat, courseId, courseDesc, offering, credit);
        }

        public bool registerStudent(RegisterViewModel model)
        {
            return ira.registerStudent(model);
        }

        public bool registerStudentForCourse(string studentId, string courseId, string semesterNo)
        {
            return irAdv.registerStudentForCourse(studentId, courseId, semesterNo);
        }

        public bool SignIn(string studentId, string password, bool createPersistentCookie)
        {
            bool isValidLoginRequest = false;
            if (string.IsNullOrEmpty(studentId)) throw new ArgumentException("Value cannot be null or empty", "userName");
            try
            {
                if(isValidUser(studentId, password))
                {
                    string role = getUserAuthRole(studentId, password);
                    FormsAuthenticationTicket authTicket =
                        new FormsAuthenticationTicket(1, studentId, DateTime.Now, DateTime.Now.AddMinutes(5), false, role);
                    string safeTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, safeTicket);
                    HttpContext.Current.Response.Cookies.Add(authenticationCookie);
                    isValidLoginRequest = true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return isValidLoginRequest;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        private string getUserAuthRole(string studentId, string password)
        {

            DataTable dt = new DataTable();
            DataTable roleInfo = new DataTable();
            int userId;
            string roleName = string.Empty; ;
            dt = getCredentials(studentId, password);
            if (dt.Rows.Count != 0)
            {
                UserSession.USERID = dt.Rows[0]["UserId"].ToString();
                UserSession.DISPLAYNAME = dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString();
                UserSession.USERNAME = studentId;
                userId = Int32.Parse(UserSession.USERID);

                roleInfo = getUserRole(userId);
                UserSession.USERROLE = roleInfo.Rows[0]["RoleId"].ToString();
                UserSession.ROLENAME = roleInfo.Rows[0]["RoleName"].ToString();
                roleName = UserSession.ROLENAME as string;
                int roleId = Int32.Parse(UserSession.USERROLE);
                
                if(roleId==1)
                    UserSession.DEPARTMENT = dt.Rows[0]["DeptId"].ToString();
                
                
            }
            
            return roleName;
        }

        public int updateGrade(string studentId, string courseId, string semesterNo, string grade, string updateType)
        {
            return irAdv.updateGrade(studentId, courseId, semesterNo, grade, updateType);
        }

        public void updateRegisteredAndIncompleteCourses(string studentId)
        {
            irAdv.updateRegisteredAndIncompleteCourses(studentId);
        }

        public bool isValidUser(string studentId, string password)
        {
            return ira.isValidUser(studentId, password);
        }
    }
}
