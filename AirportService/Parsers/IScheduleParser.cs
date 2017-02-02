using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public interface IScheduleParser
    {
        ScheduleDetailsDTO ParseDataRow(ExcelRowData excelRowData);
        ExcelRowData GenerateDataRow(ScheduleDetailsDTO schedule);
        ExcelRowData GenerateHeadingRow();
    }
}
