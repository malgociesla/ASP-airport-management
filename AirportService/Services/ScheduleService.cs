﻿using System;
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
                //throw error!!!
                return new Guid();
            }
        }

        public void Edit(ScheduleDTO scheduleDTO)
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
                }//else schedule doesn't exist
            }
            else
            {
                //throw ex
            }
        }

        public void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId)
        {
            _airplaneContext.GenerateSchedule(startDate, endDate, flightId);
            _airplaneContext.SaveChanges();
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

        //TODO: https://msdn.microsoft.com/en-us/data/jj592904
        public void UpdateSchedule(List<ScheduleDTO> schedulesList)
        {
            try
            {
                //update database
                //create new items in database, update old ones
                if (schedulesList != null)
                {
                    var existingScheduleIds = schedulesList.Select(s => s.ID).ToList();
                    var oldSchedules = _airplaneContext.Schedules.Where(s => existingScheduleIds.Contains(s.Id)).ToList();
                    List<Schedule> newSchedulesList = new List<Schedule>();

                    foreach (ScheduleDTO newScheduleDTO in schedulesList)
                    {
                        Schedule oldSchedule = oldSchedules.FirstOrDefault(s => s.Id == newScheduleDTO.ID); /*_airplaneContext.Schedules.FirstOrDefault(s => s.Id == newScheduleDTO.ID); //get whole list of items at once*/
                        Schedule newSchedule = null;
                        if (oldSchedule == null)
                        //add new scheduleItem
                        //work directly on airplanecontext add few items and save later
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
                        else if (oldSchedule.Id == newScheduleDTO.ID)
                        //edit oldSchedule from db
                        //work directly on airplanecontext add few items and save later
                        {
                            oldSchedule.IdFlight = newScheduleDTO.FlightID;
                            oldSchedule.IdFlightState = newScheduleDTO.FlightStateID;
                            oldSchedule.DepartureDT = newScheduleDTO.DepartureDT;
                            oldSchedule.ArrivalDT = newScheduleDTO.ArrivalDT;
                            oldSchedule.Comment = newScheduleDTO.Comment;
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
                    throw new AirportServiceException("No data was provided for update");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new AirportServiceException("Provided data was invalid");
            }
        }

        public List<ScheduleDetailsDTO> Import(Stream excelStream)
        {
            List<ScheduleDetailsDTO> importedList = new List<ScheduleDetailsDTO>();
            try
            {
                importedList = PrepareToExcelImport(_scheduleUtils.Read(excelStream));
            }
            catch (UtilsException ex)
            {
                throw new AirportServiceException("Couldn't read data from source.", ex);
            }
            return importedList;
        }

        public byte[] Export(List<ScheduleDetailsDTO> schedulesList)
        {
            return _scheduleUtils.Write(PrepareToExcelExport(schedulesList));
        }

        private List<ScheduleDetailsDTO> PrepareToExcelImport(ExcelData excelData)
        {
            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();

            var schedules = excelData.DataRows.Select(s => _scheduleParser
                                                            .ParseDataRow(s)
                                                      )
                                                      .ToList();
            scheduleList.AddRange(schedules);

            return scheduleList;
        }

        private ExcelData PrepareToExcelExport(List<ScheduleDetailsDTO> schedulesList)
        {
            ExcelData excelData = new ExcelData();
            try
            {
                excelData.HeadingRow = _scheduleParser.GenerateHeadingRow();

                var scheduleData = schedulesList.Select(s => _scheduleParser
                                                              .GenerateDataRow(s)
                                                       ).ToList();

                excelData.DataRows = scheduleData;
            }
            catch (Exception ex) { } //rethrow exception eg. too many columns or rows
            return excelData;
        }
    }
}
