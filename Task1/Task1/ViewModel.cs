using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// View model for setting command handlers
    /// </summary>
    internal class ViewModel:INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler? PropertyChanged;
        public ModelViewCommand GenerateCommand { get; set; }
        public ModelViewCommand ConvertCommand { get; set; }
        public ModelViewCommand SQLCopyCommand { get; set; }
        public ModelViewCommand SQLAvgAndMedianCommand { get; set; }
        public ModelViewCommand SetSQLCommand { get; set; }
        public bool SqlCommandSet { get => sqlCommandSet; set { sqlCommandSet = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SqlCommandSet")); } }
        public string SqlString { get => sqlString; set { sqlString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SqlString")); } }
        public string ResultString {
            get => _resultstring;
            set { _resultstring = value;
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResultString")); 
                } 
            }

        public string PathString { 
            get => _model.FilesPath; 
            set
            {
                _model.FilesPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PathString"));
            }
        }
        public string ValueToFind
        {
            get => _model.ValueToDelete;
            set
            {
                _model.ValueToDelete = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ValueToFind"));
            }
        }

        private Model _model;
        private string _resultstring;
        private string sqlString;
        private bool sqlCommandSet;

        public ViewModel()
        {
            _model = new Model();
            SqlString = @"Data Source=DESKTOP-2LF5IPN;Initial Catalog=Task1DB;Integrated Security=True;TrustServerCertificate=True";
            SqlCommandSet = false;

            _model.FileGenerated += (sender, result) =>
            {
                ResultString = "Result of creating: " + result;
            };
            _model.AmountOfFilesGenerated += (sender, result) =>
            {
                ResultString = "Files generated: " + result;
            };
            _model.OnLinesDeleted += (sender, result) =>
            {
                ResultString = "Lines being deleted is: " + result;
            };
            _model.AmountOfSQLLinesBeingCopied += (sender, result) =>
            {
                ResultString = "Lines being copied to SQL: " + result;
            };
            _model.AvgMedianSqlResult += (sender, result) =>
            {
                ResultString = result;
            };
            SetSQLCommand = new ModelViewCommand(() =>
            {
                ResultString = _model.SetConnectionString(SqlString);
                SqlCommandSet = true;
            });
            GenerateCommand = new ModelViewCommand(() =>
            {
                Task.Run(()=>_model.GenerateFiles());
                ResultString = "Generating";
            });
            ConvertCommand = new ModelViewCommand(() =>
            {
                Task.Run(() => _model.ConvertFilesToOne());
                ResultString = "Converting";
            });
            SQLCopyCommand = new ModelViewCommand(() =>
            {
                Task.Run(() => _model.CopyToSQLDatabase()).ContinueWith(task => { _model = null; _model = new Model(); _model.SetConnectionString(SqlString); });
                ResultString = "Copying to SQL";
            });
            SQLAvgAndMedianCommand = new ModelViewCommand(() =>
            {
                 ResultString = "SQL Command was send. Waiting...";
                Task.Run(() => {
                    ResultString = "SQL Command was send. Waiting...";
                    _model.SqlAvgAndMedian();
                    });
            });
        }


    }
}
