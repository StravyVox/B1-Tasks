using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableObjectsClassLibrary
{
    public static class DataTableOperations
    {
       
        /// <summary>
        /// Create List of Table objects, where you can check table address and records. Table contain records only attached to classAddress object
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="tablesToSeek"></param>
        /// <param name="classAddress"></param>
        /// <returns></returns>
        public static List<Table> GetTablesForClass(DataTable dataset, List<TableAddress> tablesToSeek, ClassAddress classAddress)
        {
            int startRow = classAddress.RowStartInclusive;
            int endRow = classAddress.RowEndInclusive;
            List<Table> tables = new List<Table>();

            foreach (TableAddress tableAddress in tablesToSeek)
            {
                List<Record> records = GetRecordsForTable(dataset, tableAddress, startRow, endRow - startRow);
                tables.Add(new Table(tableAddress, records));
            }

            return tables;
        }
        /// <summary>
        /// Gets row adresses and seek in them for table Headers and also Subheaders on the second row after StartRow
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="startRow"></param>
        /// <param name="ColumnsSize"></param>
        /// <returns></returns>
        public static List<TableAddress> GetTableAdressesFromDataSet(DataTable dataset, int startRow, int ColumnsSize)
        {
            List<TableAddress> tables = new List<TableAddress>();
            var tableNames = dataset.Rows[startRow].ItemArray;
            string tableName = null;
            int columnsAmount = 1;

            //Seek for Headers
            for (int col = 0; col < ColumnsSize; col++)
            {
                if (tableNames[col].ToString().Length > 0 && (tableName != tableNames[col].ToString() || tableName == null)) //Checks whether cell is null or not and also checks if Header new and should be added to list
                {
                    tableName = tableNames[col].ToString();
                    TableAddress tableAddress = new TableAddress() { TableName = tableName, ColumnStart = col, Size = 1, ColumnNames = new List<string>(), RowStart = startRow + 2 };
                    tables.Add(tableAddress);
                    columnsAmount = 1;
                }
                else
                {
                    columnsAmount++;
                    TableAddress table = tables[tables.Count - 1];
                    table.Size = columnsAmount;
                    tables[tables.Count - 1] = table;

                }
            }
            tableNames = dataset.Rows[startRow + 1].ItemArray; //Seek for SubHeaders
            foreach (TableAddress tableAddress in tables)
            {
                List<string> columnNames = new List<string>();
                for (int i = tableAddress.ColumnStart; i < tableAddress.ColumnStart + tableAddress.Size; ++i)
                {
                    if (tableNames[i].ToString().Length > 0)
                    {
                        columnNames.Add(tableNames[i].ToString());
                    }
                }
                if (columnNames.Count > 0)
                {
                    tableAddress.ColumnNames.AddRange(columnNames.ToArray());
                }
                else
                {
                    tableAddress.ColumnNames.Add(tableAddress.TableName);
                }
            }
            return tables;
        }
        /// <summary>
        /// Gets records for the given table from start row
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="tableToSeek"></param>
        /// <param name="StartRow"></param>
        /// <param name="amountOfRecords"></param>
        /// <returns></returns>
        public static List<Record> GetRecordsForTable(DataTable dataset, TableAddress tableToSeek, int StartRow, int amountOfRecords)
        {
            List<Record> records = new List<Record>();

            for (int i = StartRow; i < StartRow + amountOfRecords; i++)
            {
                var values = dataset.Rows[i].ItemArray;
                for (int columnIndex = tableToSeek.ColumnStart; columnIndex < tableToSeek.ColumnStart + tableToSeek.Size; columnIndex++)
                {
                    if (values[columnIndex].ToString()?.Count() > 0)
                    {

                        records.Add(new Record(values[columnIndex].ToString(), tableToSeek[columnIndex]));
                    }
                }
            }
            return records;
        }
        /// <summary>
        /// Finds Classes and gets their rows
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="RowToStart"></param>
        /// <param name="ColumnToSeek"></param>
        /// <param name="wordToSeek"></param>
        /// <returns></returns>
        public static List<ClassAddress> GetClasses(DataTable dataset, int ColumnToSeek, string wordToSeek)
        {
            List<ClassAddress> classes = new List<ClassAddress>();
            if (dataset != null)
            {
                for (int i = 0; i < dataset.Rows.Count; i++)
                {
                    if ((dataset.Rows[i].ItemArray[ColumnToSeek] as string) != null && (dataset.Rows[i].ItemArray[ColumnToSeek] as string).Contains(wordToSeek))
                    {
                        if (classes.Count != 0) classes[classes.Count - 1].RowEndInclusive = i;
                        classes.Add(new ClassAddress() { NameOfClass = dataset.Rows[i].ItemArray[ColumnToSeek].ToString(), RowStartInclusive = i + 1 });
                    }
                }
                if (classes.Count > 0) classes[classes.Count - 1].RowEndInclusive = dataset.Rows.Count - 1;
                return classes;
            }
            return null;
        }
        
    }
}
