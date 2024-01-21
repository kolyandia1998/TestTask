namespace TestTask.FileReader
{   


    public abstract class BaseFileParser <T>
    {   
        public abstract IEnumerable<T> Parse(Stream fileStream, Func<string[], T?> from, Func<T, bool> validation);
    }
}
