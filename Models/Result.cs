using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace TestTask.Models
{
    public record Result
    {
     public    TimeSpan AllTime { get; set; }
      public  DateTime FirstOperationDate { get; set; }
      public  float AvgCompletionTime { get; set; }
      public   float AvgIndicatorValue { get; set; }

      public  float MedianIndicatorValue { get; set; }

     public   float MaxIndicator {get; set; }

      public  float MinIndicator { get; set; }

      public  int LinesNumber { get; set; }
      
      [Key]   
      public  string? FileName { get; set; }
    }
}
