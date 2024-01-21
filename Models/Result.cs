using Microsoft.CodeAnalysis;

namespace TestTask.Models
{
    public class Result
    {
        public Result(IOrderedEnumerable<Value> valuesFromFile)
        {

            AllTime = (valuesFromFile.Max(v => v.StartTime) - valuesFromFile.Min(v => v.StartTime)).Duration();
            FirstOperationDate = valuesFromFile.Min(v => v.StartTime);
            AvgCompletionTime = valuesFromFile.Average(v => v.CompletionTime);
            AvgIndexValue = valuesFromFile.Average(v => v.Index);
            MedianIndexValue = valuesFromFile.Count() % 2 == 1 ? valuesFromFile.ElementAt(valuesFromFile.Count() / 2).Index :
                                (valuesFromFile.ElementAt(valuesFromFile.Count() / 2).Index + valuesFromFile.ElementAt(valuesFromFile.Count() / 2 - 1).Index) / 2;
            MaxIndex = valuesFromFile.Max(v => v.Index);
            MinIndex = valuesFromFile.Min(v => v.Index);
            LinesNumber = valuesFromFile.Count();
            FileName = valuesFromFile.First().FileName;
        }

        public Result(TimeSpan allTime, DateTime firstOperationDate, double avgCompletionTime, double avgIndicatorValue, double medianIndicatorValue, double maxIndicator,
            double minIndicator, int linesNumber, string fileName)
        {
            AllTime = allTime;
            FirstOperationDate = firstOperationDate;
            AvgCompletionTime = avgCompletionTime;
            AvgIndexValue = avgIndicatorValue;
            MedianIndexValue = medianIndicatorValue;
            MaxIndex = maxIndicator;
            MinIndex = minIndicator;
            FileName = fileName;
            LinesNumber = linesNumber;
        }
        public Result() { }
        public TimeSpan AllTime { get; set; }
        public DateTime FirstOperationDate { get; set; }
        public double AvgCompletionTime { get; set; }
        public double AvgIndexValue { get; set; }
        public double MedianIndexValue { get; set; }
        public double MaxIndex { get; set; }
        public double MinIndex { get; set; }
        public int LinesNumber { get; set; }
        public string FileName { get; set; }
    }
}
