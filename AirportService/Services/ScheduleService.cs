using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;
using System.Data.Entity;
using Utils;
using System.IO;
using System.Text.RegularExpressions;

namespace AirportService
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleUtils _scheduleUtils;
        private readonly AirportContext _airplaneContext;

        public ScheduleService(IScheduleUtils scheduleUtils)
        {
            _airplaneContext = new AirportContext();
            _scheduleUtils = scheduleUtils;
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

        //TODO: https://msdn.microsoft.com/en-us/data/jj592904
        public void UpdateSchedule(List<ScheduleDTO> schedulesList)
        {
            //update database
            //create new items in database, update old ones
            if (schedulesList != null)
            {
                var existingScheduleIds = schedulesList.Select(s => s.ID).ToList();

                var oldSchedules = _airplaneContext.Schedules.Where(s => existingScheduleIds.Contains(s.Id)).ToList();

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
                            IdFlight = newScheduleDTO.FlightID,
                            IdFlightState = newScheduleDTO.FlightStateID,
                            DepartureDT = newScheduleDTO.DepartureDT,
                            ArrivalDT = newScheduleDTO.ArrivalDT,
                            Comment = newScheduleDTO.Comment
                        };
                        _airplaneContext.Schedules.Add(newSchedule);
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
                if (schedulesList.Count > 0) _airplaneContext.SaveChanges();
            }
            else { } //error - list is empty
        }

        public List<ScheduleDetailsDTO> Import(Stream excelStream)
        {
            return PrepareToImport(_scheduleUtils.Read(excelStream));
        }

        public byte[] Export(List<ScheduleDetailsDTO> schedulesList)
        {
            return _scheduleUtils.Write(PrepareToExcelExport(schedulesList));
        }

        private List<ScheduleDetailsDTO> PrepareToImport(List<List<Tuple<string, int>>> objList)
        {
            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();
            List<List<Tuple<string, int>>> objList1 = new List<List<Tuple<string, int>>>();

            //objList1[0].Contains(new List<Tuple<string, int>>()
            //                                                {
            //                                                    Tuple.Create(Constants.ScheduleID,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.FlightStateID,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.From,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.To,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.Departure,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.Arrival,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.Company,(int)AirportTypes.String),
            //                                                    Tuple.Create(Constants.Comment,(int)AirportTypes.String)
            //                                                });
            //skip headings

            try
            {
                var schedules = objList.Skip(1).Select(s =>
                                                       new ScheduleDetailsDTO()
                                                       {
                                                           ID = new Guid(s[0].Item1),
                                                           FlightID = new Guid(s[1].Item1),
                                                           FlightStateID = new Guid(s[2].Item1),
                                                           CityDeparture = s[3].Item1.Substring(0, s[3].Item1.IndexOf(" (") + 1),
                                                           CountryDeparture = Regex.Match(s[3].Item1, @"\(([^)]*)\)").Groups[1].Value,
                                                           CityArrival = s[4].Item1.Substring(0, s[4].Item1.IndexOf(" (") + 1),
                                                           CountryArrival = Regex.Match(s[4].Item1, @"\(([^)]*)\)").Groups[1].Value,
                                                           DepartureDT = DateTime.Parse(s[5].Item1),
                                                           ArrivalDT = DateTime.Parse(s[6].Item1),
                                                           Company = s[7].Item1,
                                                           Comment = s[8].Item1,
                                                       }
                                                 ).ToList();
                scheduleList.AddRange(schedules);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // throw error
                // error with parsing file - couldn't parse data from file
            }

            return scheduleList;
        }

        private List<List<Tuple<string, int>>> PrepareToExport(List<ScheduleDetailsDTO> schedulesList)
        {
            List<List<Tuple<string, int>>> schedulesDictList = new List<List<Tuple<string, int>>>();
            schedulesDictList.Add(new List<Tuple<string, int>>()
                                                    {
                                                                Tuple.Create(Constants.ScheduleID,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.FlightID,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.FlightStateID,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.From,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.To,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.Departure,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.Arrival,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.Company,(int)AirportTypes.String),
                                                                Tuple.Create(Constants.Comment,(int)AirportTypes.String)
                                                    }
                                                );

            var schedule = schedulesList.Select(s =>
                                                    new List<Tuple<string, int>>()
                                                    {
                                                                Tuple.Create(s.ID.ToString(),(int)AirportTypes.String),
                                                                Tuple.Create(s.FlightID.ToString(),(int)AirportTypes.String),
                                                                Tuple.Create(s.FlightStateID.ToString(),(int)AirportTypes.String),
                                                                Tuple.Create(s.CityDeparture.ToString() + " (" + s.CountryDeparture.ToString() + ")",(int)AirportTypes.String),
                                                                Tuple.Create(s.CityArrival.ToString() + " (" + s.CountryArrival.ToString() + ")",(int)AirportTypes.String),
                                                                Tuple.Create(s.DepartureDT.Value.ToOADate().ToString(),(int)AirportTypes.Number),
                                                                Tuple.Create(s.ArrivalDT.Value.ToOADate().ToString(),(int)AirportTypes.Number),
                                                                Tuple.Create(s.Company.ToString(),(int)AirportTypes.String),
                                                                Tuple.Create(s.Comment.ToString(),(int)AirportTypes.String)
                                                    }
                                                ).ToList();

            schedulesDictList.AddRange(schedule);

            return schedulesDictList;
        }

        private ExcelData PrepareToExcelExport(List<ScheduleDetailsDTO> schedulesList)
        {
            ExcelData excelData = new ExcelData();
            try
            {
                excelData.HeadingRow = new ExcelRowData(new List<ExcelCellData>()
                                                {
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.ScheduleID,
                                                                    CellDataType = Constants.ScheduleID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.FlightID,
                                                                    CellDataType = Constants.FlightID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.FlightStateID,
                                                                    CellDataType = Constants.FlightStateID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.From,
                                                                    CellDataType = Constants.From.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.To,
                                                                    CellDataType = Constants.To.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.Departure,
                                                                    CellDataType = Constants.Departure.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.Arrival,
                                                                    CellDataType = Constants.Arrival.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.Company,
                                                                    CellDataType = Constants.Company.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = Constants.Comment,
                                                                    CellDataType = Constants.Comment.GetType()
                                                                }

                                                }
                                        );

                var scheduleData = schedulesList.Select(s =>
                                                            new ExcelRowData(
                                                                new List<ExcelCellData>()
                                                                {
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.ID.ToString(),
                                                                    CellDataType = s.ID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.FlightID.ToString(),
                                                                    CellDataType = s.FlightID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.FlightStateID.ToString(),
                                                                    CellDataType = s.FlightStateID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.CityDeparture.ToString() + " (" + s.CountryDeparture.ToString() + ")",
                                                                    CellDataType = s.CityDeparture.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.CityArrival.ToString() + " (" + s.CountryArrival.ToString() + ")",
                                                                    CellDataType = s.CityArrival.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.DepartureDT.Value.ToOADate().ToString(),
                                                                    CellDataType = s.DepartureDT.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.ArrivalDT.Value.ToOADate().ToString(),
                                                                    CellDataType = s.ArrivalDT.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.Company.ToString(),
                                                                    CellDataType = s.Company.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = s.Comment.ToString(),
                                                                    CellDataType = s.Comment.GetType()
                                                                }
                                                                }
                                                             )
                                                          ).ToList();

                excelData.DataRows = scheduleData;
            }
            catch (Exception ex) { } //rethrow exception eg. too many columns or rows
            return excelData;
        }
    }
}
