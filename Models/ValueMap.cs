using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TestTask.Models
{
    public class ValueMap : ClassMap<Value>
    {

        public ValueMap()
        {
            Map(m => m.Date);
            Map(m => m.Second).Validate(int args => int.Parse(args) );
            Map(m => m.Indicator);
         

        }
    }
}
