using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MajorProject_HRMS_APP25.Models
{
    public class LeaveDataClass
    {
        public string LeaveSubjectAttribute { get; set; }
        public string LeaveDescriptionAttribute { get; set; }
        public DateTime FromDateAttribute { get; set; }
        public DateTime ToDateAtribute { get; set; }
        public int TotalDaysAttribute { get; set; }
        public string EmployeeIdAttribute { get; set; }
    }
}