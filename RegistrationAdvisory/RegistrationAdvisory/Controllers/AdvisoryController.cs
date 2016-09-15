using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAdvisory.Models;

namespace RegistrationAdvisory.Controllers
{
    [Authorize]
    public class AdvisoryController : Controller
    {
        IBusinessAuthentication ibizAuth = GenericFactory<BusinesLayer, IBusinessAuthentication>.createInstanceOf();
        IBusinessAdvisory ibizAdv = GenericFactory<BusinesLayer, IBusinessAdvisory>.createInstanceOf();



        public ActionResult RegStatus()
        {
           

            List<AdvisoryModel> status = new List<AdvisoryModel>();
            string studentId = "";

            if (UserSession.STUDENTINSUMMARY != "")
                studentId = UserSession.STUDENTINSUMMARY as string;//adviser is logged on and viewing student information
            else
                studentId = UserSession.USERNAME as string;


            string dept = UserSession.DEPARTMENT as string;
            int count;

            try
            {
                status = ibizAdv.getRegStatus(studentId, dept);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            count = status.Count;
            if (count == 0)
                ViewBag.message = "Your current registration information is unavailable";
            else
                ViewBag.message = "Current Course Registration";


            DateTime dt = DateTime.Now;
            string dts = dt.ToString("MM/dd/yyyy");
            string[] dtar = dts.Split('/');
            string year = dtar[2];
            ViewBag.currentSemester = year;
            return View(status);
        }

        public ActionResult StudentAdvisory(int? deptId)
        {
            List<AdvisoryModel> model = new List<AdvisoryModel>();
            List<SelectListItem> semesterValue = new List<SelectListItem>();
         
            string studentDept ="";
            string semester = "Spring";

            

            if (deptId != null)
            {
                studentDept = (deptId ?? default(int)).ToString();//convert nullable to normal int nd cast to string
                UserSession.DEPTIDOFADVISERVIEW = studentDept;//dept adviser is viewing info about this dept
            }

            else
                studentDept = UserSession.DEPARTMENT as string;


            try
            {
                model = ibizAdv.getMainCourses(studentDept, semester);
                ViewBag.electives = ibizAdv.getElectives();
            }
            catch(Exception ex)
            {
                //ViewBag.status = ex.Message;
                throw ex;
            }

            DateTime dt = DateTime.Now ;
            string dts = dt.ToString("MM/dd/yyyy");
            string[] dtar = dts.Split('/');
            string month = dtar[0];
            string year = dtar[2];
            int intMonth = int.Parse(month);
           

            //populate dropdownlistbox
            semesterValue.Add(new SelectListItem
            {
                Text = "Spring",
                Value = "1",
                Selected = true
                

            });
            semesterValue.Add(new SelectListItem
            {
                Text = "Fall",
                Value = "2"
            });

            if (studentDept == "1")
                ViewBag.department = "Compuer Science";
            else if (studentDept == "2")
                ViewBag.department = "Computer Engineering";

            ViewBag.semesterList = semesterValue;
            ViewBag.semester = semester+year;
          


            
            return View(model);
        }


        [HttpPost]
        public ActionResult StudentAdvisory(string semesterNo, int? selectedSemester)
        {
            List<AdvisoryModel> model = new List<AdvisoryModel>();
            List<SelectListItem> semesterValue = new List<SelectListItem>();
            string studentDept = "";// UserSession.DEPARTMENT as string;
            string semester = "";

            if (semesterNo == "1")
                semester = "Spring";
            else
                semester = "Fall";
            

          
            if (UserSession.DEPTIDOFADVISERVIEW != "")
                studentDept =UserSession.DEPTIDOFADVISERVIEW as string ;//dept adviser is viewing info about this dept
            else
                studentDept = UserSession.DEPARTMENT as string;


            try
            {
                model = ibizAdv.getMainCourses(studentDept, semester);
                ViewBag.electives = ibizAdv.getElectives();
            }
            catch (Exception ex)
            {
                ViewBag.status = ex.Message;
            }

            DateTime dt = DateTime.Now;
            string dts = dt.ToString("MM/dd/yyyy");
            string[] dtar = dts.Split('/');
            string month = dtar[0];
            string year = dtar[2];
            int intMonth = int.Parse(month);
          
           

            

            semesterValue.Add(new SelectListItem
            {
                Text = "Spring",
                Value = "1",
                

            });
            semesterValue.Add(new SelectListItem
            {
                Text = "Fall",
                Value = "2"
            });

            if (studentDept == "1")
                ViewBag.department = "Compuer Science";
            else if (studentDept == "2")
                ViewBag.department = "Computer Engineering";

            ViewBag.semesterList = semesterValue;
            ViewBag.semester = semester + year;
            
            return View(model);
        }


        public ActionResult semesterAdvisory(int? deptId)
        {
            int Dept=0;
            if (UserSession.DEPTIDOFADVISERVIEW !="")
                Dept =int.Parse(UserSession.DEPTIDOFADVISERVIEW);//convert nullable to normal int
            else if(UserSession.DEPARTMENT !="")
                Dept = int.Parse(UserSession.DEPARTMENT);

            if (deptId != null)
            {// use when recommendation is viewed directly
                Dept = deptId ?? default(int);
                UserSession.DEPTIDOFADVISERVIEW = Dept.ToString();//allow display of back to panel link
            }

           

            try
            {
                ViewBag.sem1 = ibizAdv.getCourseBySemester(1, Dept);
                ViewBag.sem2 = ibizAdv.getCourseBySemester(2, Dept);
                ViewBag.sem3 = ibizAdv.getCourseBySemester(3, Dept);
                ViewBag.sem4 = ibizAdv.getCourseBySemester(4, Dept);

                ViewBag.sem5 = ibizAdv.getCourseBySemester(5, Dept);
                ViewBag.sem6 = ibizAdv.getCourseBySemester(6, Dept);
                ViewBag.sem7 = ibizAdv.getCourseBySemester(7, Dept);
                ViewBag.sem8 = ibizAdv.getCourseBySemester(8, Dept);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if (Dept == 1)
                ViewBag.department = "Computer Science";
            else if (Dept == 2)
                ViewBag.department = "Computer Engineering";


                return View();
        }

        public ActionResult studentPreRequisite()
        {
            List<PreReqModel> preRequisites = new List<PreReqModel>();
            string studentId = "";

            if (UserSession.STUDENTINSUMMARY !="")
                studentId = UserSession.STUDENTINSUMMARY as string;//adviser is logged on and viewing student information
            else
                studentId = UserSession.USERNAME as string;

            try
            {
                preRequisites = ibizAdv.getPreRequistes(studentId);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            ViewBag.preReqCount = preRequisites.Count;
            return View(preRequisites);
        }


    }
}