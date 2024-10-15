using System.IO;
using TestTask1.Services.Interfaces;

namespace TestTask1.Services.Implementations
{
    public class FilesGeneratorService : IFilesGeneratorService
    {
        private const int MIN_INTEGER_VALUE = 1;
        private const int MAX_INTEGER_VALUE = 100000000;
        private const int YEARS_COUNT = 5;
        private const int LATIN_SYMBOlS_COUNT = 10;
        private const int CYRILLIC_SYMBOlS_COUNT = 10;
        private const int MIN_DOUBLE_VALUE = 1;
        private const int MAX_DOUBLE_VALUE = 20;
        private const int DOUBLE_DIGITS_COUNT = 8;

        private const string LATIN_SYMBOLS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string CYRILLIC_SYMBOLS = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private const int FILES_COUNT = 100;
        private const int LINES_COUNT = 100000;

        private Random _random;
        public FilesGeneratorService()
        {
            _random = new Random();
        }

        //Получение итоговой строки
        private List<string> GenerateLines()
        {
            var lines = new List<string>();

            for (int i = 0; i < LINES_COUNT; i++)
            {
                lines.Add(string.Join("||", GenerateDate(), GenerateLatinString(), GenerateCyrillicString(), GenerateEvenInteger(), GenerateDouble()) + "||");
            }

            return lines;
        }

        private DateOnly GenerateDate()
        {
            var startDate = DateTime.Now.AddYears(-YEARS_COUNT);
            var date = startDate.AddDays(_random.Next((int)(DateTime.Now - startDate).TotalDays));
            return new DateOnly(date.Year, date.Month, date.Day);
        }

        private int GenerateEvenInteger()
        {
            return 2 * _random.Next(MIN_INTEGER_VALUE / 2, MAX_INTEGER_VALUE / 2);
        }

        private string GenerateLatinString()
        {
            var latinString = new char[LATIN_SYMBOlS_COUNT];
            for (int i = 0; i < LATIN_SYMBOlS_COUNT; i++)
            {
                latinString[i] = LATIN_SYMBOLS[_random.Next(LATIN_SYMBOLS.Length)];
            }
            return new string(latinString);
        }

        private string GenerateCyrillicString()
        {
            var cyrillicString = new char[CYRILLIC_SYMBOlS_COUNT];
            for (int i = 0; i < CYRILLIC_SYMBOlS_COUNT; i++)
            {
                cyrillicString[i] = CYRILLIC_SYMBOLS[_random.Next(CYRILLIC_SYMBOLS.Length)];
            }
            return new string(cyrillicString);
        }

        private double GenerateDouble()
        {
            return Math.Round(_random.NextDouble() * (MAX_DOUBLE_VALUE - 1) + MIN_DOUBLE_VALUE, DOUBLE_DIGITS_COUNT);
        }

        private async Task GenerateFile(string filePath)
        {
            var lines = new List<string>();
            lines = GenerateLines();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < LINES_COUNT; i++)
                {
                    await writer.WriteLineAsync(lines[i]);
                }
            }
        }

        public async Task GenerateFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var tasks = new List<Task>();

            for (int i = 0; i < FILES_COUNT; i++)
            {
                string filePath = Path.Combine(directoryPath, $"file{i + 1}.txt");
                tasks.Add(GenerateFile(filePath));
            }

            await Task.WhenAll(tasks);

        }
    }
}
