﻿using AirplaneASP.Loggers;
using System.Web;
using System.Web.Mvc;

namespace AirplaneASP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
