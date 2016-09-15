using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RegistrationAdvisory.Models
{
    public class UserSession
    {
        private static readonly string _studentId = "USERNAME";
        private static readonly string _department = "DEPARTMENT";
        private static readonly string _userId = "USERID";
        private static readonly string _userRole = "USERROLE";
        private static readonly string _roleName = "ROLENAME";
        private static readonly string _PAGE_REQUESTED = "PAGEREQUESTED";
        private static readonly string _DisplayName = "DISPLAYNAME";
        private static readonly string _adviserViewId = "DEPTIDOFADVISERVIEW";
        private static readonly string _modifyCat = "COURSECATEGORYTOMODIFY";
        private static readonly string _studentSumary = "STUDENTINSUMMARY";
        private static readonly string _studentName = "STUDENTINSUMMARYNAME";
        private static readonly string _browserBackButton = "BACKBUTTON";
        private static readonly string _isinsert = "ISINSERT";


        public static string USERNAME
        {
            get
            {
                string content = "";
                if(HttpContext.Current.Session[_studentId] != null)
                    content = (string)HttpContext.Current.Session[_studentId];
                return content;

            }
            set
            {
                HttpContext.Current.Session[_studentId] = value;
            }
        }

        public static string DEPARTMENT
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_department] != null)
                    content = (string)HttpContext.Current.Session[_department];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_department] = value;
            }
        }

        public static string USERID
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_userId] != null)
                    content = (string)HttpContext.Current.Session[_userId];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_userId] = value;
            }
        }

        public static string USERROLE
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_userRole] != null)
                    content = (string)HttpContext.Current.Session[_userRole];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_userRole] = value;
            }
        }

        public static string PAGEREQUESTED
        {
            get
            {
                string result = "";
                if (HttpContext.Current.Session[_PAGE_REQUESTED] != null)
                    result = (string)HttpContext.Current.Session[_PAGE_REQUESTED];
                return result;
            }
            set
            {
                HttpContext.Current.Session[_PAGE_REQUESTED] = value;
            }
        }


        public static string DISPLAYNAME
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_DisplayName] != null)
                    content = (string)HttpContext.Current.Session[_DisplayName];
                return content;

            }
            set
            {
                HttpContext.Current.Session[_DisplayName] = value;
            }
        }

        public static string ROLENAME
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_roleName] != null)
                    content = (string)HttpContext.Current.Session[_roleName];
                return content;

            }
            set
            {
                HttpContext.Current.Session[_roleName] = value;
            }
        }

        public static string DEPTIDOFADVISERVIEW
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_adviserViewId] != null)
                    content = (string)HttpContext.Current.Session[_adviserViewId];
                return content;

            }
            set
            {
                HttpContext.Current.Session[_adviserViewId] = value;
            }
      
        }


        public static string COURSECATEGORYTOMODIFY
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_modifyCat] != null)
                    content = (string)HttpContext.Current.Session[_modifyCat];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_modifyCat] = value;
            }
        }

        public static string STUDENTINSUMMARY
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_studentSumary] != null)
                    content = (string)HttpContext.Current.Session[_studentSumary];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_studentSumary] = value;
            }
        }

        public static string STUDENTINSUMMARYNAME
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_studentName] != null)
                    content = (string)HttpContext.Current.Session[_studentName];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_studentName] = value;
            }
        }

        public static string BACKBUTTON
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_browserBackButton] != null)
                    content = (string)HttpContext.Current.Session[_browserBackButton];
                return content;
            }
            set
            {
                HttpContext.Current.Session[_browserBackButton] = value;
            }
        }


        public static string ISINSERT
        {
            get
            {
                string content = "";
                if (HttpContext.Current.Session[_isinsert] != null)
                    content =HttpContext.Current.Session[_isinsert].ToString();
                return content;
            }
            set
            {
                HttpContext.Current.Session[_isinsert] = value;
            }
        }
    }
}
