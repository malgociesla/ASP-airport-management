using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportService;
using AirportService.DTO;

namespace air
{
    class Program
    {
        static void Main(string[] args)
        {
            ICityService cityService = new CityService();
            List<CityDTO> objlist= cityService.GetCities();
            Guid id = cityService.AddCity("mycity");
            List<CityDTO> objlist2 = cityService.GetCities();
            Guid id2 = new Guid("01E0D1E0-F0C1-E611-B353-D017C293D790");
            cityService.Edit(id,"secondName");
            List<CityDTO> objlist4 = cityService.GetCities();
            cityService.Remove(id);
            List<CityDTO> objlist3 = cityService.GetCities();
        }
    }
}
