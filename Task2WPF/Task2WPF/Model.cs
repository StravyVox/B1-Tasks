using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using TableObjectsClassLibrary;
namespace Task2WPF
{
    internal class Model
    {
        public string FilePath { get => _filePath; set { ChangeFilePath(value); } }
        public DataTable Dataset { get => _dataset; set => _dataset = value; }

        private string _filePath;
        private DataTable _dataset;
        private SQLOperator _operator;



        public Model() {
              
        }
        internal string ConnectToDatabase (string SQLConnectionString)
        {
            try
            {
                _operator = new SQLOperator(SQLConnectionString);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private void ChangeFilePath(string filePath)
        {
            if(filePath !=null && filePath.ToString().Count()>0)
            {
                _filePath = filePath;
            }
        }

        /// <summary>
        /// Insert whole Table to DataBase
        /// </summary>
        /// <param name="RowsToIgnore"></param>
        /// <param name="HeaderRowAddress"></param>
        /// <param name="TableSQLNames"></param>
        public void SendInfoToDB( int HeaderRowAddress, List<string> TableSQLNames)
        {
            if(_dataset != null)
            {
                string FileId = InsertFileToDB();
                List<TableAddress> tableHeaders = DataTableOperations.GetTableAdressesFromDataSet(_dataset, HeaderRowAddress, 8); //Headers only
                List<ClassAddress> classesList = DataTableOperations.GetClasses(_dataset, 0, "КЛАСС "); //ClassInfo only
                List<string> classesIds = InsertClassesToDB(classesList, FileId);

                List<ClassTable> classTable = new List<ClassTable>();
                for(int classId = 0; classId<classesList.Count; classId++) { //Get ClassTable structs
                    List<Table> tables = DataTableOperations.GetTablesForClass(_dataset, tableHeaders, classesList[classId]); 
                    List<DataTable> convertedTables = DataTableConverter.ConvertTablesToDataTables(tables);
                    classTable.Add(new ClassTable() { ClassName = classesList[classId].NameOfClass, FileId = FileId,ClassId = classesIds[classId], TablesForClass = convertedTables });
                }

                for(int i = 0; i<classTable.Count; i++)//Create external keys for tables and insert them into DB;
                {
                    DataTableConverter.AddExternalKey(classTable[i].TablesForClass[0], classTable[i].ClassId, "IdOfClass");
                    _operator.InputDataTable(classTable[i].TablesForClass[0], TableSQLNames[0], 1);
                    List<string> id = _operator.GetListFromDataTable($"SELECT ID FROM INVOICE WHERE IdOfClass = {classTable[i].ClassId}");
                    int indexName = 1;
                    foreach(DataTable table in classTable[i].TablesForClass.Skip(1))
                    {
                        DataTableConverter.AddExternalKey(table, id, "NumOfInvoice");
                        _operator.InputDataTable(table, TableSQLNames[indexName]);
                        indexName++;
                    }
                }
            }

        }
        /// <summary>
        /// Insert File Info to DataBase
        /// </summary>
        /// <returns></returns>
        private string InsertFileToDB()
        {
            _operator.InputSingleRecord("Files", new List<string> {"DateOfAdd", "FileName" },new List<string> { DateTime.Now.ToString("yyyy-MM-dd"), FilePath });
            DataTable resultDB = _operator.GetFromDataTable($"SELECT Id FROM Files WHERE FileName=\'{FilePath}\'");
            return resultDB.Rows[0].ItemArray[0].ToString();
            
        }
        /// <summary>
        /// Insert class address info to DataBase
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="IdOfFile"></param>
        /// <returns></returns>
        private List<string> InsertClassesToDB(List<ClassAddress> classes, string IdOfFile)
        {
            List<string> resultIds = new List<string>();
            DataTable classesTable = new DataTable();
            classesTable.Columns.Add("IdOfFile", typeof(string));
            classesTable.Columns.Add("NameOfClass", typeof(string));
            foreach(ClassAddress classAddress in classes)
            {
                DataRow row = classesTable.NewRow();
                row["IdOfFile"] = IdOfFile;
                row["NameOfClass"] = classAddress.NameOfClass;
                classesTable.Rows.Add(row);
            }
            _operator.InputDataTable(classesTable, "ClassTable", 1);
            DataTable resultTable = _operator.GetFromDataTable($"SELECT Id FROM ClassTable WHERE IdOfFile = {IdOfFile}");
            foreach(DataRow row in resultTable.Rows)
            {
                resultIds.Add(row.ItemArray[0].ToString());
            }
            return resultIds;
        }
        /// <summary>
        /// Executes SQL Query and return amount of inserted Files into DB
        /// </summary>
        /// <returns></returns>
        internal string GetAmountOfLoadedFiles()
        {
            return _operator.GetListFromDataTable("SELECT COUNT(ID) FROM FILES")[0];
        }

        /// <summary>
        /// Executes SQL Query and return results for the loaded file
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        internal DataView GetInfoFromDatabaseForFile(string FileName)
        {
            string SqlString = $"select inv.NumOfInvoice, inc.Active, inc.Passive,turn.Debit,turn.Credit, outs.Active, outs.Passive " +
                $"from Files as fl" +
                $"\r\nINNER JOIN ClassTable as class On class.IdOfFile = fl.Id" +
                $"\r\nINNER JOIN Invoice as inv On class.Id = inv.IdOfClass" +
                $"\r\nINNER JOIN IncomeSaldo as inc On inc.NumOfInvoice=inv.Id" +
                $"\r\nINNER JOIN OutcomeSaldo as outs On outs.NumOfInvoice=inv.Id" +
                $"\r\nINNER JOIN Turnover as turn On turn.NumOfInvoice=inv.Id \r\n" +
                $"where fl.FileName = \'{FileName}\'\r\n;";
            DataTable result = _operator.GetFromDataTable(SqlString);
            return result.AsDataView();
        }

        /// <summary>
        /// Load file from drive and set it as dataset; If any errors, returned string will not be null
        /// </summary>
        /// <returns></returns>
        internal string GetInfoFromFile()
        {
            try
            {
                using (var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        DataSet dataset = reader.AsDataSet();
                        DataTable table = dataset.Tables[0];
                        _dataset = table;
                        return null;
                    }
                }
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }

        struct ClassTable
        {
            public string FileId;
            public string ClassName;
            public string ClassId;
            public List<DataTable> TablesForClass; 
        }
    }
}
