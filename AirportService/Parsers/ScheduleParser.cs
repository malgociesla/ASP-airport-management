using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using AirportService.DTO;
using System.Text.RegularExpressions;

namespace AirportService
{
    public class ScheduleParser : IScheduleParser
    {
        public ScheduleDetailsDTO ParseDataRow(ExcelRowData excelRowData)
        {
            ScheduleDetailsDTO schedule = null;
            try
            {
                schedule = new ScheduleDetailsDTO()
                {
                    ID = new Guid(excelRowData.DataRow[0].CellValue),
                    FlightID = new Guid(excelRowData.DataRow[1].CellValue),
                    FlightStateID = new Guid(excelRowData.DataRow[2].CellValue),
                    CityDeparture = excelRowData.DataRow[3].CellValue.Substring(0, excelRowData.DataRow[3].CellValue.IndexOf(" (") + 1),
                    CountryDeparture = Regex.Match(excelRowData.DataRow[3].CellValue, @"\(([^)]*)\)").Groups[1].Value,
                    CityArrival = excelRowData.DataRow[4].CellValue.Substring(0, excelRowData.DataRow[4].CellValue.IndexOf(" (") + 1),
                    CountryArrival = Regex.Match(excelRowData.DataRow[4].CellValue, @"\(([^)]*)\)").Groups[1].Value,
                    DepartureDT = DateTime.Parse(excelRowData.DataRow[5].CellValue),
                    ArrivalDT = DateTime.Parse(excelRowData.DataRow[6].CellValue),
                    Company = excelRowData.DataRow[7].CellValue,
                    Comment = excelRowData.DataRow[8].CellValue,
                };
            }
            catch (ArgumentOutOfRangeException ex) { }//error "Couldn't parse data"
            catch (FormatException ex) { } //error "Couldn't parse data"

            return schedule;
        }

        public ExcelRowData GenerateDataRow(ScheduleDetailsDTO schedule)
        {
            ExcelRowData excelRowData = new ExcelRowData()
            {
                DataRow = new List<ExcelCellData>()
                                                   {
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.ID.ToString(),
                                                                    CellDataType = schedule.ID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.FlightID.ToString(),
                                                                    CellDataType = schedule.FlightID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.FlightStateID.ToString(),
                                                                    CellDataType = schedule.FlightStateID.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.CityDeparture.ToString() + " (" + schedule.CountryDeparture.ToString() + ")",
                                                                    CellDataType = schedule.CityDeparture.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.CityArrival.ToString() + " (" + schedule.CountryArrival.ToString() + ")",
                                                                    CellDataType = schedule.CityArrival.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.DepartureDT.Value.ToOADate().ToString(),
                                                                    CellDataType = schedule.DepartureDT.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.ArrivalDT.Value.ToOADate().ToString(),
                                                                    CellDataType = schedule.ArrivalDT.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.Company.ToString(),
                                                                    CellDataType = schedule.Company.GetType()
                                                                },
                                                                new ExcelCellData()
                                                                {
                                                                    CellValue = schedule.Comment.ToString(),
                                                                    CellDataType = schedule.Comment.GetType()
                                                                }
                                                    }
            };

            return excelRowData;
        }
        public ExcelRowData GenerateHeadingRow()
        {
            ExcelRowData excelHearingData = new ExcelRowData(new List<ExcelCellData>()
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
            return excelHearingData;
        }
    }
}
