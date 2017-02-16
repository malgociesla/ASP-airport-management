using System.IO;

namespace Utils
{
    public interface IScheduleUtils
    {
        ExcelData Read(Stream excelStream);
        byte[] Write(ExcelData excelData);
    }
}
