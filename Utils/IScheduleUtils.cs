using System;
using System.Collections.Generic;
using System.IO;

namespace Utils
{
    public interface IScheduleUtils
    {
        List<List<Tuple<string, int>>> Read(Stream excelStream);
        byte[] Write(List<List<Tuple<string, int>>> schedulesList);
    }
}
