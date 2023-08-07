using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableObjectsClassLibrary;
using ExcelDataReader;
using System.Windows.Controls;
using System.Data;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows;
using System.Data.SqlTypes;

namespace Task2WPF
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private string _filePath;
        private Model _model;
        private string _tableHeader;
        private string _rowsSkip;
        private string _result;
        private string _sqlString;
        private DataView _dataTable;
        private bool connectedToDataBase;
        private bool loadedFile;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string FilePath { get => _model.FilePath; set { _model.FilePath = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilePath")); } }
        public string TableHeader { get => _tableHeader; set { _tableHeader = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TableHeader")); } }
        public string Result { get => _result; set { _result = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result")); } }
        public string SQLConnectionString { get => _sqlString; set { _sqlString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SQLConnectionString")); } }
        public DataView DataTable { get => _dataTable; set { _dataTable = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataTable")); } }
        public bool ConnectedToDataBase { get => connectedToDataBase; set { connectedToDataBase = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ConnectedToDataBase")); } }
        public bool LoadedFile { get => loadedFile; set { loadedFile = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LoadedFile")); } }

        public ModelViewCommand TestCommand { get; set; }
        public ModelViewCommand SelectFile { get; set; }
        public ModelViewCommand GetAmountOfFiles { get; set; }
        public ModelViewCommand LoadInfoFromFile { get; set; }
        public ModelViewCommand ShowDataFromDBByFile { get; set; }
        public ModelViewCommand ConnectToDataBase { get; set; }

        public ViewModel() {
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _model = new Model();
            TableHeader = "6";
            SQLConnectionString = "Data Source=DESKTOP-2LF5IPN;Initial Catalog=Task2DB;Integrated Security=True;TrustServerCertificate=True";
            ConnectedToDataBase = false;
            LoadedFile = false;
            TestCommand = new ModelViewCommand((obj) =>
            {
                _model.SendInfoToDB( int.Parse(TableHeader), new List<string> {"Invoice","IncomeSaldo","Turnover","OutcomeSaldo" });
            });
            SelectFile = new ModelViewCommand((obj) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    FilePath = openFileDialog.FileName;
            }); 
            LoadInfoFromFile = new ModelViewCommand((obj) =>
            {

                Result = "Loading file....";
                string result= _model.GetInfoFromFile();
                if (result != null)
                {
                    Result = result;
                }
                else
                {
                    LoadedFile = true;

                    Result = "File loaded succesfully";
                }
            });
            GetAmountOfFiles = new ModelViewCommand((obj) =>
            {

                Result = "Calling to Database....";
                Result = _model.GetAmountOfLoadedFiles();
            });
            ShowDataFromDBByFile = new ModelViewCommand((obj) =>
            {

                if (obj.ToString().Length > 0)
                {
                    Result = "Calling to Database....";
                    this.DataTable = _model.GetInfoFromDatabaseForFile(obj.ToString());
                    Result = "DataTable is loaded";
                }
                else
                {
                    Result = "No File selected";
                }
            });
            ConnectToDataBase = new ModelViewCommand((obj) =>
            {
                Result = "Connecting to Database....";
                string result = _model.ConnectToDatabase(obj.ToString());
                if(result != null)
                {
                    Result = result;
                }
                else
                {
                    ConnectedToDataBase = true;

                    Result = "Successful connect";
                }
            });
        }
    }
}
