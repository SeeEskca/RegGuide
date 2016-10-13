using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAdvisory.Models;
using System.Data;
//using System.Net.Mail;

namespace RegistrationAdvisory.Controllers
{
    [Authorize(Roles ="Adviser")]
    public class AdviserController : Controller
    {
        IBusinessAdvisory ibzadv = GenericFactory<BusinesLayer, IBusinessAdvisory>.createInstanceOf();
       
        // GET: Adviser
        public ActionResult Panel()
        {

         

            UserSession.COURSECATEGORYTOMODIFY = "";//reset modify course/update grade pages if user loads a course and navigate to control panel
            UserSession.ISINSERT = "";              //without updating...

            return View();
        }


        [HttpPost]
        public ActionResult Panel(string id)
        {
            List<AdvisoryModel> courseBySemester = new List<AdvisoryModel>();
            List<SummaryModel> completedcoursesBySemester = new List<SummaryModel>();
            List<SummaryModel> studentSummary = new List<SummaryModel>();
            List<RegistrationModel> registrationStatus = new List<RegistrationModel>();

            DataTable dt = new DataTable();
            int deptid;
            UserSession.STUDENTINSUMMARY = id;
            
            string semesterno = "summary";//semester viewbags base name
            try
            {
                id = id.Trim();
                dt=ibzadv.getStudentSummaryCredentials(id);
                if(dt.Rows.Count == 0)
                {
                    ViewBag.error = "Cannot find student with given student ID";
                    return View();
                }

                UserSession.BACKBUTTON = "SUMMARYPAGE";//Object required to control browser back button on shared views
                                                       //object is used to control the display of partial pages in the panel view
                ViewBag.studentName = dt.Rows[0]["Name"].ToString();
                UserSession.STUDENTINSUMMARYNAME = dt.Rows[0]["Name"].ToString();
                deptid = Int32.Parse(dt.Rows[0]["deptId"].ToString());

                if (deptid == 1)
                {
                    ViewBag.department = "Computer Science";
                    UserSession.DEPARTMENT = "Computer Science";
                }
                else
                {
                    ViewBag.department = "Computer Engineering";
                    UserSession.DEPARTMENT = "Computer Engineering";
                }

                ibzadv.updateRegisteredAndIncompleteCourses(id);//update registered courses status
                //fetch registered courses
                registrationStatus = ibzadv.getRegisteredCourseGradeStatus(id);

                for (int i = 1; i < 9; i++)//generate semester reports for four academic years
                {
                    courseBySemester = ibzadv.getCourseBySemester(i, deptid);
                    completedcoursesBySemester = ibzadv.getStudentGradeBySemester(i, id, deptid);
                    semesterno += i.ToString();
                    ViewData.Add(semesterno, getStudentDisplaySummary(courseBySemester, completedcoursesBySemester, registrationStatus));
                    semesterno = "summary";//resets viewbag base name for next viewbag
            }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return View();
        }


        public ActionResult AddCourse()
        {
           
            return View();
        }


        [HttpPost]
        public ActionResult AddCourse(CourseModel model)
        {

            
            int status;
            string offering = model.offering.ToString();
            string dept = model.department.ToString();
            string courseCat = model.coursetype.ToString();

            if (ModelState.IsValid)
            {
                try
                {
                    status = ibzadv.addCourse(model.courseName, model.courseId, offering, dept,
                        model.preReqId, courseCat, model.credit, model.semesterNo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

               if(status > 0)
                    ViewBag.status = "Course added successfully...";

                ModelState.Clear();
                return View();

            }
            


          
           
            return View(model);
        }

        public ActionResult DeleteCourse()
        {
            

            var model = new DeleteModifyModel();
            model.courseCats = courseTypes(getCourseType());//populate course category dropdown list box
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteCourse(DeleteModifyModel model)
        {
            int status=0;
            if (ModelState.IsValid)
            {
                try
                {
                    status = ibzadv.Deletecourse(model.courseId, model.courseCat);
                }
                catch (Exception ex)
                {
                    ViewBag.status = "Please make a valid selection..." + ex.Message;
                }

                if (status > 0)
                    ViewBag.statusOk = "Course was successfully delelted...";
                else
                    ViewBag.statusErr = "Oops Course not found!";
                ModelState.Clear();

                var modelForDropDown = new DeleteModifyModel();
                modelForDropDown.courseCats = courseTypes(getCourseType());
                return View(modelForDropDown);

            }
            else
                ViewBag.status = "Please make a valid selection";
            return View(model);
        }
        public ActionResult ModifyCourse()
        {
            

            var model = new DeleteModifyModel();
            model.courseCats = courseTypes(getCourseType());
            return View(model);
        }

        [HttpPost]
        public ActionResult ModifyCourse(string cmd, DeleteModifyModel model, string courseId, string courseDesc, string credit, string offering)
        {
            DataTable dt = new DataTable();
            int status = 0;
            string coursecat = "";
            var modelCat = new DeleteModifyModel();

            if (ModelState.IsValid)
            {
                try
                {
                    if (cmd == "Submit")
                    {
                        if (model.courseCat != null && model.courseId != null)
                        {
                            dt = ibzadv.getCourseById(model.courseId, model.courseCat);
                            //handle null table error
                            if (dt.Rows.Count == 0)
                            {
                                ViewBag.modelError = "Course not found, Please modify your selection";
                                modelCat.courseCats = courseTypes(getCourseType());//repopulate dropdown
                                UserSession.COURSECATEGORYTOMODIFY = "";
                                return View(modelCat);
                            }
                            string[] item = RepositoryHelper.convertDataRowToList(dt);
                            ViewBag.courseId = item[0];
                            ViewBag.courseDesc = item[1];
                            ViewBag.credit = item[2];
                            ViewBag.offering = item[3];
                            UserSession.COURSECATEGORYTOMODIFY = model.courseCat;
                        }
                        else
                        {
                            ViewBag.modelError = "Model error...Please fill out both fields";
                            modelCat.courseCats = courseTypes(getCourseType());//repopulate dropdown
                            return View(modelCat);
                        }

                    }
                    else
                    {
                        //if course update button is detected
                        coursecat = UserSession.COURSECATEGORYTOMODIFY as string;
                        status = ibzadv.modifyCourse(coursecat, courseId, courseDesc, offering, credit);
                        UserSession.COURSECATEGORYTOMODIFY = "";

                        if (status > 0)
                            ViewBag.status = "Course modification update successful...";
                        else
                            ViewBag.modelError = "Course update fail...";

                        modelCat.courseCats = courseTypes(getCourseType());//repopulate dropdown
                        return View(modelCat);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.status=ex.Message;
                }
            }
           

            modelCat.courseCats = courseTypes(getCourseType());
            return View(modelCat);

            //return View();
        }

      
        public ActionResult AddUpdateGrade()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUpdateGrade(string cmd, string studentId, string courseId, string grade, string semesterNo)
        {
            DataTable dt = new DataTable();
           // DataTable dt1 = new DataTable();
            string updateType = "";
            try
            {
                if (cmd == "Check for Grade")
                {
                    //check if student exists, and retrieve grade info 
                    //trim studentid and courseid
                    studentId = studentId.Trim();
                    courseId = courseId.Trim();

                    dt = ibzadv.getStudentSummaryCredentials(studentId);
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.studentId = studentId;
                        ViewBag.courseId = courseId;
                        ViewBag.studentName = dt.Rows[0]["name"].ToString();
                        if (dt.Rows[0]["deptid"].ToString() == "1")
                            ViewBag.department = "Computer Science";
                        else
                            ViewBag.department = "Computer Engineering";


                        dt = null;
                    //determine if course exists
                        if (ibzadv.isGenuineCourse(courseId))
                                dt = ibzadv.getStudentGradeByCourse(studentId, courseId);
                        else {
                                ViewBag.error = "None existing course...please modify your entry";
                                return View();
                              }
                   

                        if (dt.Rows.Count > 0)
                            {
                                ViewBag.offering = dt.Rows[0]["SemesterNo"].ToString();
                                ViewBag.grade = dt.Rows[0]["Grade"].ToString();
                                ViewBag.statusgrade = "See student's course grade below, you can modify grade if necessary(Add Transfer Credits as TR grade)";
                                UserSession.ISINSERT = "0";//signal repository to update an existing course grade
                            }
                         else
                            {
                                ViewBag.statusgrade = "No grade is found for the given course, you can enter a new grade(Add Transfer Credits as TR grade)";
                                UserSession.ISINSERT = "1";//signal repository to insert as new course grade
                            }
                    }
                    else
                        ViewBag.error = "The registration Number you entered cannot be matched to any student!";
                }
                else
                {
                    //add or modify supplied grade and semesterNo
                    updateType = UserSession.ISINSERT as string;
                    //trim and convert grade to upper case then check for correctness
                    //trim semeterNo
                    grade = grade.Trim();
                    grade = grade.ToUpper();
                    semesterNo = semesterNo.Trim();
                    studentId = studentId.Trim();
                    courseId = courseId.Trim();
                    string[] possibleGrades = { "A", "A-", "B", "B+", "B-", "C", "C+", "C-", "D", "D+", "D-", "F", "TR" };
                    if (possibleGrades.Contains(grade))
                    {

                        int statusError = ibzadv.updateGrade(studentId, courseId, semesterNo, grade, updateType);
                        if (statusError > 0)
                            ViewBag.status = "Student grade has been successfully applied";
                        else
                            ViewBag.error = "Error updating student grade!!";
                    }
                    else
                        ViewBag.error = "Error!: Unacceptable grade Letter: " + grade + "...Enter(A, A-, B, B+, B-, C, C+, C-, D, D +, D -, F, TR)";

                   

                    UserSession.ISINSERT = "";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }


        public ActionResult getStudentSummaryDetails()
        {
            List<int> creditSums = new List<int>();
            string studentId = UserSession.STUDENTINSUMMARY as string;
            //// string projectedGradDate;
            try
            {
                creditSums = ibzadv.getCompletedCredits(studentId);

                ViewBag.completedCredits = creditSums[1];
                ViewBag.transferedCredits = creditSums[0];
                ViewBag.totalCreditsCompleted = creditSums[0] + creditSums[1];
                ViewBag.requriedCrdits = 120;
                ViewBag.creditsBeforeGraduation = 120 - (creditSums[0] + creditSums[1]);
                ViewBag.studentName = UserSession.STUDENTINSUMMARYNAME;

                //evaluate expected date of graduation based on completed credits
                ViewBag.projectedGradDate = getProjectedGradDate(120 - (creditSums[0] + creditSums[1]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult getStudentInfo()
        {
            DataTable dt = new DataTable();
            string studentId = UserSession.STUDENTINSUMMARY as string;
            try
            {
                dt = ibzadv.getStudentInfo(studentId);
                if(dt.Rows.Count != 0)
                {
                    ViewBag.studentId = studentId;
                    ViewBag.studentName = UserSession.STUDENTINSUMMARYNAME as string;
                    ViewBag.department = UserSession.DEPARTMENT as string;
                    ViewBag.admitYear = dt.Rows[0]["admityr"].ToString();
                    ViewBag.gradYear = dt.Rows[0]["gradyr"].ToString();
                    ViewBag.email = dt.Rows[0]["email"].ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View();
        }

        private string getProjectedGradDate(int creditBeforeGrad)
        {
            string projectedgrad;
            string semester=string.Empty;
            int gradYear=0;
            string gradSemester = string.Empty;
            DateTime dt = DateTime.Now;
            string dts = dt.ToString("MM/dd/yyyy");
            string[] dtar = dts.Split('/');

            int currentYear = Int32.Parse(dtar[2]);
            int currentMonth = Int32.Parse(dtar[0]);

            //determine current semester from current date
            if (Enumerable.Range(1, 5).Contains(currentMonth))
            {
                semester = "Spring";
            }
            else
                semester = "Fall";//summer semester not considered



            //evaluate number outstanding semesters from outstanding credits
            int semesterCount = 0;
            while(creditBeforeGrad >= 15)
            {
                creditBeforeGrad -= 15;
                semesterCount++;
            }

            switch (semesterCount)
            {
                case 1:
                    if (semester == "Spring")
                    {
                        gradYear = currentYear;
                        if (creditBeforeGrad == 0)
                            gradSemester = "Spring";
                        else
                            gradSemester = "Fall";
                    }
                    else
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 1;
                            gradSemester = "Spring";
                        }
                    }
                    break;
                case 2:
                    if(semester == "Spring")
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 1;
                            gradSemester = "Spring";
                        }
                    }
                    else
                    {
                        gradYear = currentYear + 1;
                        if (creditBeforeGrad == 0)
                            gradSemester = "Spring";
                        else
                            gradSemester = "Fall";
                    }
                    break;
                case 3:
                    if (semester == "Spring")
                    {
                        gradYear = currentYear + 1;
                        if (creditBeforeGrad == 0)
                            gradSemester = "Spring";
                        else
                            gradSemester = "Fall";
                    }
                    else
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear + 1;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 2;
                            gradSemester = "Spring";
                        }
                    }
                    break;
                case 4:
                    if (semester == "Spring")
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear + 1;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 2;
                            gradSemester = "Spring";
                        }
                    }
                    else
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear + 2;
                            gradSemester = "Spring";
                        }
                        else
                        {
                            gradYear = currentYear + 2;
                            gradSemester = "Fall";
                        }
                    }

                    break;
                case 5:
                    if (semester == "Spring")
                    {
                        gradYear = currentYear + 2;
                        if (creditBeforeGrad == 0)
                            gradSemester = "Spring";
                        else
                            gradSemester = "Fall";
                    }
                    else
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear + 2;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 3;
                            gradSemester = "Spring";
                        }
                    }

                    break;
                case 6:
                    if (semester == "Spring")
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear + 2;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 3;
                            gradSemester = "Spring";
                        }
                    }
                    else
                    {
                        gradYear = currentYear + 3;
                        if (creditBeforeGrad == 0)
                            gradSemester = "Spring";
                        else
                            gradSemester = "Fall";
                    }
                    break;
                case 7:
                    if (semester == "Spring")
                    {
                        gradYear = currentYear + 3;
                        if (creditBeforeGrad == 0)
                            gradSemester = "Spring";
                        else
                            gradSemester = "Fall";
                    }
                    else
                    {
                        if (creditBeforeGrad == 0)
                        {
                            gradYear = currentYear + 3;
                            gradSemester = "Fall";
                        }
                        else
                        {
                            gradYear = currentYear + 4;
                            gradSemester = "Spring";
                        }
                    }
                    break;
                case 8:
                    if (semester == "Spring")
                    {
                        gradYear = currentYear + 3;
                        gradSemester = "Fall";
                    }
                    else
                    {
                        gradYear = currentYear + 4;
                        gradSemester = "Spring";
                    }
                    break;
                default:
                    break;
            }

            projectedgrad = gradSemester + " " + gradYear;
            return projectedgrad;
        }


        private List<string> getCourseType()
        {

            return new List<string>
            {
                              
                "Computer Engineering",
                 "Computer Science",
                 "Prerequisite",
                "Elective"
                
            };
        }

        private List<SelectListItem> courseTypes(List<string> courseCats)
        {
            List<SelectListItem> courseList = new List<SelectListItem>();
            int i = 1;
           
            foreach(var item in courseCats)
            {
               courseList.Add(new SelectListItem {
                    Text = item,
                    Value = i.ToString()
                });
            i++;
            }


            return courseList;
        }

        private List<SelectListItem> courseTypesStringValue(List<string> courseCats)
        {
            //returns dropdown select items with value= display text
            List<SelectListItem> courseList = new List<SelectListItem>();
            int i = 1;

            foreach (var item in courseCats)
            {
                courseList.Add(new SelectListItem
                {
                    Text = item,
                    Value = item
                });
                i++;
            }


            return courseList;
        }

        public List<SummaryModel> getStudentDisplaySummary(List<AdvisoryModel>cusBySemester, List<SummaryModel> completedCus, List<RegistrationModel>gradeStatus)
        {
            List<SummaryModel> studentSummary = new List<SummaryModel>();
            string[] failGrade = {"C-", "D", "D+", "D-", "F"};
            SummaryModel summary;
            bool isACompltedItem;
            bool isCurrentRegistration;
            
            foreach(var item in cusBySemester)
            {
                isACompltedItem = false;
                isCurrentRegistration = false;
                foreach (var course in completedCus)
                {
                    if (item.courseId == course.courseId)//this item has been complted
                    {
                        summary = new SummaryModel();
                        summary.courseId = course.courseId;
                        summary.courseName = course.courseName;
                        summary.credit = course.credit;
                        summary.grade = course.grade.Trim();
                        if (failGrade.Contains<string>(summary.grade))
                            summary.repeatCourse = 1;

                        studentSummary.Add(summary);
                        isACompltedItem = true;
                        break;//break out of current loop as soon as a match is found
                    }
                }

                if (!isACompltedItem)
                {
                    foreach (var registeredCourse in gradeStatus)
                    {
                        if (item.courseId == registeredCourse.courseId)//the student is currently registered for this course
                        {
                            summary = new SummaryModel();
                            summary.courseId = item.courseId;
                            summary.courseName = item.courseName;
                            summary.credit = item.credit;
                            summary.grade = registeredCourse.regStatus.Trim();
                            studentSummary.Add(summary);
                            isCurrentRegistration = true;
                            break;//break out of current loop as soon as a match is found
                        }
                    }
                }

                //this course has not been completed
                if (!isACompltedItem && !isCurrentRegistration)
                {
                    summary = new SummaryModel();
                    summary.courseId = item.courseId;
                    summary.courseName = item.courseName;
                    summary.credit = item.credit;
                    summary.grade = item.grade;//No grade for this item. not registered not completed
                    studentSummary.Add(summary);
                }
            }


            return studentSummary;
        }

        public ActionResult addRemovePrequisite(string cmd)
        {
            var model = new PrereqAddDeleteModel();
            try
            {
               model.courseCats = courseTypesStringValue(getPrerequisiteList());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult addRemovePrequisite(string cmd, PrereqAddDeleteModel model)
        {
            var modell = new PrereqAddDeleteModel();

            int opState = -1;
            try
            {
                if (model.studentId !=null && model.courseCat != null)
                    opState = ibzadv.addDeleteStudentPrereq(cmd, model.studentId, model.courseCat);
                else
                {
                    ViewBag.error = "Please check your selection and try again...";
                    modell.courseCats = courseTypesStringValue(getPrerequisiteList());
                    return View(modell);

                }
                        
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (opState == 0)
                ViewBag.error = "The selected pre-requisite was not found for the current student";
            else if(opState > 0)
            {
                if (cmd == "Delete Pre-requisite")
                    ViewBag.status = "Course successfully Deleted...";
                else
                    ViewBag.status = "Course successfully Added...";
            }


           
            modell.courseCats = courseTypesStringValue(getPrerequisiteList());//repopulate drop down list
            return View(modell);
        }


        public ActionResult registerStudentForCourse()
        {
            //student gets automatically delisted from course registration when a grade is entered for the course
           
            return View();
        }

        [HttpPost]
        public ActionResult registerStudentForCourse(CourseRegistrationModel model)
        {
            try
            {
                if (ibzadv.registerStudentForCourse(model.studentId, model.courseId, model.semesterNo))
                    ViewBag.status = "Course Registration was Successful";
                else
                    ViewBag.error = "Problems while attemping to register student for the course";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        public List<string> getPrerequisiteList()
        {
            DataTable dt = new DataTable();
            List<string> courseIds = new List<string>();
            dt = ibzadv.getCourseById("", "");
            foreach (DataRow dr in dt.Rows)
            {
                string course = dr["courseid"].ToString();
                courseIds.Add(course);
            }

            return courseIds;
        }
    }
}