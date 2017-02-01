using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ExcelRowData : IEnumerable<ExcelCellData>
    {
        public List<ExcelCellData> DataRow { get; set; }

        public ExcelRowData()
        {
            DataRow = new List<ExcelCellData>();
        }

        public ExcelRowData(List<ExcelCellData> excelCellDataList)
        {
            DataRow = excelCellDataList;
        }

        public IEnumerator<ExcelCellData> GetEnumerator()
        {
            return DataRow.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return DataRow.GetEnumerator();
        }
    }
}
