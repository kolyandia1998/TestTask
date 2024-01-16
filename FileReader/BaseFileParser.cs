namespace TestTask.FileReader
{   


    public abstract class BaseFileParser 
    {   
        public abstract IEnumerable<T> Parse<T>(string path);

    }
}
