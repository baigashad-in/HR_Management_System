using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MajorProject_HRMS_APP25.Models
{
    public class DBManager
    {
        SqlConnection ConnectionObj;

        public DBManager()
        {
            ConnectionObj = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringGlobal"].ConnectionString);
        }

        public int ExecuteManagerIUD(string StoredProcedure, SqlParameter[] ManagerColumnParametes)
        {
            SqlCommand CommandObj = new SqlCommand(StoredProcedure, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddRange(ManagerColumnParametes);

            if (ConnectionObj.State == ConnectionState.Closed)
            {
                ConnectionObj.Open();
            }

            int result = CommandObj.ExecuteNonQuery();

            if (ConnectionObj.State == ConnectionState.Open)
            {
                ConnectionObj.Close();
            }

            return result;
        }

        public object ExecuteManagerScalar(string StoredProcedure, SqlParameter[] ManagerColumnParameters)
        {
            SqlCommand CommandObj = new SqlCommand(StoredProcedure, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddRange(ManagerColumnParameters);

            if(ConnectionObj.State == ConnectionState.Closed)
            {
                ConnectionObj.Open();
            }

            object result = CommandObj.ExecuteScalar();

            if(ConnectionObj.State == ConnectionState.Open)
            {
                ConnectionObj.Close();
            }

            return result;

        }

        public object ExecuteMangerScalar(string StoredProcedure)
        {
            SqlCommand CommandObj = new SqlCommand(StoredProcedure, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;

            if(ConnectionObj.State == ConnectionState.Closed)
            {
                ConnectionObj.Open();
            }

            object result = CommandObj.ExecuteScalar();

            if( ConnectionObj.State == ConnectionState.Open)
            {
                ConnectionObj.Close();
            }

            return result;
        }

        public DataTable ExecuteManagerSelect(string StoredProcedure, SqlParameter[] ManagerColumnParameters)
        {
            SqlCommand CommandObj = new SqlCommand(StoredProcedure, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddRange(ManagerColumnParameters);

            SqlDataAdapter adapter = new SqlDataAdapter(CommandObj);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        public DataTable ExecuteManagerSelect(string StoredProcedure)
        {
            SqlCommand CommandObj = new SqlCommand(StoredProcedure,ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter adapter = new SqlDataAdapter(CommandObj);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;

        }
    }
}