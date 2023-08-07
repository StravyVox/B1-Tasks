using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Provides functions to work with files
    /// </summary>
    internal class FileConverter
    {
        public event EventHandler<int> OnLinesDeleted;
        public event EventHandler<bool> OnFileCreated;

        /// <summary>
        /// Get list of filenames 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        internal List<string> GetFilesFromFolder(string folder)
        {
           return new List<string>( Directory.GetFiles(folder,"text*.txt"));
        }

        /// <summary>
        /// Clear line from all finded files and return result of convertion
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="valueToFind"></param>
        /// <returns></returns>
        internal List<string> ConvertFiles(string folder, string valueToFind)
        {
            List<string> filesInFolder = GetFilesFromFolder(folder);
            List<Task> tasksToRemoveStrings = new List<Task>();
            List<int> amountOfDeletedLines = new List<int>();
            foreach (string file in filesInFolder)
            {
                tasksToRemoveStrings.Add(
                    Task.Run(() => ClearFileFromLines(file, valueToFind)).ContinueWith(task => amountOfDeletedLines.Add(task.Result)));
            }
            Task.WaitAll(tasksToRemoveStrings.ToArray());
            int amount = amountOfDeletedLines.Aggregate((x, y) => x + y);
            OnLinesDeleted?.Invoke(this, amount);
            return filesInFolder;
        }
        /// <summary>
        /// Remove lines from file and save file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="valueToFind"></param>
        /// <returns></returns>
        internal int ClearFileFromLines(string filePath, string valueToFind)
        {
            var tempFile = Path.GetTempFileName();
            var linesToDelete = File.ReadAllLines(filePath).Where(line=>line.Contains(valueToFind));
            var linesToKeep = File.ReadAllLines(filePath).Where(line => (line.Contains(valueToFind)==false));

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(filePath);
            File.Move(tempFile, filePath);
           
            return linesToDelete.Count();
        }

        /// <summary>
        /// Copy a lot of files to one
        /// </summary>
        /// <param name="listOfFiles"></param>
        /// <param name="pathToFile"></param>
        internal void CopyFilesToFile(List<string> listOfFiles, string pathToFile)
        {
            StreamWriter streamWriter = new StreamWriter(pathToFile, false);
            streamWriter.Close();
            listOfFiles.ForEach(file =>File.AppendAllLines(pathToFile,File.ReadAllLines(file)));
            OnFileCreated?.Invoke(this, true);
        }
        /// <summary>
        /// Remove lines from files and copy files to one file
        /// </summary>
        /// <param name="pathOfFolder"></param>
        /// <param name="pathForFile"></param>
        /// <param name="valueToFind"></param>
        internal void ConvertFilesToOne(string pathOfFolder, string pathForFile, string valueToFind)
        {
            List<string> files = ConvertFiles(pathOfFolder, valueToFind);
            CopyFilesToFile(files, pathForFile);
        }
        
    }
}
