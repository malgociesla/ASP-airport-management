using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Text.RegularExpressions;

namespace Utils
{
    public class ScheduleExcelUtils : IScheduleUtils
    {
        public ExcelData Read(Stream excelStream)
        {
            ExcelData excelData = new ExcelData();

            if (excelStream != null)
            {
                try
                {
                    using (var excelDoc = SpreadsheetDocument.Open(excelStream, false))
                    {
                        var rows = excelDoc.WorkbookPart.WorksheetParts.First()
                                .Worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                        var cells = excelDoc.WorkbookPart.WorksheetParts.First()
                                .Worksheet.GetFirstChild<SheetData>().Descendants<Cell>();

                        int rowsCount = rows.Count();
                        int cellsCount = cells.Count();
                        int rowSize = 'A' + (cellsCount/rowsCount);

                        int fromRowID = 1;
                        char fromColumnID = 'A';
                        for (int rowID = fromRowID; rowID <= rowsCount; rowID++)
                        {
                            ExcelRowData rowData = new ExcelRowData();
                            for (char columnID = fromColumnID; columnID < rowSize; columnID++)
                            {
                                string cellAddress = columnID + rowID.ToString();
                                ExcelCellData cellData = GetCellExcelCellData(excelDoc, cellAddress);
                                if (cellData!=null)
                                {
                                    rowData.DataRow.Add(cellData);
                                }
                            }
                            if (rowID == fromRowID)
                                excelData.HeadingRow = rowData;
                            else
                                excelData.DataRows.Add(rowData);
                        }


                    }
                }
                catch (Exception ex) { }//error sth went wrong with reading excel file
            }
            else { }//error stream empty
            
            return excelData;
        }

        public byte[] Write(ExcelData excelData)
        {
            if (excelData != null)
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

                        // Add minimal Stylesheet to format DateTime cells
                        var stylesPart = excelDoc.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                        stylesPart.Stylesheet = SetStyleSheet();

                        //insert data	
                        GenerateCells(excelData, sheetData);

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

        private Stylesheet SetStyleSheet()
        {
            Stylesheet styleSheet = new Stylesheet
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
            return styleSheet;
        }

        private List<Cell> GenerateCells(ExcelData excelData, SheetData sheetData)
        {
            List<Cell> cellList = new List<Cell>();
            var allDataRows = excelData.AllRows;

            int fromRowID = 1;
            char fromColumnID = 'A';

            int rowID = fromRowID;
            foreach (var dataRow in allDataRows) //single object
            {
                Row row = new Row()
                                    {
                                        RowIndex = (uint)rowID
                                    };

                char columnID = fromColumnID;
                foreach (var cellData in dataRow)
                {
                        string cellAddress = columnID + rowID.ToString();
                        Cell cell = SetCell(cellData.CellValue, GetCellType(cellData.CellDataType), cellAddress);
                        row.Append(cell);
                        cellList.Add(cell);

                        columnID++;  
                }
                sheetData.Append(row);
                rowID++;
            }
            return cellList;
        }

        private CellValues GetCellType(Type cellDataType)
        {
            if (cellDataType == typeof(DateTime)
                || cellDataType == typeof(DateTime?)) return CellValues.Number;
            else return CellValues.String;
        }

        private Type GetCellType(CellValues cellDataType)
        {
            //if (cellDataType == CellValues.String)
                return typeof(string);
        }

        private uint GetCellStyleIndex(CellValues cellDataType)
        {
            if (cellDataType == CellValues.Number) return 1;
            else return 0;
        }

        private Cell SetCell(string cellValue, CellValues cellDataType, string cellAddress)
        {
            Cell cell = new Cell()
            {
                CellValue = new CellValue(cellValue),
                DataType = new EnumValue<CellValues>(cellDataType),
                CellReference = cellAddress,
                StyleIndex = GetCellStyleIndex(cellDataType)
            };
            return cell;
        }

        private ExcelCellData GetCellExcelCellData(SpreadsheetDocument excelDoc, string cellAddress)
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

            CellValues cellDataType = CellValues.String;
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
                if (double.TryParse(cellValue, out cellValueDouble))
                {
                    cellDataType = CellValues.Number;
                    //try to parse value as date
                    try
                    {
                        cellValue = DateTime.FromOADate(cellValueDouble).ToString();
                        cellDataType = CellValues.Date;
                    }
                    catch (ArgumentException ex) { }// this wasn't valid date so it must be numeric value
                }
                //if cell isn't null -> assign value
                return new ExcelCellData()
                {
                    CellValue = cellValue,
                    CellDataType = GetCellType(cellDataType)
                };
            }
            return null;
        }
    }
}
