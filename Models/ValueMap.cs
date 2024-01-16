using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TestTask.Models
{
    public class ValueMap : ClassMap<Value>
    {

        public ValueMap()
        {
            Map(m => m.Date).TypeConverter<CustomDateTimeConverter>();
            Map(m => m.Second);
            Map(m => m.Indicator);
             
        }
    }
}
