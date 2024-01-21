
using Humanizer;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace TestTask.Models
{
    
    public class Value 
    {
        [Key]
        public DateTime StartTime { get; set; }
        public int CompletionTime { get; set; }
        public string? FileName { get; set; }
        public double Index { get; set; }

        public static Value? FromCsv(string[] fields) {

            if (fields.Count() < 3)
            return null;

            var value = new Value();

            DateTime startTime;
            if (!DateTime.TryParseExact(fields[0], "yyyy-MM-d_hh-mm-ss", null, DateTimeStyles.None, out startTime))
            return null;
            value.StartTime = startTime;

            int completionTime;
            if (!int.TryParse(fields[1], null, out completionTime))
            return null;
            value.CompletionTime = completionTime;
           
            double index;   
            if (!double.TryParse(fields[2], null, out index))
            return null;
            value.Index = index;    

            return value;
        }
        public static bool Validate(Value value) { 
  
            var validator = new ValueValidator();
            return validator.Validate(value).IsValid;
        }
    }
}
