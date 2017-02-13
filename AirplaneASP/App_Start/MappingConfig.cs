using AirplaneASP.Models.Cities;
using AirplaneASP.Models.Companies;
using AirplaneASP.Models.Countries;
using AirplaneASP.Models.Flights;
using AirplaneASP.Models.Schedules;
using AirportService.DTO;
using AutoMapper;

namespace AirplaneASP
{
    public static class MappingConfig
    {
        public static MapperConfiguration ConfigureMappings()
        {
            //TODO: automapper -> if list null - new model (model can't be null)
            return new MapperConfiguration(c =>
            {
                //Company
                c.CreateMap<CompanyDTO, CompanyModel>();
                c.CreateMap<CompanyModel, CompanyDTO>();
                //Country
                c.CreateMap<CountryDTO, CountryModel>();
                c.CreateMap<CountryModel, CountryDTO>();
                //City
                c.CreateMap<CityDTO, CityModel>();
                c.CreateMap<CityModel, CityDTO>();
                //Flight
                c.CreateMap<FlightDTO, FlightModel>();
                c.CreateMap<FlightModel, FlightDTO>();
                //Schedule
                c.CreateMap<ScheduleDTO, ScheduleModel>();
                c.CreateMap<ScheduleModel, ScheduleDTO>();
                c.CreateMap<ScheduleDetailsDTO, ScheduleDetailsImportModel>();
                c.CreateMap<ScheduleDetailsImportModel, ScheduleDetailsDTO>();
            });
        }
    }
}