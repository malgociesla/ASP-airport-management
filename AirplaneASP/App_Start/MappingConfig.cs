using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirportService.DTO;
using AirplaneASP.Models.Companies;

namespace AirplaneASP
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<CompanyDTO, CompanyModel>();
                config.CreateMap<CompanyModel, CompanyDTO>();
            }
                );
        }
    }
}