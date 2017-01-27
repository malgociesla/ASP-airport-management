using System.Collections.Generic;
using System.IO;

namespace Utils
{
    public interface IScheduleUtils
    {
        //List<List<Dictionary<string, int>>> Read(Stream excelStream);
        byte[] Write(List<List<Dictionary<string, int>>> schedulesList);
    }
}
