﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows;

namespace Task1
{
    /// <summary>
    /// Provide operations with datatbase
    /// </summary>
    internal class SQLOperator
    {
        private string _connectionString;
        private string _datatable;
        public event EventHandler<int> AmountOfCopiedInfo;
        public event EventHandler<string> AvgMedianSQlResult;
        public SQLOperator(string tableName)
        {
            _datatable = tableName;
        }
        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
        //Create DataTable to work with sql
        private DataTable CreateTable()
        {
            DataTable _table = new DataTable();
            _table.Columns.Add("DateInfo", typeof(string));
            _table.Columns.Add("RandomLatin", typeof(string));
            _table.Columns.Add("RandomRu", typeof(string));
            _table.Columns.Add("RandomInt", typeof(string));
            _table.Columns.Add("RandomFloat", typeof(string));
            return _table; 
        }
        /// <summary>
        /// Copy file to the database. File should match the schema of datatable
        /// </summary>
        /// <param name="PathToFile"></param>
        internal void SQLCopeFileToDatabase(string PathToFile)
        {
            DataTable table = CreateTable();
            try
            {
                using (SqlConnection connector = new SqlConnection(_connectionString))
                {
                    string[] strings = File.ReadAllLines(PathToFile);
                    int TotalAmount = strings.Length;
                    int completedAmount = 0;
                    SqlBulkCopy copier = new SqlBulkCopy(connector);
                    copier.DestinationTableName = _datatable;
                    connector.Open();
                    List<DataRow> rows = new List<DataRow>();
                    while (TotalAmount > 0)
                    {    
                        rows = GetSQLFormattedStringsFromFile(table,completedAmount, 500000, rows, strings);
                        copier.WriteToServer(rows.ToArray());
                       
                        completedAmount += rows.Count;
                        AmountOfCopiedInfo.Invoke(this, completedAmount);
                    }
                    copier.Close();
                    connector.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }
        //Get amount of strings from file starts with startCount
        private List<DataRow> GetSQLFormattedStringsFromFile(DataTable table, int startCount, int AmountOfStrings, List<DataRow> result, string[] allFileStrings)
        {
           
            var strings = allFileStrings.Skip(startCount).Take(AmountOfStrings);
            int index = 0;
            foreach(string rowString in strings)
            {
                string[] newRowStrings = rowString.Split("||");
                if (table.Rows.Count < 1)
                {
                    DataRow row = table.NewRow();
                    row["DateInfo"] = newRowStrings[0];
                    row["RandomLatin"] = newRowStrings[1];
                    row["RandomRu"] = newRowStrings[2];
                    row["RandomInt"] = newRowStrings[3];
                    row["RandomFloat"] = newRowStrings[4];
                    result.Add(row);
                }
                else
                {
                    result[index]["DateInfo"] = newRowStrings[0];
                    result[index]["RandomLatin"] = newRowStrings[1];
                    result[index]["RandomRu"] = newRowStrings[2];
                    result[index]["RandomInt"] = newRowStrings[3];
                    result[index]["RandomFloat"] = newRowStrings[4];
                }
                index++;
            }
            if(result.Count > index)
            {
                return result.Take(index).ToList();
            }
            return result;
        }
        /// <summary>
        /// Calls a sql query and return the result as an event message
        /// </summary>
        /// <returns></returns>
        internal async Task GetAvgAndMedianFromDatabase()
        {
            string sqlString = $"SELECT ((SELECT MAX(RandomFloat) FROM (SELECT top 50 percent RandomFloat FROM {_datatable} Order By RandomFloat) as onehalf) + (SELECT MIN(RandomFloat) FROM (SELECT top 50 percent RandomFloat FROM {_datatable} Order By RandomFloat DESC) as otherhalf))/2 as median, AVG(CAST(RandomInt as bigint)) from {_datatable};";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlString, connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string result = $"Average int {reader.GetValue(0)}, Median Float: {reader.GetValue(1)}";
                    AvgMedianSQlResult?.Invoke(this, result);
                }
            }
        }
    }
}
