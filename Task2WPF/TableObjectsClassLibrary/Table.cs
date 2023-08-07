using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableObjectsClassLibrary
{
    /// <summary>
    /// Class that contains information about Table and list of records
    /// </summary>
    public class Table
    {
        public TableAddress TableInfo;
        public List<List<Record>> records { get; }

        public Table(TableAddress tableAddress, List<Record> recordsToAdd)
        {
            TableInfo = tableAddress;
            records = new List<List<Record>>();
            foreach (string columnName in TableInfo.ColumnNames)
            {
                records.Add(new List<Record>());
            }
            foreach (Record rec in recordsToAdd)
            {
                this.AddRecord(rec.ColumnName, rec.Value);
            }
        }
        public void AddRecord(string ColumnName, string recordValue)
        {
            try
            {
                records[TableInfo[ColumnName]].Add(new Record(recordValue,ColumnName));
            }
            catch
            {
                Console.WriteLine($"Error on adding {ColumnName}:{recordValue}");
            }
        }
    }
}
