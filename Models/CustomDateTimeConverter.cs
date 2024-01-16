using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace TestTask.Models
{
    public class CustomDateTimeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {   
             
            DateTime date;
            if ( text != null && DateTime.TryParseExact(text, "yyyy-MM-d_hh-mm-ss", null, DateTimeStyles.None, out date) )
            return date;
            return null;
        }

        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if ( value is null )
                return string.Empty;

            if ( value is DateTime  date)
                return date.ToString("yyyy-MM-d_hh-mm-ss");

            return string.Empty;
        }
    }


    

 







}
