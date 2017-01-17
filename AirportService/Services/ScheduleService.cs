﻿using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;
using System.Data.Entity;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Text.RegularExpressions;

namespace AirportService
{
    public class ScheduleService : IScheduleService
    {
        private readonly AirportContext _airplaneContext;
        public ScheduleService()
        {
            _airplaneContext = new AirportContext();
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

        public void Import(List<ScheduleDTO> schedulesList)
        {
            //update database
            //create new items in database, update old ones
            if (schedulesList != null)
            {
                foreach (ScheduleDTO newSchedule in schedulesList)
                {
                    Schedule oldSchedule = _airplaneContext.Schedules.FirstOrDefault(s => s.Id == newSchedule.ID);
                    if (oldSchedule == null)
                        Add(newSchedule);
                    else if (oldSchedule.Id == newSchedule.ID)
                        Edit(newSchedule);
                }
            }
            else { } //error - list is empty
        }

        public List<ScheduleDetailsDTO> GetImportedList(Stream excelStream)
        {
            //read from stream and transform to list<schedule>
            //excelStream
            //updtade: check for nulls try/catch!!!!!!!!!!
            List<ScheduleDetailsDTO> list = new List<ScheduleDetailsDTO>();

            using (var excelDoc = SpreadsheetDocument.Open(excelStream, false))
            {
                //parse schedule items and return ScheduleList?
                var rows = excelDoc.WorkbookPart.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>().ChildElements.Skip(1);

                foreach (var row in rows)
                {
                    var cells = row.ChildElements;
                    {
                        ScheduleDetailsDTO item = new ScheduleDetailsDTO()
                        {
                            ID = new Guid(cells[0].InnerText),
                            FlightID = new Guid(cells[1].InnerText),
                            FlightStateID = new Guid(cells[2].InnerText),
                            CityDeparture = cells[3].InnerText.Substring(0,cells[3].InnerText.IndexOf(" (") + 1),
                            CountryDeparture = Regex.Match(cells[3].InnerText, @"\(([^)]*)\)").Groups[1].Value,
                            CityArrival = cells[4].InnerText.Substring(0, cells[4].InnerText.IndexOf(" (") + 1),
                            CountryArrival = Regex.Match(cells[4].InnerText, @"\(([^)]*)\)").Groups[1].Value,
                            DepartureDT = DateTime.Parse(cells[5].InnerText),
                            ArrivalDT = DateTime.Parse(cells[6].InnerText),
                            Company = cells[7].InnerText,
                            Comment = cells[8].InnerText
                        };
                        list.Add(item);
                    }
                }
            }
            return list;//CHANGE!
        }

        public byte[] ExportSchedule(List<ScheduleDetailsDTO> schedulesList)
        {
            if (schedulesList != null)
            {
                using (var templateStream = new MemoryStream())
                {
                    using (var excelDoc = SpreadsheetDocument.Create(templateStream, SpreadsheetDocumentType.Workbook, true))
                    {
                        // Add a WorkbookPart to the document.
                        WorkbookPart workbookpart = excelDoc.AddWorkbookPart();
                        workbookpart.Workbook = new Workbook();

                        // Add a WorksheetPart to the WorkbookPart.
                        WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                        SheetData sheetData = new SheetData();

                        //Insert headings for each info
                        Row headingRow = new Row() { RowIndex = 1 };
                        List<string> headingList = new List<string>()
                    {
                        "ScheduleID",
                        "FlightID",
                        "FlightStateID",
                        "From",
                        "To",
                        "Departure",
                        "Arrival",
                        "Company",
                        "Comment"
                    };
                        foreach (string heading in headingList)
                        {
                            Cell cell = new Cell { CellValue = new CellValue(heading), DataType = new EnumValue<CellValues>(CellValues.String) };
                            headingRow.AppendChild<Cell>(cell);
                        }
                        sheetData.Append(headingRow);

                        //
                        uint rowId = 2;
                        foreach (var item in schedulesList)
                        {
                            Row row = new Row() { RowIndex = rowId++ };
                            //"ScheduleID"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.ID.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"FlightID"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.FlightID.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"FlightStateID"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.FlightStateID.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"From"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.CityDeparture.ToString() + " (" + item.CountryDeparture.ToString() + ")"),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"To"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.CityArrival.ToString() + " (" + item.CountryArrival.ToString() + ")"),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"Departure"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.DepartureDT.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"Arrival"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.ArrivalDT.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"Company"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.Company.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });
                            //"Comment"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.Comment.ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.String)
                            });

                            sheetData.Append(row);
                        }

                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        // Add Sheets to the Workbook.
                        Sheets sheets = excelDoc.WorkbookPart.Workbook.
                            AppendChild<Sheets>(new Sheets());

                        // Append a new worksheet and associate it with the workbook.
                        Sheet sheet = new Sheet()
                        {
                            Id = excelDoc.WorkbookPart.
                            GetIdOfPart(worksheetPart),
                            SheetId = 1,
                            Name = "Schedules"
                        };
                        sheets.Append(sheet);

                    }
                    templateStream.Position = 0;
                    var result = templateStream.ToArray();
                    templateStream.Flush();

                    return result;
                }
            }
            //error - empty parameter scheduleList
            return null;
        }
    }
}
