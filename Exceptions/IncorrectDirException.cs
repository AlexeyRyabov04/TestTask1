namespace TestTask1.Exceptions
{
    public class IncorrectDirException : Exception
    {
        public IncorrectDirException()
        {
        }

        public IncorrectDirException(string message) : base(message)
        {
        }
    }
}
