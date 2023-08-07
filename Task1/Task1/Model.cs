using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class Model
    {

        public event EventHandler<bool>? FileGenerated;
        public event EventHandler<int>? AmountOfFilesGenerated;

        public event EventHandler<int> OnLinesDeleted;
        public event EventHandler<bool> OnFileCreated;

        public event EventHandler<int> AmountOfSQLLinesBeingCopied;
        public event EventHandler<string> AvgMedianSqlResult;

        public string FilesPath { get => _filesPath; set => _filesPath = value; }
        public string NewFilePath { get => _newFilePath; set => _newFilePath = value; }
        public string ValueToDelete { get => _valueToDelete; set => _valueToDelete = value; }


        private string _newFilePath;
        private string _valueToDelete;
        private string? _filesPath;
        private FileGenerator _fileGenerator;
        private FileConverter _converter;
        private SQLOperator _sqlTransactor;

        /// <summary>
        /// Initialize the model. Start sql connection, set event handlers
        /// </summary>
        public Model() {
            _sqlTransactor = new SQLOperator("InputTable");
            _sqlTransactor.AmountOfCopiedInfo += (obj, args) => AmountOfSQLLinesBeingCopied?.Invoke(obj, args);
            _sqlTransactor.AvgMedianSQlResult += (obj, args) => AvgMedianSqlResult?.Invoke(obj, args);

            _fileGenerator = new FileGenerator();
            _fileGenerator.FileGenerated += (obj, inf) => FileGenerated?.Invoke(obj, inf);
            _fileGenerator.AmountOfFilesGenerated += (obj, inf) => AmountOfFilesGenerated?.Invoke(obj, inf);

            _converter = new FileConverter();
            _converter.OnLinesDeleted += (obj, inf) => OnLinesDeleted?.Invoke(obj, inf);
            _converter.OnFileCreated += (obj, inf) => OnFileCreated?.Invoke(obj, inf);

        }

        public string SetConnectionString(string connectionString) {_sqlTransactor.SetConnectionString(connectionString);return "Set"; }
        public void GenerateFiles()=>_fileGenerator.GenerateFiles(_filesPath);
        public void ConvertFilesToOne() => _converter.ConvertFilesToOne(_filesPath, _filesPath + "\\result.txt", _valueToDelete);

        public void CopyToSQLDatabase() => _sqlTransactor.SQLCopeFileToDatabase(_filesPath + "\\result.txt");

        public async void SqlAvgAndMedian() => await _sqlTransactor.GetAvgAndMedianFromDatabase();

    }
}
