using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AirplaneASP.Models.Schedules;

namespace AirplaneASP.ModelValidation
{
    public class FromToDateAttribute : ValidationAttribute
    {
        public FromToDateAttribute()
        {
            ErrorMessage = "Please enter both fields: from date and to date";
        }

        public override bool IsValid(object value)
        {
            if (value is FilterViewModel)
            {
                FilterViewModel filter = (FilterViewModel)value;
                DateTime? from = filter.FromDate;
                DateTime? to = filter.ToDate;
                if (!(from == null && to == null))
                {
                    //check dates
                    if (from <= to)
                        return true;
                    else return false;
                }
                else return true; 
            }
            return false; //type not handled by validation
        }
    }
}