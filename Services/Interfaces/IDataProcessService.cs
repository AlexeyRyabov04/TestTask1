namespace TestTask1.Services.Interfaces
{
    public interface IDataProcessService
    {
        public event Action<int, int> CountLines;
        public Task SaveFileToDatabase(string filePath);
        public (long, double) CalculateSumAndMedian();
    }
}
