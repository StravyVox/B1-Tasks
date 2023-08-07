using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableObjectsClassLibrary
{

    /// <summary>
    /// Class, that provides functions to convert Tables objects to Datatables
    /// </summary>
    public static class DataTableConverter
    {

        /// <summary>
        /// Adds ExternalKey (new column) with given key for the tables
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ColumnName"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<DataTable> SetExternalKeyToDataTables(string key, string ColumnName, List<DataTable> tables)
        {
            foreach (DataTable table in tables)
            {
                AddExternalKey(table, key, ColumnName);
            }
            return tables;
        }
        /// <summary>
        /// Adds ExternalKey (new column) with given keys for the tables
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="ColumnName"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<DataTable> SetExternalKeyToDataTables(List<string> keys, string ColumnName, List<DataTable> tables)
        {
            foreach (DataTable table in tables)
            {
               AddExternalKey(table, keys, ColumnName);
            }
            return tables;
        }
        /// <summary>
        /// Adds ExternalKey (new column) with given keys for the tables
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="ColumnName"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<DataTable> SetExternalKeyToDataTables(List<Record> keys, string ColumnName, List<DataTable> tables)
        {
            List<string> keySet = new List<string>();
            foreach(Record record in keys)
            {
                keySet.Add(record.Value);
            }
            foreach (DataTable table in tables)
            {
                AddExternalKey(table, keySet, ColumnName);
            }
            return tables;
        }
        public static List<DataTable> ConvertTablesToDataTables(List<Table> tablesForClass)
        {
            List<DataTable> list = new List<DataTable>();
            foreach (Table table in tablesForClass)
            {
                list.Add(ConvertToDataTable(table));
            }
            return list;
        }

        /// <summary>
        /// Read data from Table object and returns Datatable object
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(Table table)
        {
            DataTable result = new DataTable();
            List<DataRow> rowList = new List<DataRow>();
            result.TableName = table.TableInfo.TableName;
            foreach (string column in table.TableInfo.ColumnNames)
            {
                result.Columns.Add(column, typeof(string));
            }
            for (int i = 0; i < table.records[0].Count; i++)
            {
                rowList.Add(result.NewRow());
                for (int column = 0; column < table.records.Count; column++)
                {
                    rowList[rowList.Count - 1][table.TableInfo.ColumnNames[column]] = table.records[column][i].Value;
                }
                result.Rows.Add(rowList[rowList.Count-1]);
            }
            return result;
        }
        /// <summary>
        /// Adds ExternalKey (new column) with given keys for the table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keys"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DataTable? AddExternalKey(DataTable table, List<string> keys, string columnName)
        {
            if (table.Rows.Count == keys.Count)
            {
                table.Columns.Add(columnName, typeof(string));
                for (int i = 0; i < keys.Count; i++)
                {
                    table.Rows[i][columnName] = keys[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Adds ExternalKey (new column) with given key for the table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DataTable? AddExternalKey(DataTable table, string key, string columnName)
        {
                table.Columns.Add(columnName, typeof(string));
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i][columnName] = key;
                }
            return table;
        }
    }
}
