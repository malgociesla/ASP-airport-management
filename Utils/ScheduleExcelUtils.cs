using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Text.RegularExpressions;
using AirportService.DTO;

namespace Utils
{
    public class ScheduleExcelUtils
    {
        public List<ScheduleDetailsDTO> GetImportedList(Stream excelStream)
        {
            List<ScheduleDetailsDTO> list = new List<ScheduleDetailsDTO>();
            if (excelStream != null)
            {
                try
                {
                    using (var excelDoc = SpreadsheetDocument.Open(excelStream, false))
                    {
                        //get count rows
                        //skip heading row
                        int rowsCount = excelDoc.WorkbookPart.WorksheetParts.First()
                                                        .Worksheet.GetFirstChild<SheetData>()
                                                        .ChildElements.Skip(1).Count();

                        //iterate through each row to get cell values
                        //get cell range [Ai,Ii], i[2,rowsCount]
                        int fromRange = 2;
                        int toRange = fromRange + rowsCount;
                        for (int i = fromRange; i < toRange; i++)
                        {
                            ScheduleDetailsDTO item = new ScheduleDetailsDTO()
                            {
                                ID = new Guid(GetCellValue(excelDoc, "A" + i)),
                                FlightID = new Guid(GetCellValue(excelDoc, "B" + i)),
                                FlightStateID = new Guid(GetCellValue(excelDoc, "C" + i)),
                                CityDeparture = GetCellValue(excelDoc, "D" + i).Substring(0, GetCellValue(excelDoc, "D" + i).IndexOf(" (") + 1),
                                CountryDeparture = Regex.Match(GetCellValue(excelDoc, "D" + i), @"\(([^)]*)\)").Groups[1].Value,
                                CityArrival = GetCellValue(excelDoc, "E" + i).Substring(0, GetCellValue(excelDoc, "E" + i).IndexOf(" (") + 1),
                                CountryArrival = Regex.Match(GetCellValue(excelDoc, "E" + i), @"\(([^)]*)\)").Groups[1].Value,
                                DepartureDT = DateTime.Parse(GetCellValue(excelDoc, "F" + i)),
                                ArrivalDT = DateTime.Parse(GetCellValue(excelDoc, "G" + i)),
                                Company = GetCellValue(excelDoc, "H" + i),
                                Comment = GetCellValue(excelDoc, "I" + i)
                            };
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex) { }//error sth went wrong with reading excel file
            }
            else { }//error stream empty
            return list;
        }

        public byte[] Export(List<ScheduleDetailsDTO> schedulesList)
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
                        // Add minimal Stylesheet to format DateTime cells
                        var stylesPart = excelDoc.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                        stylesPart.Stylesheet = new Stylesheet
                        {
                            Fonts = new Fonts(new Font()),
                            Fills = new Fills(new Fill()),
                            Borders = new Borders(new Border()),
                            CellStyleFormats = new CellStyleFormats(new CellFormat()),
                            CellFormats =
                                new CellFormats(
                                    new CellFormat(), //StyleIndex=0
                                    new CellFormat    //StyleIndex=1 (for DateTime)
                                    {
                                        NumberFormatId = 22,
                                        ApplyNumberFormat = true
                                    })
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
                                CellValue = new CellValue(item.DepartureDT.Value.ToOADate().ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.Number),
                                StyleIndex = 1
                            });
                            //"Arrival"
                            row.AppendChild<Cell>(new Cell
                            {
                                CellValue = new CellValue(item.ArrivalDT.Value.ToOADate().ToString()),
                                DataType = new EnumValue<CellValues>(CellValues.Number),
                                StyleIndex = 1
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
        private string GetCellValue(SpreadsheetDocument excelDoc, string cellAddress)
        {
            //get string table in case the cell value is not an accual value,
            //but an index of sharedstringtable, where the value is stored by excel
            var stringTable = excelDoc.WorkbookPart
                                      .GetPartsOfType<SharedStringTablePart>()
                                      .FirstOrDefault();
            Cell thisCell = excelDoc.WorkbookPart
                                   .WorksheetParts.First()
                                   .Worksheet.Descendants<Cell>()
                                   .Where(c => c.CellReference == cellAddress).FirstOrDefault();

            var cellValue = ""; //if cell in given range == null, treat it like an empty string
                                //get value for each cell
            if (thisCell != null)
            {
                cellValue = thisCell.InnerText;
                double cellValueDouble = 0;
                if (thisCell.DataType != null)
                {
                    //if cell value is an index and not an accual value
                    if (thisCell.DataType.Value == CellValues.SharedString)
                    {
                        if (stringTable != null)
                        {
                            //get an accual cell's value
                            cellValue = stringTable.SharedStringTable
                                               .ElementAt(int.Parse(cellValue))
                                               .InnerText;
                        }
                    }
                }
                //DateType is null for numerics and date values
                else if (double.TryParse(cellValue, out cellValueDouble))
                {
                    //try to parse value as date
                    try
                    {
                        cellValue = DateTime.FromOADate(cellValueDouble).ToString();
                    }
                    catch (ArgumentException ex) { }// this wasn't valid date so it must be numeric value
                }
            }
            return cellValue;
        }
    }
}
