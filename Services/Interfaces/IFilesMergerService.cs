namespace TestTask1.Services.Interfaces
{
    public interface IFilesMergerService
    {
        public int MergeFiles(string dirPath, string filePath, string deleteString);
    }
}
