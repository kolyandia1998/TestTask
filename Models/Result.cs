using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace TestTask.Models
{
    public class Result
    {
        public Result(IEnumerable<Value> valuesFromFile) {

            AllTime = (valuesFromFile.Max(v => v.StartTime) - valuesFromFile.Min(v => v.StartTime)).Duration();
            FirstOperationDate = valuesFromFile.Min(v => v.StartTime);
            AvgCompletionTime = valuesFromFile.Average(v => v.CompletionTime);
            AvgIndicatorValue = valuesFromFile.Average(v => v.Index);
            MedianIndicatorValue = valuesFromFile.Count() % 2 == 1 ? valuesFromFile.ElementAt(valuesFromFile.Count() / 2).Index :
                                  (valuesFromFile.ElementAt(valuesFromFile.Count() / 2).Index + valuesFromFile.ElementAt(valuesFromFile.Count() / 2 - 1).Index) / 2;
            MaxIndicator = valuesFromFile.Max(v => v.Index);
            MinIndicator = valuesFromFile.Min(v => v.Index);
            LinesNumber = valuesFromFile.Count();
            FileName = valuesFromFile.First().FileName;
        }

        public Result(TimeSpan allTime, DateTime firstOperationDate, double avgCompletionTime, double avgIndicatorValue, double medianIndicatorValue, double maxIndicator,
            double minIndicator, int linesNumber, string fileName)
        {
            AllTime = allTime;
            FirstOperationDate = firstOperationDate;
            AvgCompletionTime = avgCompletionTime;
            AvgIndicatorValue = avgIndicatorValue;
            MedianIndicatorValue = medianIndicatorValue;    
            MaxIndicator = maxIndicator;
            MinIndicator = minIndicator;
            FileName = fileName;
            LinesNumber = linesNumber;
        }


         public Result() { }
         public TimeSpan AllTime { get; set; }
         public DateTime FirstOperationDate  { get; set; }
         public double AvgCompletionTime { get; set; }
         public   double AvgIndicatorValue { get; set; }
         public  double MedianIndicatorValue { get; set; }
         public   double MaxIndicator { get; set; }
         public  double MinIndicator { get; set; }
         public  int LinesNumber { get; set; }
         [Key]
         public string FileName { get; set; }

    }
}
