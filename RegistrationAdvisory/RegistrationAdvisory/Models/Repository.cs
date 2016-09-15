using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationAdvisory.Models
{
    public class Repository : IRepositoryAuthentication, IRepositoryAdvisory
    {
        IDataAccess idac=null;
        public Repository(): this(GenericFactory<DataAccess, IDataAccess>.createInstanceOf()) { }

        public Repository(IDataAccess idc)
        {
            idac = idc;
        }

        public int addCourse(string courseName, string courseId, string offering, string department, string preReqId, string courseType, int credit, int semesterNo)
        {
            string sql = "";
            int deptId;
            if (department == "Computer Science")
                deptId = 1;
            else
                deptId = 2;

            if (offering == "Both")
                offering = "Fall and Spring";

            if (courseType == "MajorCourse" && department == "ComputerScience")
                sql = "insert into cpscCourses (courseId, deptId, preReqId, courseDesc, credit, semesterOffered, semesterNo)" +
                       "values('" + courseId + "', " + deptId + ", '" + preReqId + "', '" + courseName + "', " +
                       credit + ", '" + offering + "', " + semesterNo + ")";
            else if (courseType == "MajorCourse" && department == "ComputerEngineering")
                sql = "insert into cpegCourses (courseId, deptId, preReqId, courseDesc, credit, semesterOffered, semesterNo)" +
                       "values('" + courseId + "', " + deptId + ", '" + preReqId + "', '" + courseName + "', " +
                       credit + ", '" + offering + "', " + semesterNo + ")";
            else if(courseType=="Elective")
                sql = "insert into Electives (courseId, deptId, courseDesc, credit, semesterOffered, semesterNo)" +
                       "values('" + courseId + "', " + 0 + ", '" + courseName + "', " +
                       credit + ", '" + offering + "', " + semesterNo + ")";
            else if(courseType=="Prerequisite")
                sql = "insert into Prerequisites (courseId, courseDesc, semesterOffered, deptId, credit) " +
                       "values('" + courseId + "', '" + courseName + "', '" + offering + "', " +
                       deptId + ", " + credit + ")";

            return idac.insertUpdateDelete(sql);
        }

        public int addDeleteStudentPrereq(string cmd, string studentId, string courseId)
        {
            string sql="";
            if (cmd == "Delete Pre-requisite")
            {
                sql = "delete from StudentPrerequisite where studentId ='" + studentId + "'" + "and prereqId='" + courseId + "'";
            }
            else
                sql = "insert into StudentPrerequisite(studentId, prereqId) values('" + studentId + "', '" + courseId + "')";


            return idac.insertUpdateDelete(sql);
        }

        public int Deletecourse(string courseId, string courseType)
        {
            string table =getCourseTable(courseType);
           
            string sql = "delete from " + table + " where courseId='" + courseId + "'";

            return idac.insertUpdateDelete(sql);
             
        }

        public List<int> getCompletedCredits(string studentId)
        {
            int transfer, complete;
            object obj1, obj2;
            List<int> creditSums = new List<int>();
            string sql1 = "select sum(credit) from transferedcredits where studentId ='" + studentId +"'";
            string sql2 = "select sum(credit) from CompletedCourses where studentId ='" + studentId + "'";

            obj1 = idac.getScarlar(sql1);
            obj2 = idac.getScarlar(sql2);
            if (obj1 != System.DBNull.Value)
            {
                transfer = (int)obj1;
                creditSums.Add(transfer);
            }
            else
            {
                transfer = 0;
                creditSums.Add(transfer);
            }

            if (obj2 != System.DBNull.Value)
            {
                complete = (int)obj2;
                creditSums.Add(complete);
            }
            else
            {
                complete = 0;
                creditSums.Add(complete);
            }
          

            return creditSums;
        }

        public DataTable getCourseById(string courseId, string courseCAt)
        {
            string table = getCourseTable(courseCAt);
            DataTable dt = new DataTable();
            string sql = "";

            if (courseId =="" && courseCAt == "")
            {
                sql = "select courseid from prerequisites";
            }
            else
                sql = "select * from " + table + " where courseId='" + courseId + "'";
            
            dt = idac.getDataTable(sql);
            //table = "";
            return dt;// idac.getDataTable(sql);
        }

        public List<AdvisoryModel> getCourseBySemester(int semesterNo, int dept)
        {
            DataTable dt = new DataTable();
            string sql = "";
            if (dept == 1)
            {
               sql = "select courseId, courseDesc, SemesterOffered, credit" +
                              " from cpsccourses" +
                              " where SemesterNo like ('%" + semesterNo + "%')";
            }
            else
            {
                sql = "select courseId, courseDesc, SemesterOffered, credit" +
                              " from cpegcourses" +
                              " where SemesterNo like ('%" + semesterNo + "%')";
            }
            dt = idac.getDataTable(sql);

            return RepositoryHelper.convertDataTableToList<AdvisoryModel>(dt);
        }

        public DataTable getCredentials(string studentId, string password)
        {
            DataTable dt = new DataTable();
            string sql = "select UserId, Email, FirstName, LastName, DeptId from Students stu join Users usr on stu.StudentID = usr.StudentId where usr.studentId='" + studentId + "' and usr.Password='" + password + "'";
            dt = idac.getDataTable(sql);
            return dt;
        }

        public List<AdvisoryModel> getElectives()
        {
            DataTable dt = new DataTable();
            List<AdvisoryModel> adMod = new List<AdvisoryModel>();
            string sql = "select courseId, courseDesc, semesterOffered, credit" +
                          " from electives" +
                          " where SemesterOffered  in ('fall', 'spring', 'fall and spring') ";
            dt = idac.getDataTable(sql);
            
            return adMod = RepositoryHelper.convertDataTableToList<AdvisoryModel>(dt);
        }

        public List<AdvisoryModel> getMainCourses(string studentDept, string semester)
        {
            DataTable dt = new DataTable();
            List<AdvisoryModel> adMod = new List<AdvisoryModel>();
            string sql = "";

            if (studentDept == "1")
            {
                sql = "select courseId, courseDesc, SemesterOffered, credit" +
                              " from cpsccourses" +
                              " where SemesterOffered  in ('" + semester + "', 'fall and spring') ";
            }
            else
            {
                sql = "select courseId, courseDesc, SemesterOffered, credit" +
                              " from cpegcourses" +
                              " where SemesterOffered  in ('" + semester + "', 'fall and spring') ";
            }
            dt = idac.getDataTable(sql);
           
            return adMod = RepositoryHelper.convertDataTableToList<AdvisoryModel>(dt);
        }

        public List<PreReqModel> getPreRequistes(string studentId)
        {
            string sql = "select distinct pre.courseid, pre.courseDesc, pre.credit" +
                          " from StudentPrerequisite spr" +
                          " join Prerequisites pre" +
                          " on spr.PreReqId = pre.courseId" +
                          " where spr.StudentId ='" + studentId + "'";
            DataTable dt = new DataTable();
            dt = idac.getDataTable(sql);
            return RepositoryHelper.convertDataTableToList<PreReqModel>(dt);
        }

        public List<AdvisoryModel> getRegStatus(string studentId, string dept)
        {
            string sql = "";

            if (dept == "1")
            {
                sql = "select cpe.courseId,cpe.courseDesc, cpe.Credit, cpe.SemesterOffered" +
                               " from cpsccourses cpe" +
                               " join registeredcourses rgc" +
                               " on cpe.courseId = rgc.courseId" +
                               " where rgc.StudentId ='" + studentId + "'";
            }
            else
            {
                sql = "select cpe.courseId,cpe.courseDesc, cpe.Credit, cpe.SemesterOffered" +
                               " from cpegcourses cpe" +
                               " join registeredcourses rgc" +
                               " on cpe.courseId = rgc.courseId" +
                               " where rgc.StudentId ='" + studentId + "'";
            }

            DataTable dt = new DataTable();
            dt = idac.getDataTable(sql);
            return RepositoryHelper.convertDataTableToList<AdvisoryModel>(dt);
        }

        public DataTable getStudentGradeByCourse(string studentid, string courseId)
        {
            string sql = "select SemesterNo, grade" +
                         " from CompletedCourses" +
                         " where StudentId = '" + studentid + "' and CourseId = '" + courseId + "'";
          
            return idac.getDataTable(sql);
        }

        public List<SummaryModel> getStudentGradeBySemester(int semester, string studentid, int deptid)
        {
            string table = "CPEGCourses cpe";
            if (deptid == 1)
                table = "CPSCCourses cpe";

            string sql= "select cpe.courseid, cpe.courseDesc, cpe.credit, cpe.SemesterNo,cpc.grade" +
                        " from " + table +
                        " join CompletedCourses cpc" +
                        " on cpe.CourseId = cpc.CourseId" +
                        " where cpc.StudentId = '" + studentid + "' and cpc.SemesterNo = '" + semester +"'";

            return RepositoryHelper.convertDataTableToList<SummaryModel>(idac.getDataTable(sql));
        }

        public DataTable getStudentInfo(string studentId)
        {
            string sql= "select admityr, gradyr, email" +
                        " from students" +
                        " where studentId ='" + studentId + "'";

            return idac.getDataTable(sql);

        }

        public DataTable getStudentSummaryCredentials(string studentId)
        {
           string sql= "select FirstName + ' ' + LastName as name, deptid" +
                        " from students" +
                        " where studentid = '" + studentId + "'";
            return idac.getDataTable(sql);
        }

        public DataTable getUserRole(int userId)
        {
            string sql = "select usr.roleid, def.rolename" +
                        " from userrole usr" +
                        " join definedroles def" +
                        " on usr.roleid = def.roleid" +
                        " where userid ='" + userId + "'";
           return idac.getDataTable(sql);
        }

        public int modifyCourse(string courseCat, string courseId, string courseDesc, string offering, string credit)
        {
            string table="";
            if (courseCat == "1")
                table = "cpegcourses";
            else if (courseCat == "2")
                table = "cpsccourses";
            else if (courseCat == "3")
                table = "prerequisites";
            else if (courseCat == "4")
                table = "electives";


            string sql = "update " + table + 
                " set courseDesc='" + courseDesc + "', semesterOffered='" + offering + "', credit=" + credit + 
                " where courseId='" + courseId + "'";
            return idac.insertUpdateDelete(sql);
             

        }

        public bool registerStudent(RegisterViewModel model)
        {
            //check if student is already in the database
            DataTable dt = new DataTable();
            string sql1 = "select * from students where studentId='" + model.StudentId + "'";
            dt = idac.getDataTable(sql1);
            if (dt.Rows.Count > 0)
                return false;//student already exist

            int deptId;
            string deptName = model.Department.ToString();
            if (deptName == "ComputerEngineering")
                deptId = 2;
            else
                deptId = 1;

            string sql = "insert into students(StudentId, DeptId, FirstName, LastName, Initial, AdmitYr, GradYr, Email)" +
                           " values('" + model.StudentId + "', " + deptId + ", '" + model.FirstName + "', '" +
                           model.LastName + "', '" + model.Initial + "', " + model.AdmitYr + ", " + model.GradYr + ", '" +
                           model.Email + "')";
            return idac.insertUpdateDelete(sql) > 0 ? true : false;
        }

        public bool registerStudentForCourse(string studentId, string courseId, string semesterNo)
        {
            string sqlCheck = "select * from registeredCourses where studentId='" + studentId + "' and courseId='" + courseId + "'";
            DataTable dt = new DataTable();
            string dtime = DateTime.Now.ToString("MM/dd/yyyy");
            string sql;
            string registeredSemester = getSemesterName(semesterNo);//determine semester name from semester number
            dt = idac.getDataTable(sqlCheck);//check if  course exist in registration table
            if (dt.Rows.Count > 0)
                return false;
            else
                sql = "insert into registeredCourses(StudentId, CourseId, SemesterNo, SemesterName, CStatus, RegDate)" +
                       " values('" + studentId + "', '" + courseId + "', '" + semesterNo + "', '" + registeredSemester + "', 'R','" + dtime + "')";

            return idac.insertUpdateDelete(sql) > 0 ? true : false;
        }

        public int updateGrade(string studentId, string courseId, string semesterNo, string grade, string updateType)
        {
            string sql = " ";
            int status=0;
            int type = Int32.Parse(updateType);
            DataTable dt = new DataTable();
            string sqlD = "delete from registeredCourses where studentId='" + studentId + "' and courseId='" + courseId + "'";
            string sqlCheck = "select * from registeredCourses where studentId='" + studentId + "' and courseId='" + courseId + "'";
            if (type == 1)
            {
                int credit = getCourseCredit(studentId, courseId);
                if (grade == "TR" && credit > 0)//transfered credit
                {
                    string sqlTR = "insert into transferedcredits(StudentId, CourseId, Credit) values('" +
                                    studentId + "', '" + courseId + "', " + credit + ")";
                    sql = "insert into completedCourses(StudentId, CourseId, Grade, SemesterNo, Credit ) values('" + studentId + "', '" +
                          courseId + "', '" + grade + "', '" + semesterNo + "', " + 0 + ")";//transfered credits are also completed credits. to avoid double counting transfered credits, courses with TR grade have zero credit in complted course table
                    status =idac.insertUpdateDelete(sqlTR);//update transfered credit table, when a new transfer grade is added to completed course table
                    if(status > 0)
                        status = idac.insertUpdateDelete(sql);
                }
                else if(grade != "TR" && credit > 0)//normal completed credit. courses with 0 credit not counted
                { 
                                       
                    sql = "insert into completedCourses(StudentId, CourseId, Grade, SemesterNo, Credit ) values('" + studentId + "', '" +
                          courseId + "', '" + grade + "', '" + semesterNo + "', " + credit + ")";
                    status = idac.insertUpdateDelete(sql);
                    if (status > 0)
                    {
                        dt = idac.getDataTable(sqlCheck);//check if graded course exist in registration table
                        if (dt.Rows.Count > 0)
                            idac.insertUpdateDelete(sqlD);//delete completed course with new grade from course registration table
                    }
                }
            }
            else
            {
                sql = "update completedCourses set semesterNo='" + semesterNo + "', grade='" + grade +
                       "' where studentId ='" + studentId + "' and courseId='" + courseId + "'";
                status = idac.insertUpdateDelete(sql);
            }

           
            return status;
        }

        private string getCourseTable(string courseType)
        {
            string table = "";
            if (courseType == "2")
                table = "cpscCourses";
            else if (courseType == "1")
                table = "cpegCourses";
            else if (courseType == "4")
                table = "Electives";
            else if (courseType == "3")
                table = "Prerequisites";


            return table;
        }

        private int getCourseCredit(string studentId, string courseId)
        {
            int credit;

            string sql = "select credit from CPSCCourses where courseId ='" + courseId + "'";
            string sql1 = "select credit from CPEGCourses where courseId ='" + courseId + "'";
            object obj = idac.getScarlar(sql);

            if(obj != null && obj != System.DBNull.Value)
                credit = (int)obj;
            else
            {
                credit = (int)idac.getScarlar(sql1);
               
            }

                          
            return credit;
        }

        public bool isGenuineCourse(string courseId)
        {
            string sql = "select * from cpsccourses where courseId='" + courseId + "'";
            string sql1 = "select * from cpegcourses where courseId='" + courseId + "'";
            DataTable dt = new DataTable();
            bool status = false;
            dt = idac.getDataTable(sql);
            if (dt.Rows.Count > 0)
                return true;
            else
            {
                dt = idac.getDataTable(sql1);
                if (dt.Rows.Count > 0)
                    return true;
            }

            return status;
        }

        public void updateRegisteredAndIncompleteCourses(string studentId)
        {
            DataTable dt = new DataTable();
            
            string sql = "select * from registeredcourses where studentId='" + studentId + "'";
            dt = idac.getDataTable(sql);//return table containing registered courses for given student
            if(dt.Rows.Count > 0)
                modifyRegistrationStatus(dt);

        }

        void modifyRegistrationStatus(DataTable dt)
        {
           
            string semester;
            string studId = string.Empty;
            string courseId = string.Empty;
          
            string dtime = DateTime.Now.ToString("MM/dd/yyyy");
            string[] mdy = dtime.Split('/');
            //DateTime dts = new DateTime(1970, 1, 1);//base time reference
            //double daysAfterReg = (DateTime.Now - dts).TotalDays;
            foreach (DataRow dr in dt.Rows)
            {
                
                semester = dr["SemesterName"].ToString();
                studId = dr["StudentId"].ToString();
                courseId = dr["CourseId"].ToString();
               
                if(semester == "spring" && Int32.Parse(mdy[0])==5 && Int32.Parse(mdy[1]) >= 15 )//after end of second week of May, all spring courses without grades becomes incomplete
                {                                                                               //and their grade becomes I
                    //gradeStatus = "I", incomplete
                    string sqlSpring = "update registeredCourses set CStatus='I' where studentId='" + studId +
                                   "' and courseId='" + courseId + "'"; 
                    idac.insertUpdateDelete(sqlSpring);
                }
                else if(semester=="fall" && Int32.Parse(mdy[0]) == 12 && Int32.Parse(mdy[1]) >= 15)//after end of second week of December, all fall courses without grade will be given incomplte grade
                {
                    //gradeStatus = "I", incomplete
                    string sqlFall = "update registeredCourses set CStatus='I' where studentId='" + studId +
                        "' and courseId='" + courseId + "'";
                    idac.insertUpdateDelete(sqlFall);
                }
              
            }

            
        }

        string getSemesterName(string semesterNo)
        {
            int sVal = Int32.Parse(semesterNo);
            return sVal % 2 == 0 ? "spring" : "fall";
        }

        public List<RegistrationModel> getRegisteredCourseGradeStatus(string studentId)
        {
            string sql =  "select courseId, SemesterNo, CStatus" +
                                     " from registeredcourses" +
                                     " where StudentId ='" + studentId + "'";
            return RepositoryHelper.convertDataTableToList<RegistrationModel>(idac.getDataTable(sql));
            
        }

        public bool isValidUser(string studentId, string password)
        {
            return getCredentials(studentId, password).Rows.Count > 0 ? true : false;
        }
    }
}
