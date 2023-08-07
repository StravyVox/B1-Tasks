using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableObjectsClassLibrary
{
    /// <summary>
    /// Class to safe the data about record. value of record and column name
    /// </summary>
    public class Record
    {
        private string _value;
        private string _columnName;

        public Record(string value, string columnName)
        {
            this._value = value;
            this._columnName = columnName;
        }

        public string Value { get => _value; set => this._value = value; }
        public string ColumnName { get => _columnName; set => _columnName = value; }
    }
}
