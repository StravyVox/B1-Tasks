using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableObjectsClassLibrary
{
    /// <summary>
    /// Class that provides info about the table. Such as name of table, amount and names of columns, size of table
    /// </summary>
    public class TableAddress
    {
        private string _tableName;
        private int _rowStart;
        private int _columnStart;
        private int _size;
        private List<string> _columnNames;

        public string this[int columnAddress]
        {
            get
            {
                return ColumnNames[columnAddress - ColumnStart];
            }
        }
        public int this[string index]
        {
            get
            {
                return ColumnNames.FindIndex((str) => str == index);

            }
        }

        public List<string> ColumnNames { get => _columnNames; set => _columnNames = value; }
        public string TableName { get => _tableName; set => _tableName = value; }
        public int RowStart { get => _rowStart; set => _rowStart = value; }
        public int ColumnStart { get => _columnStart; set => _columnStart = value; }
        public int Size { get => _size; set => _size = value; }
    }
}
