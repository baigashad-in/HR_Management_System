using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MajorProject_HRMS_APP25.Models
{
    public class DataLayerClass
    {
        DBLayerClass db = new DBLayerClass();

        // Select List of all Employees in descending order of Joining
        public DataTable SelectAllEmployee()
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@Action",2)
                };
            DataTable dt = db.ExecuteSelect("SP_HRMS_Procedure", parametersToInsert);
            return dt;
        }

        // Select list of all task assigned today to the employee

        public DataTable SelectTodaysTask()
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action", 1)
            };
            DataTable dt = db.ExecuteSelect("SP_SelectTaskReport", parametersToInsert);
            return dt;
        }
        public DataTable SelectAllTask()
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action", 2)
            };
            DataTable dt = db.ExecuteSelect("SP_SelectTaskReport", parametersToInsert);
            return dt;
        }
        public DataTable SelectMyTodayTask(string EmployeeId)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action", 3),
                new SqlParameter("@EmployeeId", EmployeeId)
            };
            DataTable dt = db.ExecuteSelect("SP_SelectTaskReport", parametersToInsert);
            return dt;
        }
        public DataTable SelectMyTask(string EmployeeId)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action", 4),
                new SqlParameter("@EmployeeId", EmployeeId)
            };
            DataTable dt = db.ExecuteSelect("SP_SelectTaskReport", parametersToInsert);
            return dt;
        }
        

        public DataTable SelectMyLeave(string EmployeeId)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@EmployeeId", EmployeeId)
            };
            DataTable dt = db.ExecuteSelect("SP_SelectMyLeave", parametersToInsert);
            return dt;
        }
    }
}