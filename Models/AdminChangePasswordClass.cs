using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MajorProject_HRMS_APP25.Models
{
    public class AdminChangePasswordClass
    {
        public string OldPasswordAttribute { get; set; }
        public string NewPasswordAttribute { get; set; }
        public string ConfirmPasswordAttribute { get; set; }
    }
}