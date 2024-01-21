using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace TestTask.Models
{
    public class Result
    {
      public    TimeSpan AllTime { get; set; }
      public  DateTime FirstOperationDate { get; set; }
      public  double AvgCompletionTime { get; set; }
      public   double AvgIndicatorValue { get; set; }

      public  double MedianIndicatorValue { get; set; }

     public   double MaxIndicator {get; set; }

      public  double MinIndicator { get; set; }

      public  int LinesNumber { get; set; }
      
      [Key]   
      public  string? FileName { get; set; }
    }
}
