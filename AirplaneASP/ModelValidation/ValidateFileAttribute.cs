using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AirplaneASP.Models.Schedules;

namespace AirplaneASP.ModelValidation
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName) && (file.ContentType == "application/vnd.ms-excel" || file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                return true;
            else if (file == null)
                return true;
            else
                return false;
        }
    }
}