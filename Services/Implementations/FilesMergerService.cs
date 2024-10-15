using System.IO;
using TestTask1.Exceptions;
using TestTask1.Services.Interfaces;

namespace TestTask1.Services.Implementations
{
    public class FilesMergerService : IFilesMergerService
    {
        //объединение файлов в один
        public int MergeFiles(string dirPath, string filePath, string deleteString)
        {
            if (!Directory.Exists(dirPath))
            {
                throw new IncorrectDirException();
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var deletedStrings = 0;
            var dirInfo = new DirectoryInfo(dirPath);
            var allFiles = dirInfo.GetFiles();
            if (allFiles.Length == 0)
            {
                throw new IncorrectDirException();
            }
            foreach (var file in allFiles)
            {
                var lines = File.ReadAllLines(file.FullName);
                if (deleteString.Length == 0)
                {
                    File.AppendAllLines(filePath, lines);
                    continue;
                }
                //Удаление строки с заданным сочетанием символов
                var filtered = lines.Where(line => !line.Contains(deleteString));
                deletedStrings += lines.Length - filtered.Count();
                File.AppendAllLines(filePath, filtered);
            }

            return deletedStrings;
        }
    }
}
