using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TestTask.Data;
using TestTask.FileReader;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("[controller]")]
    public class MainController : Controller
    {
        private readonly TestTaskDBContext _context;
        public MainController(TestTaskDBContext db_context)
        {
            _context = db_context;
        }



        private Result CreateResult(IEnumerable<Value> valuesFromFile) {

            var minDate = valuesFromFile.Min(v => v.StartTime);
            var allTime = (valuesFromFile.Max(v => v.StartTime) - minDate).Duration();
            var avgCompletionTime = valuesFromFile.Average(v => v.CompletionTime);
            var avgIndicatorValue = valuesFromFile.Average(v => v.Index);
            var numberLineCount = valuesFromFile.Count();
            var sortedFileRecords = valuesFromFile.OrderBy(v => v.Index);
            var medianIndicator = numberLineCount % 2 == 1 ? valuesFromFile.ElementAt(numberLineCount / 2).Index :
            (sortedFileRecords.ElementAt(numberLineCount / 2).Index + sortedFileRecords.ElementAt(numberLineCount / 2 - 1).Index) / 2;
            var maxIndicator = sortedFileRecords.Max(v => v.Index);
            var minIndicator = sortedFileRecords.Min(v => v.Index);
            var result = new Result();
            result.AllTime = allTime;
            result.AvgIndicatorValue = avgIndicatorValue;
            result.MedianIndicatorValue = medianIndicator;
            result.AvgCompletionTime = (double)avgCompletionTime;
            result.LinesNumber = numberLineCount;
            result.MaxIndicator = maxIndicator;
            result.MinIndicator = minIndicator;
            result.FirstOperationDate = minDate;
            result.FileName = valuesFromFile.First().FileName;
            return result;  
        }



        private async Task addOrUpdate (IEnumerable<Value> valuesFromFile, Result result) {

            var dbValueRecord = _context.Values.Where(v => v.FileName == valuesFromFile.First().FileName).ToList();



            var dbResultRecord = _context.Results.Where(v => v.FileName == valuesFromFile.First().FileName).FirstOrDefault();


            if (dbValueRecord.Count > 0)
            {
              _context.Values.RemoveRange(dbValueRecord);
              await  _context.Values.AddRangeAsync(valuesFromFile);
              _context.Results.Update(result);
              await _context.SaveChangesAsync();
            }
            else
            {
             await   _context.Values.AddRangeAsync(valuesFromFile);
             await   _context.Results.AddAsync(result);
             await _context.SaveChangesAsync();
            }


        }


        [HttpPost("Upload")]
        public async Task<IResult> Upload()
        {   
           

            var uploadedFile = Request.Form.Files.First();
            
            var uploadedFileName = uploadedFile.FileName;

            if (!uploadedFileName.EndsWith("csv"))
               
            return  Results.Text("Unsupported file format");

            var parser = new CsvFileParser<Value>();
            using var fileStream = uploadedFile.OpenReadStream();
            try
            {
                IEnumerable<Value> valuesFromFile = parser.Parse(fileStream, Value.FromCsv, Value.Validate).Select(v => { v.FileName = uploadedFileName; return v; });
                var resull = CreateResult(valuesFromFile);
                await addOrUpdate(valuesFromFile, resull);
                return Results.Text("Successfully");
            }
            catch (ValidationException ex) 
            {
                return Results.Text(ex.Message);
            }
        }



        [HttpGet("GetFormForFileUpload")]
        public async Task GetForm()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("Form.html");
        }




        [HttpGet("GetRecords")]
        public async Task GetValueRecords(string fileName)
        {
            var dbRecords = _context.Values.Where(v => v.FileName == fileName).ToList();

            if (dbRecords.Count > 0)
            {
                await Response.WriteAsJsonAsync(dbRecords);
            }
            else
            {
                await Response.WriteAsJsonAsync($"No records with fileName={fileName}");
            }
        }

        [HttpGet("GetResults")]
        public async Task GetResultsRecords(string? fileName, DateTime? begin, DateTime? end, float? avgInicatorBegin, float? avgInicatorEnd, int? avgTimeBegin, int? avgTimeEnd)
        {
            var results = _context.Results.AsQueryable();

            if (!string.IsNullOrEmpty(fileName)) {
                results = results.Where(r => r.FileName == fileName);
            }
            if (begin != null && end != null) {
                results = results.Where(r => r.FirstOperationDate >= begin && r.FirstOperationDate <= end);
            }
            if (avgInicatorBegin != 0 && avgInicatorBegin != null && avgInicatorEnd != 0 && avgInicatorEnd != null)
            {
                results = results.Where(r => r.AvgIndicatorValue >= avgInicatorBegin && r.AvgIndicatorValue <= avgInicatorEnd);
            }
            if (avgTimeBegin != 0 && avgTimeBegin != null && avgTimeEnd != 0 && avgTimeEnd != null) {

                results = results.Where(r => r.AvgCompletionTime >= avgTimeBegin && r.AvgCompletionTime <= avgTimeEnd);
            }
            await  Response.WriteAsJsonAsync ( results.ToList());




        }
    }
}
