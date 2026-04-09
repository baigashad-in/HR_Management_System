using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MajorProject_HRMS_APP25.Models
{
    public class EmployeeDataClass
    {

        //public int EmployeeSrAttribute {  get; set; }

        [Required]
        public string EmployeeNameAttribute {  get; set; }
        public string EmployeeGenderAttribute {  get; set; }
        public DateTime EmployeeDOBAttribute {  get; set; }
        public long EmployeeContactNumberAttribute {  get; set; }
        public string EmployeePersonalEmailAttribute {  get; set; }
        public string EmployeeOfficialEmailAttribute {  get; set; }
        public string EmployeeAddressAttribute {  get; set; }
        public long EmployeeAadharNumberAttribute {  get; set; }

        [Required]
        public string EmployeeIdAttribute {  get; set; }
        public string EmployeeRoleAttribute {  get; set; }
        public string EmployeeDesignationAttribute {  get; set; }
        public DateTime EmployeeDOJAttribute {  get; set; }
        public string EmployeeBankNameAttribute { get; set; }
        public string EmployeePANAttribute {  get; set; }
        public string EmployeeAccountNumberAttribute {  get; set; }
        public string EmployeeIFSCcodeAttribute {  get; set; }

        [Required]
        public int EmployeeSalaryAttribute {  get; set; }
        
    }
}