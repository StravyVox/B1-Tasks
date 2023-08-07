using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TableObjectsClassLibrary;
using Microsoft.Data.SqlClient;
namespace Task2WPF
{
    public class SQLOperator
    {
        private SqlConnection _sqlConnection; 
        public event EventHandler<int> ResultOfSQLInputOperations;
        public SQLOperator(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }
        public void InputSingleRecord(string TableName,List<string> columnNames, List<string> Values)
        {
            StringBuilder valuesStr = new StringBuilder("\'"+Values[0]+"\'");
            StringBuilder columnsStr = new StringBuilder(columnNames[0]);;
            for (int i = 1; i < columnNames.Count; i++)
            {
                columnsStr.Append(", " + columnNames[i]);
            }
            for (int i = 1; i < Values.Count; i++)
            {
                valuesStr.Append(", \'" + Values[i]+"\'");
            }

            string sqlCommand = $"INSERT INTO {TableName}({columnsStr}) VALUES({valuesStr});";
            _sqlConnection.Open();
            SqlCommand command = new SqlCommand(sqlCommand,_sqlConnection);
            int result = command.ExecuteNonQuery();
            ResultOfSQLInputOperations?.Invoke(this, result);
            _sqlConnection.Close();
        }

        public void InputDataTable(DataTable table, string TableName, int offsetInMapping = 0)
        {
            List<DataRow> rows = new List<DataRow>();
            DataRowCollection rowsCollection = table.Rows;
            foreach(DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            _sqlConnection.Open();
            SqlBulkCopy copier = new SqlBulkCopy(_sqlConnection);
            for(int i =0; i < table.Rows[0].ItemArray.Length; i++)
            {
                copier.ColumnMappings.Add(i,i+offsetInMapping);
            }
            copier.DestinationTableName = TableName;
            copier.WriteToServer(rows.ToArray());
            ResultOfSQLInputOperations?.Invoke(this, copier.RowsCopied);
            _sqlConnection.Close();
        }
        public DataTable GetFromDataTable(List<string> valuesToGet,  string TableName)
        {
            StringBuilder valuesStr = new StringBuilder(valuesToGet[0]);
            for (int i = 1; i < valuesToGet.Count; i++)
            {
                valuesStr.Append(", " + valuesToGet[i]);
            }
            string resultCommand = $"SELECT {valuesStr.ToString()} FROM {TableName}";
            _sqlConnection.Open();
            
            SqlCommand command = new SqlCommand(resultCommand,_sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            _sqlConnection.Close();
            return dt;
        }
        public DataTable GetFromDataTable(string SQLString)
        {
            _sqlConnection.Open();
            SqlCommand command = new SqlCommand(SQLString, _sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            _sqlConnection.Close();
            return dt;
        }
        public List<string> GetListFromDataTable(string SQLString)
        {
            _sqlConnection.Open();
            SqlCommand command = new SqlCommand(SQLString, _sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            _sqlConnection.Close();
            List<string> result =   new List<string>();
            foreach(DataRow row in  dt.Rows)
            {
                result.Add(row[0].ToString());
            }
            return result;
        }

    }
}
