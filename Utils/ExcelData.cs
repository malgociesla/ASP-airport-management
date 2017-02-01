using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ExcelData
    {
        private static readonly int _maxRows = 1000;
        private static readonly int _maxColumns = 26;
        private ExcelRowData _headingRow;
        public ExcelRowData HeadingRow
        {
            get
            {
                return _headingRow;
            }
            set
            {
                if(_headingRow.Count() <= _maxColumns)
                    _headingRow = value;
                //else throw error: too many columns!
            }
        }

        private List<ExcelRowData> _dataRows;
        public List<ExcelRowData> DataRows
        {
            get
            {
                return _dataRows;
            }
            set
            {
                if (value.Count() < _maxRows)
                    if (value.All(r => r.Count() <= _maxColumns))
                        _dataRows = value;    
                    //else throw error: to many columns            
                //else throw error: too many rows!
            }
        }

        private readonly List<ExcelRowData> _allRows;
        public List<ExcelRowData> AllRows
        {
            get
            {
                _allRows.Clear();
                _allRows.Add(this.HeadingRow);
                _allRows.AddRange(this.DataRows);
                return _allRows;
            }
        }
        public ExcelData()
        {
            this._headingRow = new ExcelRowData();
            this._dataRows = new List<ExcelRowData>();
            this._allRows = new List<ExcelRowData>();
        }
    }
}
