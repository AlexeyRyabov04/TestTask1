using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using TestTask1.Data;
using TestTask1.Exceptions;
using TestTask1.Models;
using TestTask1.Services.Interfaces;

namespace TestTask1.Services.Implementations
{
    public class DataProcessService : IDataProcessService
    {
        public event Action<int, int> CountLines;

        //Парсинг строки
        private FileLineModel TryParseString(string line)
        {
            var elements = line.Split("||");
            if (elements.Length != 6)
            {
                throw new IncorrectFileException();
            }
            var fileLine = new FileLineModel();
            fileLine.Date = DateOnly.ParseExact(elements[0], "dd.MM.yyyy");
            fileLine.LatinString = elements[1];
            fileLine.CyrillicString = elements[2];
            fileLine.IntegerNumber = int.Parse(elements[3]);
            fileLine.DoubleNumber = double.Parse(elements[4], CultureInfo.InvariantCulture);
            return fileLine;
        }

        public async Task SaveFileToDatabase(string filePath)
        {
            using (var dbContext = new AppDbContext())
            {
                if (!File.Exists(filePath))
                {
                    throw new IncorrectFileException();
                }

                var lines = File.ReadAllLines(filePath);
                var linesCount = lines.Length;
                CountLines?.Invoke(0, linesCount);
                var importedLineCount = 0;
                foreach (var line in lines)
                {
                    try
                    {
                        var fileLines = TryParseString(line);
                        importedLineCount++;
                        dbContext.FileLine.Add(fileLines);
                        //Вывод хода процесса
                        CountLines?.Invoke(importedLineCount, linesCount - importedLineCount);
                    }
                    catch (Exception)
                    {
                        throw new IncorrectFileException();
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }

        private record QueryResult(long resSum, double median);
        //вызов хранимой процедуры
        /*
            SELECT 
                SUM("IntegerNumber") as resSum,
                PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY "DoubleNumber") AS median
            FROM "FileLine";
        */
        public (long, double) CalculateSumAndMedian()
        {
            using (var dbContext = new AppDbContext())
            {

                var result = dbContext.Database.SqlQuery<QueryResult>($"select * from calculate_sum_and_median()").ToList();
                return (result[0].resSum, result[0].median);
            }

        }
    }
}
