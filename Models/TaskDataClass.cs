using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MajorProject_HRMS_APP25.Models
{
    public class TaskDataClass
    {
        public string SelectEmployeeAttribute { get; set; }
        public string TaskSubjectAttribute { get; set; }
        public string TaskDescriptionAttribute { get; set; }
        public string SelectPriorityAttribute { get; set; }
        public DateTime DueDateAttribute { get; set; }
        public HttpPostedFileBase AttachmentAttribute { get; set; }
    }
}