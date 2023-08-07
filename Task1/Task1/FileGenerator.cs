using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class FileGenerator
    {
        public event EventHandler<bool> FileGenerated;
        public event EventHandler<int> AmountOfFilesGenerated;
        private Random _rnd;
        private int _amountOfGeneratedFiles;

        internal FileGenerator()
        {
            _rnd = new Random();
            _amountOfGeneratedFiles = 0;
        }
        /// <summary>
        /// Generate one line for the text file
        /// </summary>
        /// <returns></returns>
        internal string GenerateLine()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string ruchars = "АБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзиклмнопрстуфхцчшщъыьэюя";
            string delimeter = "||";
            long time = _rnd.NextInt64(DateTime.Now.AddYears(-5).Date.Ticks, DateTime.Now.Date.Ticks);
            DateTime date = new DateTime(time);
            string newString = new string(Enumerable.Repeat(chars, 8).Select(s => { return s[_rnd.Next(chars.Length)]; }).ToArray());
            string newRuString = new string(Enumerable.Repeat(ruchars, 8).Select(s => { return s[_rnd.Next(ruchars.Length)]; }).ToArray());
            int randomInt = _rnd.Next(100000001);
            double randomfloat = (_rnd.Next(1, 21) + Math.Round(_rnd.NextDouble(), 8,MidpointRounding.ToEven));
            string resultString = date.ToString("yyyy-MM-dd") + delimeter + newString + delimeter + newRuString + delimeter + randomInt + delimeter + randomfloat;

            return resultString;

        }
        /// <summary>
        /// Generate 100 files in the folder path
        /// </summary>
        /// <param name="folderPath"></param>
        internal void GenerateFiles(string folderPath)
        {
            string path = folderPath + "\\text";
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                string indexPath = path + i.ToString() + ".txt";
                tasks.Add(Task.Run(() => { GenerateFile(indexPath); }).ContinueWith((task) => { _amountOfGeneratedFiles++; AmountOfFilesGenerated?.Invoke(this, _amountOfGeneratedFiles); }));
            }
            Task.WaitAll(tasks.ToArray());
            FileGenerated?.Invoke(this, true);
        }
        /// <summary>
        /// Fill the file with generated lines
        /// </summary>
        /// <param name="pathObject"></param>
        private void GenerateFile(object? pathObject)
        {
            string path = pathObject as String;
            if (path != null)
            {
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int f = 0; f < 25000; f++)
                        {
                            writer.WriteLine(this.GenerateLine());
                        }
                    }
                    writer.Close();
                }
            }

        }
    }
}
