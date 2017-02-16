using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;
using System.Data.Entity;
using Utils;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace AirportService
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleUtils _scheduleUtils;
        private readonly IScheduleParser _scheduleParser;
        private readonly AirportContext _airplaneContext;

        public ScheduleService(IScheduleUtils scheduleUtils, IScheduleParser scheduleParser)
        {
            _airplaneContext = new AirportContext();
            _scheduleUtils = scheduleUtils;
            _scheduleParser = scheduleParser;
        }

        public Guid Add(ScheduleDTO scheduleDTO)
        {
            try
            {
                if (scheduleDTO != null)
                {
                    Schedule schedule = new Schedule
                    {
                        IdFlight = scheduleDTO.FlightID,
                        IdFlightState = scheduleDTO.FlightStateID,
                        DepartureDT = scheduleDTO.DepartureDT,
                        ArrivalDT = scheduleDTO.ArrivalDT,
                        Comment = scheduleDTO.Comment
                    };
                    _airplaneContext.Schedules.Add(schedule);
                    _airplaneContext.SaveChanges();
                    return schedule.Id;
                }
                else
                {
                    throw new AirportServiceException("Couldn't add schedule. Provided data was invalid.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new AirportServiceException("Couldn't add schedule. Provided data was invalid.");
            }
        }

        public void Edit(ScheduleDTO scheduleDTO)
        {
            try
            {
                if (scheduleDTO != null)
                {
                    var schedule = _airplaneContext.Schedules.FirstOrDefault(c => c.Id == scheduleDTO.ID);
                    if (schedule != null)
                    {
                        schedule.IdFlight = scheduleDTO.FlightID;
                        schedule.IdFlightState = scheduleDTO.FlightStateID;
                        schedule.DepartureDT = scheduleDTO.DepartureDT;
                        schedule.ArrivalDT = scheduleDTO.ArrivalDT;
                        schedule.Comment = scheduleDTO.Comment;

                        _airplaneContext.SaveChanges();
                    }
                    else
                    {
                        throw new AirportServiceException("Couldn't edit schedule. Provided schedule doesn't exist.");
                    }
                }
                else
                {
                    throw new AirportServiceException("Couldn't edit schedule. Provided data was invalid.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new AirportServiceException("Couldn't edit schedule. Provided data was invalid.");
            }
        }

        public void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId)
        {
            if (startDate != null && endDate != null)
            {
                _airplaneContext.GenerateSchedule(startDate, endDate, flightId);
                _airplaneContext.SaveChanges();
            }
            else
            {
                throw new AirportServiceException("Couldn't generate schedule. Provided data was invalid.");
            }
        }

        public List<ScheduleDetailsDTO> GetAll()
        {
            var schedules = _airplaneContext
                .Schedules
                .ToList()
                .Select(s => new ScheduleDetailsDTO
                {
                    ID = s.Id,
                    FlightStateID = s.IdFlightState,
                    FlightID = s.IdFlight,
                    DepartureDT = s.DepartureDT,
                    ArrivalDT = s.ArrivalDT,
                    Comment = s.Comment,
                    CityDeparture = s.Flight.CityDeparture.Name,
                    CountryDeparture = s.Flight.CityDeparture.Country.Name,
                    CityArrival = s.Flight.CityArrival.Name,
                    CountryArrival = s.Flight.CityArrival.Country.Name,
                    Company = s.Flight.Company.Name
                });

            return schedules.ToList();
        }

        private IQueryable<ScheduleDetailsDTO> GetFilteredByDate(DateTime from, DateTime to)
        {
            var schedules = GetAllDefault()
                .Where(s =>
                        ((s.ArrivalDT >= from && s.ArrivalDT <= to)
                     && (s.DepartureDT >= from && s.DepartureDT <= to)));
            return schedules;
        }

        private IQueryable<ScheduleDetailsDTO> GetAllDefault()
        {
            var scheduleQ = _airplaneContext.Schedules
                    .Include(a => a.FlightState)
                    .OrderBy(s => s.DepartureDT)
                    .Select(s => new ScheduleDetailsDTO
                    {
                        ID = s.Id,
                        FlightStateID = s.IdFlightState,
                        FlightID = s.IdFlight,
                        DepartureDT = s.DepartureDT,
                        ArrivalDT = s.ArrivalDT,
                        Comment = s.Comment,
                        CityDeparture = s.Flight.CityDeparture.Name,
                        CountryDeparture = s.Flight.CityDeparture.Country.Name,
                        CityArrival = s.Flight.CityArrival.Name,
                        CountryArrival = s.Flight.CityArrival.Country.Name,
                        Company = s.Flight.Company.Name
                    });
            return scheduleQ;
        }

        public List<ScheduleDetailsDTO> GetList(int pageNumber,
            int pageSize,
            out int totalItemsCount,
            DateTime? from = null,
            DateTime? to = null)
        {
            IQueryable<ScheduleDetailsDTO> scheduleQ = null;
            List<ScheduleDetailsDTO> schedules = new List<ScheduleDetailsDTO>();

            if (from == null || to == null)
                scheduleQ = GetAllDefault();
            else
                scheduleQ = GetFilteredByDate((DateTime)from, (DateTime)to);

            schedules = scheduleQ
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToList();
            totalItemsCount = scheduleQ.ToList().Count;
            return schedules;
        }

        public void Remove(Guid id)
        {
            var schedule = _airplaneContext.Schedules.FirstOrDefault(s => s.Id == id);
            if (schedule != null)
            {
                _airplaneContext.Schedules.Remove(schedule);
                _airplaneContext.SaveChanges();
            }
            else
            {
                throw new AirportServiceException("Couldn't remove schedule. Provided data was invalid.");
            }
        }

        private int GetCountOfLandingPlanes(DateTime landingTime)
        {
            int landingPlanesCount;

            DateTime timeFrom = landingTime.AddMinutes(-4).AddSeconds(-59);
            DateTime timeTo = landingTime.AddMinutes(4).AddSeconds(59);

            landingPlanesCount = _airplaneContext.Schedules
                                                .Where(s => s.ArrivalDT > timeFrom &&
                                                            s.ArrivalDT < timeTo)
                                                .ToList()
                                                .Count();
            return landingPlanesCount;
        }

        //validate for plane collision before update
        //repeat ValidatePlaneCollision trigger behavior
        //to fix concurrency EF error caused by trigger on insert
        private DateTime IncrementTimeToAvoidPlaneCollision(DateTime arrivatDT)
        {
            //max 4 planes landing at the same time
            int maxLandingCapacity = 4;
            //check how many planes are landing at the same time
            DateTime timeCounter = arrivatDT;
            int planeCount = GetCountOfLandingPlanes(timeCounter);
            //if there are to many planes landing at the same time
            while (planeCount >= maxLandingCapacity)
            {
                //increment time by 5min
                timeCounter = timeCounter.AddMinutes(5);
                //check again for plane collision
                planeCount = GetCountOfLandingPlanes(timeCounter);
            }
            return timeCounter;
        }

        //update database
        //create new items in database, update old ones
        public void UpdateSchedule(List<ScheduleDTO> schedulesList)
        {
            try
            {
                if (schedulesList != null)
                {
                    var selectedSchedulesIds = schedulesList.Select(s => s.ID).ToList();
                    var existingSchedulesList = _airplaneContext.Schedules.Where(s => selectedSchedulesIds.Contains(s.Id)).ToList();
                    List<Schedule> newSchedulesList = new List<Schedule>();

                    foreach (ScheduleDTO newScheduleDTO in schedulesList)
                    {
                        Schedule existingSchedule = existingSchedulesList.FirstOrDefault(s => s.Id == newScheduleDTO.ID);
                        Schedule newSchedule = null;
                        if (existingSchedule == null)
                        //add new schedule
                        {
                            newSchedule = new Schedule
                            {
                                Id = newScheduleDTO.ID,
                                IdFlight = newScheduleDTO.FlightID,
                                IdFlightState = newScheduleDTO.FlightStateID,
                                DepartureDT = newScheduleDTO.DepartureDT,
                                ArrivalDT = IncrementTimeToAvoidPlaneCollision(newScheduleDTO.ArrivalDT.Value),
                                Comment = newScheduleDTO.Comment
                            };
                            newSchedulesList.Add(newSchedule);
                        }
                        else if (existingSchedule.Id == newScheduleDTO.ID)
                        //update exising schedule
                        {
                            existingSchedule.IdFlight = newScheduleDTO.FlightID;
                            existingSchedule.IdFlightState = newScheduleDTO.FlightStateID;
                            existingSchedule.DepartureDT = newScheduleDTO.DepartureDT;
                            existingSchedule.ArrivalDT = newScheduleDTO.ArrivalDT;
                            existingSchedule.Comment = newScheduleDTO.Comment;
                        }
                    }
                    if (schedulesList.Count > 0)
                    {
                        bool saveFailes;
                        do
                        {
                            saveFailes = false;
                            try
                            {
                                _airplaneContext.Schedules.AddRange(newSchedulesList);
                                _airplaneContext.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                saveFailes = true;
                                ex.Entries.Single().Reload();
                            }
                        } while (saveFailes);
                    }
                }
                else
                {
                    throw new AirportServiceException("No data was provided for update.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new AirportServiceException("Provided data was invalid.");
            }
        }

        public List<ScheduleDetailsDTO> Import(Stream excelStream)
        {
            try
            {
                return PrepareToExcelImport(_scheduleUtils.Read(excelStream));
            }
            catch (UtilsException ex)
            {
                throw new AirportServiceException(ex.Message,ex);
            }
        }

        public byte[] Export(List<ScheduleDetailsDTO> schedulesList)
        {
            try
            {
                return _scheduleUtils.Write(PrepareToExcelExport(schedulesList));
            }
            catch (UtilsException ex)
            {
                throw new AirportServiceException(ex.Message, ex);
            }
        }

        private List<ScheduleDetailsDTO> PrepareToExcelImport(ExcelData excelData)
        {
            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();
            if (excelData != null)
            {
                var schedules = excelData.DataRows.Select(s => _scheduleParser
                                                                .ParseDataRow(s)
                                                          )
                                                          .ToList();
                scheduleList.AddRange(schedules);
            }
            else
            {
                throw new AirportServiceException("Coudn't read data from source.");
            }

            return scheduleList;
        }

        private ExcelData PrepareToExcelExport(List<ScheduleDetailsDTO> schedulesList)
        {
            ExcelData excelData = new ExcelData();
            if (schedulesList != null)
            {
                excelData.HeadingRow = _scheduleParser.GenerateHeadingRow();

                var scheduleData = schedulesList.Select(s => _scheduleParser
                                                              .GenerateDataRow(s)
                                                       ).ToList();
                excelData.DataRows = scheduleData;
            }
            else
            {
                throw new AirportServiceException("No data was provided for export.");
            }
            return excelData;
        }
    }
}
