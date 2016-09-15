using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace RegistrationAdvisory.Models
{
    class DataAccess : IDataAccess
    {
        string CONSTR = ConfigurationManager.ConnectionStrings["Advisory"].ConnectionString;

        public DataTable getDataTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(CONSTR);
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
           
            return dt;
        }

        public object getScarlar(string sql)
        {
            object value;
            SqlConnection con = new SqlConnection(CONSTR);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                value = cmd.ExecuteScalar();
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return value;
        }

        public int insertUpdateDelete(string sql)
        {
            int status;
            SqlConnection con = new SqlConnection(CONSTR);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                status=cmd.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return status;
        }
    }
}
