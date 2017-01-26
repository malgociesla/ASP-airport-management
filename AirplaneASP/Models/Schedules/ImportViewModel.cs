using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AirplaneASP.ModelValidation;

namespace AirplaneASP.Models.Schedules
{
    public class ImportViewModel
    {
        public ImportViewModel()
        {
            ScheduleList = new List<ScheduleDetailsImportModel>();
        }
        public List<ScheduleDetailsImportModel> ScheduleList { get; set; }
        [ValidateFile(ErrorMessage = "Selected file is invalid")]
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}