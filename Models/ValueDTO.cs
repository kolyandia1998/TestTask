using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace TestTask.Models
{
    public record ValueDTO
    {
        public static Value From(string[] values)
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
                dto.Second = seconds;
            else
            {
                throw new ArgumentException();
            }


            if ( true )

            return dto;
        }


  


    }
}
