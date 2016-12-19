using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airplaneCA
{
    interface IDataManager
    {
        void SetDataManager(IConnection conn);
        void Close();
        void Print();
        void PrintTable(string tableName);
        void GenerateSchedule(string startDate, string endDate);
    }
}
