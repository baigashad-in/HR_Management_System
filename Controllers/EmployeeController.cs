using MajorProject_HRMS_APP25.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MajorProject_HRMS_APP25.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        DBLayerClass dBLayerObj = new DBLayerClass();
        DataLayerClass dtLayerObj = new DataLayerClass();
        MailMessengerClass OTPmail = new MailMessengerClass();

        // GET: Employee


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string EmployeeUserNameAttribute, string EmployeePasswordAttribute)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@EmployeeId", EmployeeUserNameAttribute)
            };
            DataTable dt = dBLayerObj.ExecuteSelect("SP_GetEmployeePasswordHash", parametersToInsert);
            if (dt.Rows.Count > 0)
            {
                string storedHash = dt.Rows[0]["EmployeePassword"].ToString();
                if (PasswordHelper.VerifyPassword(EmployeePasswordAttribute, storedHash))
                {
                    FormsAuthentication.SetAuthCookie(EmployeeUserNameAttribute, false);
                    return RedirectToAction("Dashboard");
                }
            }
            TempData["Error"] = "Incorrect UserId or Password";
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string EmployeeIdVerifyAttribute)
        {
            Random rnd = new Random();
            int OTP = rnd.Next(100000, 999999);

            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@EmployeeId", EmployeeIdVerifyAttribute)
            };

            object res = dBLayerObj.ExecuteScaler("SP_ForgotPassword", parametersToInsert);

            if (res != null)
            {
                string subject_passing = "Regarding OTP to reset your login credentials.";
                string body_passing = $@"<html><head></head><body>
                        <h2>Welcome to the Company</h2>
                        Welcome {EmployeeIdVerifyAttribute},
                        <br>

                        <p>Your OTP is </p>
                        <br>
                        <p>{OTP}</p>
                        
                        <br>
                        <br>

                        Thank You
                        <br>
                        <br>
                        Warm Regards

                        </body></html> ";

                OTPmail.SendMail(res.ToString(), subject_passing, body_passing);
            }
            else
            {
                return Content($"<script>{res.ToString()}');location.href='/Employee/ForgotPassword'</script>");
            }

                Session["OTP"] = OTP;
            return Content($"<script>alert('OTP sent to {res}');location.href='/Employee/VerifyOTP'</script>");
        }

        [AllowAnonymous]
        public ActionResult VerifyOTP()
        {
            return View();
        }

        [AllowAnonymous, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyOTP(string VerifyOtpAttribute)
        {
            string SentOTP = Session["OTP"].ToString();

            if (SentOTP == VerifyOtpAttribute)
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                return Content("<script>alert('Incoreect OTP. Kindly Re-Enter');location.href='/Employee/VerifyOTP'</script>");
            }

        }

        [AllowAnonymous]
        public ActionResult ChangeForgotPasswordEmployee()
        {
            return View();
        }

        [AllowAnonymous, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeForgotPasswordEmployee(string ConfirmForgotPasswordAttribute, string VerifyOtpAttribute)
        {
            string SentOTP = Session["OTP"].ToString();

            SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@EmployeeId", User.Identity.Name),
                    new SqlParameter("@NewPassword", ConfirmForgotPasswordAttribute)
                };
            int res = dBLayerObj.ExecuteIUD("SP_ChangeForgotPasswordEmployee", parametersToInsert);
            if ((res > 0) && (SentOTP == VerifyOtpAttribute))
            {
                return Content("<script>alert('Password Change Successfull');location.href='/Employee/Login'</script>");
            }
            else
            {
                return Content("<script>alert('Password Change Failed');location.href='/Employee/ChangeForgotPasswordEmployee'</script>");
            }

        }

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ApplyForLeave()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyForLeave(LeaveDataClass LeaveDataObj)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Leave_Subject", LeaveDataObj.LeaveSubjectAttribute),
                new SqlParameter("@Leave_Details", LeaveDataObj.LeaveDescriptionAttribute),
                new SqlParameter("@From_Date", LeaveDataObj.FromDateAttribute.ToString("yyyy-MM-dd")),
                new SqlParameter("@To_Date", LeaveDataObj.ToDateAtribute.ToString("yyyy-MM-dd")),
                new SqlParameter("@Total_Days", LeaveDataObj.TotalDaysAttribute),
                new SqlParameter("@Leave_Status", "Pending"),
                new SqlParameter("@EmployeeId", User.Identity.Name)
            };

            int res = dBLayerObj.ExecuteIUD("SP_ApplyLeave", parametersToInsert);
            if (res > 0)
            {
                return Content("<script>alert('Applied for Leave Successfully');location.href='/Employee/Dashboard'</script>");
            }
            else
            {
                return Content("<script>alert('Application for Leave Failed. Try Again');location.href='/Employee/Dashboard'</script>");
            }
        }
        public ActionResult MyProfile()
        {
            return View();
        }
        public ActionResult LeaveMgnmt()
        {
            DataTable dt = dtLayerObj.SelectMyLeave(User.Identity.Name); // so that User.Identity.Name sends current logged-in employee's employeeId
            return View(dt);
        }
        
        public ActionResult MyTask()
        {
            DataTable dt1 = dtLayerObj.SelectMyTodayTask(User.Identity.Name);
            DataTable dt2 = dtLayerObj.SelectMyTask(User.Identity.Name);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);

            ViewBag.TaskData = ds;
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}