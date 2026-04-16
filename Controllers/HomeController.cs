using MajorProject_HRMS_APP25.Models;
using System;
using System.Collections.Generic;
using System.Data; // ADO.NET class
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Management; // AD.NET class
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;

namespace MajorProject_HRMS_APP25.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        MailMessengerClass mailObj = new MailMessengerClass();
        DBLayerClass dbLayerObj = new DBLayerClass();
        DataLayerClass dataLayerObj = new DataLayerClass();

        #region Admin Login
        [AllowAnonymous]
        public ActionResult Index() // Admin Login
        {
            return View();
        }
        [AllowAnonymous, HttpPost] // Admin Login
        [ValidateAntiForgeryToken]
        public ActionResult Index(string AdminUserNameAttribute, string AdminPasswordAttribute)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
           {
                new SqlParameter("@EmployeeId", AdminUserNameAttribute)
           };
            DataTable dt = dbLayerObj.ExecuteSelect("SP_GetAdminPasswordHash", parametersToInsert);
            if (dt.Rows.Count > 0)
            {
                string storedHash = dt.Rows[0]["EmployeePassword"].ToString();
                if (PasswordHelper.VerifyPassword(AdminPasswordAttribute, storedHash))
                {
                    FormsAuthentication.SetAuthCookie(AdminUserNameAttribute, false);
                    return RedirectToAction("Dashboard");
                }
                
            }
            TempData["Error"] = "Incorrect UserId or Password";
            return RedirectToAction("Index");
            
        }
        #endregion

        #region Dashboard
        public ActionResult Dashboard()
        {

            return View();
        }
        #endregion

        #region AddEmployee
        public ActionResult AddEmployee()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(EmployeeDataClass EmployeeObj)
        {
            Random rnd = new Random();
            int password = rnd.Next(10000, 99999);
            string hashedPassword = PasswordHelper.HashPassword(password.ToString());
            try
            {
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@EmployeeName", EmployeeObj.EmployeeNameAttribute),
                    new SqlParameter("@EmployeeGender", EmployeeObj.EmployeeGenderAttribute),
                    new SqlParameter("@EmployeeDOB", EmployeeObj.EmployeeDOBAttribute),
                    //new SqlParameter("@EmployeeDOB", EmployeeObj.EmployeeDOBAttribute.ToString("yyyy-MM-dd")),
                    new SqlParameter("@EmployeeContactNumber", EmployeeObj.EmployeeContactNumberAttribute),
                    new SqlParameter("@EmployeePersonalEmail", EmployeeObj.EmployeePersonalEmailAttribute),
                    new SqlParameter("@EmployeeOfficialEmail", EmployeeObj.EmployeeOfficialEmailAttribute),
                    new SqlParameter("@EmployeeAddress", EmployeeObj.EmployeeAddressAttribute),
                    new SqlParameter("@EmployeeAadharNumber", EmployeeObj.EmployeeAadharNumberAttribute),
                    new SqlParameter("@EmployeeId", EmployeeObj.EmployeeIdAttribute),
                    new SqlParameter("@EmployeeRole", EmployeeObj.EmployeeRoleAttribute),
                    new SqlParameter("@EmployeeDesignation", EmployeeObj.EmployeeDesignationAttribute),
                    new SqlParameter("@EmployeeDOJ", EmployeeObj.EmployeeDOJAttribute),
                    //new SqlParameter("@EmployeeDOJ", EmployeeObj.EmployeeDOJAttribute.ToString("yyyy-MM-dd hh:mm:ss")),
                    new SqlParameter("@EmployeePAN", EmployeeObj.EmployeePANAttribute),
                    new SqlParameter("@EmployeeAccountNumber", EmployeeObj.EmployeeAccountNumberAttribute),
                    new SqlParameter("@EmployeeBankName", EmployeeObj.EmployeeBankNameAttribute),
                    new SqlParameter("@EmployeeIFSCcode", EmployeeObj.EmployeeIFSCcodeAttribute),
                    new SqlParameter("@EmployeeSalary", EmployeeObj.EmployeeSalaryAttribute),
                    new SqlParameter("@EmployeePassword", hashedPassword),
                    new SqlParameter("@Action",1)

                };

                object ScalarResult = dbLayerObj.ExecuteScaler("SP_HRMS_Procedure", parametersToInsert);

                if (ScalarResult.Equals("Record Added Successfuly. - SQL Msg"))
                {
                    string subject_passing = "Regarding onboarding and your login credentials.";
                    string body_passing = $@"<html><head></head><body>
                        <h2>Welcome to the Company</h2>
                        Welcome {EmployeeObj.EmployeeNameAttribute},
                        <br>

                        <p>We are excited to welcome you to the our team! Your onboarding is now complete, and your login credentials are ready to use.</p>
                        <br>
                        <p><b>Login Id : </b> {EmployeeObj.EmployeeIdAttribute}</p>
                        <p><b>Password : </b> {password}</p>
                        <br>
                        <br>

                        Thank You
                        <br>
                        <br>
                        Warm Regards

                        </body></html> ";

                    mailObj.SendMail(EmployeeObj.EmployeePersonalEmailAttribute, subject_passing, body_passing);
                }


                return Content($"<script>alert('{ScalarResult}');location.href='/Home/EmployeeMngmt'</script>");

                //int result = dbLayerObj.ExecuteIUD("SP_HRMS_Procedure", parametersToInsert);
                //if (result > 0)
                //{
                //    return Content("<script>alert('Data Inserted Successfully');location.href='/Home/ListEmployee'</script>");
                //}
                //else
                //{
                //    return Content("<script>alert('Data Insert Failed');location.href='/Home/AddEmployee'</script>");
                //}
            }
            catch (Exception ex) 
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        #endregion

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListEmployee()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@Action",2)
                };
                dt = dbLayerObj.ExecuteSelect("SP_HRMS_Procedure", parametersToInsert);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(dt);
        }

        //[CustomAuthorization]
        public ActionResult EmployeeMngmt()
        {
            DataTable dt = dataLayerObj.SelectAllEmployee();
            return View(dt);
        }
        public ActionResult GetEmployeeById(string OldEmployeeId)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@Action",3),
                    new SqlParameter("@EmployeeId", OldEmployeeId)
                };
                 dt = dbLayerObj.ExecuteSelect("SP_HRMS_Procedure", parametersToInsert);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(dt.Rows[0]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeeById(string OldEmployeeId, EmployeeDataClass EmployeeObj)
        {
            try
            {
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@Action",4),
                    new SqlParameter("@EmployeeName", EmployeeObj.EmployeeNameAttribute),
                    new SqlParameter("@EmployeeGender", EmployeeObj.EmployeeGenderAttribute),
                    new SqlParameter("@EmployeeDOB", EmployeeObj.EmployeeDOBAttribute),
                    new SqlParameter("@EmployeeContactNumber", EmployeeObj.EmployeeContactNumberAttribute),
                    new SqlParameter("@EmployeePersonalEmail", EmployeeObj.EmployeePersonalEmailAttribute),
                    new SqlParameter("@EmployeeOfficialEmail", EmployeeObj.EmployeeOfficialEmailAttribute),
                    new SqlParameter("@EmployeeAddress", EmployeeObj.EmployeeAddressAttribute),
                    new SqlParameter("@EmployeeAadharNumber", EmployeeObj.EmployeeAadharNumberAttribute),
                    new SqlParameter("@EmployeeDesignation", EmployeeObj.EmployeeDesignationAttribute),
                    new SqlParameter("@EmployeeDOJ", EmployeeObj.EmployeeDOJAttribute),
                    new SqlParameter("@EmployeePAN", EmployeeObj.EmployeePANAttribute),
                    new SqlParameter("@EmployeeAccountNumber", EmployeeObj.EmployeeAccountNumberAttribute),
                    new SqlParameter("@EmployeeBankName", EmployeeObj.EmployeeBankNameAttribute),
                    new SqlParameter("@EmployeeSalary", EmployeeObj.EmployeeSalaryAttribute),
                    new SqlParameter("@EmployeeId", OldEmployeeId)
                };
                int res = dbLayerObj.ExecuteIUD("SP_HRMS_Procedure", parametersToInsert);

                if (res > 0)
                {
                    return Content("<script>alert('Data Updated Successfully');location.href='/Home/EmployeeMngmt'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Update Failed');location.href='/Home/EmployeeMngmt'</script>");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }
        public ActionResult DeleteEmployeeById(string OldEmployeeId)
        {
            try
            {
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@Action",5),
                    new SqlParameter("@EmployeeId", OldEmployeeId)
                };

                int res = dbLayerObj.ExecuteIUD("SP_HRMS_Procedure", parametersToInsert);

                if (res > 0)
                {
                    return Content("<script>alert('Data Deleted Successfully');location.href='/Home/EmployeeMngmt'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Delete Failed');location.href='/Home/EmployeeMngmt'</script>");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteEmployeeByIdJson(string EmployeeIdToController)
        {
            //try
            //{
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@EmployeeId", EmployeeIdToController)
                };

                int res = dbLayerObj.ExecuteIUD("SP_DeleteEmployeeUsingAJAX", parametersToInsert);

                return Json(res, JsonRequestBehavior.AllowGet);

                //if (res > 0)
                //{
                //    return Content("<script>alert('Data Deleted Successfully');location.href='/Home/EmployeeMngmt'</script>");
                //}
                //else
                //{
                //    return Content("<script>alert('Data Delete Failed');location.href='/Home/EmployeeMngmt'</script>");
                //}

            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Error = ex.Message;
            //}

            //return View();
        }

        public ActionResult AssignTask()
        {
            // Select All Employees list
            DataTable dt1 = dataLayerObj.SelectAllEmployee();
            //return View(dt1);

            // Select Today's Task
            DataTable dt2 = dataLayerObj.SelectTodaysTask();
            //return View(dt2);


            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);

            ViewBag.Data = ds;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTask(TaskDataClass TaskDataObj)
        {
            // Select All Employees list
            DataTable dt1 = dataLayerObj.SelectAllEmployee();
            //return View(dt1);

            // Select Today's Task
            DataTable dt2 = dataLayerObj.SelectTodaysTask();
            //return View(dt2);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            ViewBag.Data = ds;

            string savedFileName = null;
            if (TaskDataObj.AttachmentAttribute != null && TaskDataObj.AttachmentAttribute.ContentLength > 0)
            {           
                string originalFileName = Path.GetFileName(TaskDataObj.AttachmentAttribute.FileName);
                string extension = Path.GetExtension(originalFileName).ToLower();

                // Whitelist allowed extensions
                string[] allowedExtensions = { ".pdf", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".jpeg" };
                if (!allowedExtensions.Contains(extension))
                {
                    return Content("<script>alert('File type not allowed');location.href='/Home/AssignTask'</script>\");
                }
                

                // Limit file size to 5MB
                if (TaskDataObj.AttachmentAttribute.ContentLength > 5 * 1024 * 1024)
                {
                    return Content("<script>alert('File too large. Max 5MB.');location.href='/Home/AssignTask'</script>");
                }

                // Rename GUID to prevent overwriting and path traversal
                string safeFileName = Guid.NewGuid().ToString() + extension;
                string folderPath = Server.MapPath("~/Content/TaskFiles/");
                

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, safeFileName);

                TaskDataObj.AttachmentAttribute.SaveAs(filePath);
                savedFileName = safeFileName;
            }

            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@EmployeeId", TaskDataObj.SelectEmployeeAttribute),
                new SqlParameter("@TaskSubject", TaskDataObj.TaskSubjectAttribute),
                new SqlParameter("@TaskPriority", TaskDataObj.SelectPriorityAttribute),
                new SqlParameter("@TaskDueDate", TaskDataObj.DueDateAttribute),
                new SqlParameter("@TaskAttachment", savedFileName),
                new SqlParameter("@TaskDescription", TaskDataObj.TaskDescriptionAttribute)
            };
            int res = dbLayerObj.ExecuteIUD("SP_AddTask", parametersToInsert);
            if (res > 0)
            {
                ViewBag.Data = ds;
                //Session["UserData"] =
                return Content("<script>alert('Data Inserted Successfully');location.href='/Home/ListTask'</script>");
            }
            else
            {
                return Content("<script>alert('Data instertion Failed');location.href='/Home/AssignTask'</script>");
            }
        }

        public ActionResult ListTask()
        {
            DataTable dt = dataLayerObj.SelectAllTask();
            return View(dt);
        }
        public ActionResult AdminLeaveMngmt()
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action",1)
            };
            DataTable dt = dbLayerObj.ExecuteSelect("SP_SelectAllLeave", parametersToInsert);
            return View(dt);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AdminLeaveMngmt(string AdminCommentAttribute, string StatusViaAdminAttribute, string EmployeeIdToController)
        //{
        //    SqlParameter[] parametersToInsert = new SqlParameter[]
        //    {
        //        new SqlParameter("@Action",1)
        //    };
        //    DataTable dt = dbLayerObj.ExecuteSelect("SP_SelectAllLeave", parametersToInsert);
        //    return View(dt);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptLeave(string AdminCommentToController, string StatusViaAdminToController, string SrToController)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action","Accept"),
                new SqlParameter("@Leave_Status", "Accepted"),
                new SqlParameter("@Status_via_Admin", StatusViaAdminToController),
                new SqlParameter("@Status_Comment", AdminCommentToController),
                new SqlParameter("@Sr", SrToController)
            };
            int res = dbLayerObj.ExecuteIUD("SP_AcceptRejectLeave", parametersToInsert);

            if (res > 0)
            {
                return Json(new { keyname = "success", result = res });
                //return Content("<script>alert('Leave Accepted Successfully');location.href='/Home/AdminLeaveMngmt'</script>");
            }
            else
            {
                return Json(new { keyname = "failed", result = res });
                //return Content("<script>alert('Failed to Accept Leave');location.href='/Home/AdminLeaveMngmt'</script>");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectLeave(string AdminCommentToController, string SrToController)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
            {
                new SqlParameter("@Action","Reject"),
                new SqlParameter("@Leave_Status", "Rejected"),
                new SqlParameter("@Status_Comment", AdminCommentToController),
                new SqlParameter("@Sr", SrToController)
            };
            int res = dbLayerObj.ExecuteIUD("SP_AcceptRejectLeave", parametersToInsert);

            if (res > 0)
            {
                return Json(new { keyname = "success", result = res });
                //return Content("<script>alert('Leave Rejected Successfully');location.href='/Home/AdminLeaveMngmt'</script>");
            }
            else
            {
                return Json(new { keyname = "error" });
                //return Content("<script>alert('Failed to Reject Leave');location.href='/Home/AdminLeaveMngmt'</script>");
            }

        }

        
        public ActionResult GetLeaveById(string OldLeaveId)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@Action",2),
                    new SqlParameter("@EmployeeId", OldLeaveId)
                };
                dt = dbLayerObj.ExecuteSelect("SP_SelectAllLeave", parametersToInsert);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(dt.Rows[0]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLeaveById()
        {
            DataTable dt = dbLayerObj.ExecuteSelect("SP_SelectAllLeave");
            return View(dt);
        }
        public ActionResult DeleteLeaveById()
        {
            DataTable dt = dbLayerObj.ExecuteSelect("SP_SelectAllLeave");
            return View(dt);
        }
        public ActionResult ChangePasswordAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordAdmin(AdminChangePasswordClass AdminChangePasswordObj)
        {
            SqlParameter[] parametersToInsert = new SqlParameter[]
                {
                    new SqlParameter("@EmployeeId", User.Identity.Name),
                    new SqlParameter("@OldPassword", AdminChangePasswordObj.OldPasswordAttribute),
                    new SqlParameter("@NewPassword", AdminChangePasswordObj.NewPasswordAttribute)
                };
            int res = dbLayerObj.ExecuteIUD("SP_AdminChangePassword", parametersToInsert);
            if (res > 0)
            {
                return Content("<script>alert('Password Change Successfull');location.href='/Home/AddEmployee'</script>");
            }
            else
            {
                return Content("<script>alert('Password Change Failed');location.href='/Home/ChangePasswordAdmin'</script>");
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
           return RedirectToAction("Index");
        }
    }
}