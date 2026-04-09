using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; // important to import ConfigruationManager Class

namespace MajorProject_HRMS_APP25.Models
{
    public class DBLayerClass
    {
       SqlConnection ConnectionObj ;
        public DBLayerClass() // Constructor to intialize ConnectionObj(SqlConnection)
        {
            ConnectionObj = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringGlobal"].ConnectionString);
        }

        // a UDF to perform insert, update, delete transaction in DB using parameters
        public int ExecuteIUD(string ProcedureName, SqlParameter[] ColumnNameparameters)
        {
            SqlCommand CommandObj = new SqlCommand(ProcedureName, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddRange(ColumnNameparameters);

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

        // a UDF to perform  select operation to single value from DB using parameters
        public object ExecuteScaler(string ProcedureName, SqlParameter[] ColumnNameparameters)
        {
            SqlCommand CommandObj = new SqlCommand(ProcedureName, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddRange(ColumnNameparameters);

            if (ConnectionObj.State == ConnectionState.Closed)
            {
                ConnectionObj.Open();
            }

            object result = CommandObj.ExecuteScalar();

            if (ConnectionObj.State == ConnectionState.Open)
            {
                ConnectionObj.Close();
            }

            return result;
        }

        // a UDF to perform single value select operation without parameters from DB
        public object ExecuteScaler(string ProcedureName)
        {
            SqlCommand CommandObj = new SqlCommand(ProcedureName, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;

            if (ConnectionObj.State == ConnectionState.Closed)
            {
                ConnectionObj.Open();
            }

            object result = CommandObj.ExecuteScalar();

            if (ConnectionObj.State == ConnectionState.Open)
            {
                ConnectionObj.Close();
            }

            return result;
        }

        // a UDF to execute select operation without parameters in DB that returns a result set
        public DataTable ExecuteSelect(string ProcedureName)
        {
            SqlCommand CommandObj = new SqlCommand(ProcedureName, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sda = new SqlDataAdapter(CommandObj);
            DataTable table = new DataTable();
            sda.Fill(table);

            return table;
        }


        // a UDF to execute select operation with parameters in DB that returns a result set
        public DataTable ExecuteSelect(string ProcedureName, SqlParameter[] ColumnNameParameters)
        {
            SqlCommand CommandObj = new SqlCommand(ProcedureName, ConnectionObj);
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddRange(ColumnNameParameters);

            SqlDataAdapter sda = new SqlDataAdapter(CommandObj);
            DataTable table = new DataTable();
            sda.Fill(table);

            return table;
        }
    }
}