using CsvHelper;
using System.Globalization;

namespace TestTask.Models
{
    public static class ValueDTO
    {
        public static Value From(string[] values,string fileName)
        {
            var dto = new Value();

            DateTime date;
            if (DateTime.TryParseExact(values[0], "yyyy-MM-d_hh-mm-ss", null, DateTimeStyles.None, out date))
                dto.Date = date;
            else 
            {
                throw new ArgumentException();
            
            }
            int seconds;
            if (int.TryParse(values[1], null,out seconds))
                dto.Second = seconds;
            else
            {
                throw new ArgumentException();

            }

            float indicator;
            if (float.TryParse(values[2], null, out indicator))
                dto.Indicator = indicator;
            else
            {
                throw new ArgumentException();
            }
            dto.FileName = fileName;


            return dto;

        }



        public static bool Validate(Value value) { 
         var validator = new ValueValidator();
            return validator.Validate(value).IsValid;

        }


  


    }
}
