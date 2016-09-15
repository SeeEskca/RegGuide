using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RegistrationAdvisory.Models
{
    public class RepositoryHelper
    {
        public RepositoryHelper() { }

        public static List<T> convertDataTableToList<T>(DataTable dt)
           where T : IEntity, new()
        {
            List<T> Tlist = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {

                T pd = new T();//declare model
                pd.setFields(dr);//assign table fields to model properties
                Tlist.Add(pd);//add model to list
                              //i++;
            }
           

            return Tlist;

        }

        public static string[] convertDataRowToList(DataTable dt)
        {
            string[] cList=null;// = new List<CourseModel>();
           
            string courseId;
            string courseName;
            string credit;
            string offering;
           
                foreach(DataRow dr in dt.Rows)
                {
                    courseId = (string)dr["CourseId"];
                    courseName = (string)dr["CourseDesc"];
                    credit = dr["Credit"].ToString();
                    offering = (string)dr["SemesterOffered"];
                    cList = new string[]
                    {
                       courseId,//read only
                       courseName,
                       credit,
                       offering
                    };

                }

            return cList;
            
        }
    }
}
