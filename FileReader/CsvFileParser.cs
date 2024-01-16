using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using System.Text.Json.Nodes;
using TestTask.Models;

namespace TestTask.FileReader
{
    public class CsvFileParser : BaseFileParser
    {
      
        public  override IEnumerable<T> Parse<T>(string path)
        {
            
            using var reader = new StreamReader(path);


            /*    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {   
                    HasHeaderRecord = false

                };*/
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.ApplyAttributes(typeof(T));
            using var csv = new CsvReader(reader, config);
           


            Console.WriteLine(csv.Configuration.HasHeaderRecord);

            /*  csv.Context.TypeConverterCache.AddConverter<DateTime>(new CustomDateTimeConverter());
              csv.Context.RegisterClassMap<ValueMap>();*/
             
            var records = csv.GetRecords<T>();
            return records.ToList();
 
        }


       


    }
}
