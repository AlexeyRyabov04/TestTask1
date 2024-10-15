namespace TestTask1.Models
{
    public class FileLineModel
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? LatinString { get; set; }
        public string? CyrillicString { get; set; }
        public int IntegerNumber { get; set; }
        public double DoubleNumber { get; set; }
    }
}
