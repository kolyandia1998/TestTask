
using Microsoft.VisualBasic.FileIO;


namespace TestTask.FileReader
{
    public class CsvFileParser<T> : BaseFileParser<T>
    {
        public override IEnumerable<T> Parse(Stream fileStream, Func<string[], T?> fromCsv, Func<T, bool> validate)
        {
            List<T> result = new List<T>();
            using var textFieldParser = new TextFieldParser(fileStream);

            textFieldParser.SetDelimiters(";");
            textFieldParser.TextFieldType = FieldType.Delimited;

            while (!textFieldParser.EndOfData)
            {
                var fileds = textFieldParser.ReadFields();
                if (fileds == null)
                    continue;
                var valueFromFile = fromCsv(fileds);
                if (valueFromFile == null)
                    continue;
                if (!validate(valueFromFile))
                    continue;
                result.Add(valueFromFile);
            }
            if (result.Count < 1 || result.Count > 10000)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Unsupported number of lines in file");
            }
            return result;
        }
    }
}







