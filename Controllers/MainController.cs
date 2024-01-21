using Microsoft.AspNetCore.Mvc;
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

        private async Task addOrUpdate(IEnumerable<Value> valuesFromFile, Result result)
        {
            var dbValueRecord = _context.Values.Where(v => v.FileName == valuesFromFile.First().FileName).ToList();
            if (dbValueRecord.Count > 0)
            {
                _context.Values.RemoveRange(dbValueRecord);
                await _context.Values.AddRangeAsync(valuesFromFile);
                var dbResultRecord = _context.Results.Where(v => v.FileName == valuesFromFile.First().FileName).FirstOrDefault();
                dbResultRecord.MedianIndexValue = result.MedianIndexValue;
                dbResultRecord.AvgIndexValue = result.AvgIndexValue;
                dbResultRecord.LinesNumber = result.LinesNumber;
                dbResultRecord.AllTime = result.AllTime;
                dbResultRecord.AvgCompletionTime = result.AvgCompletionTime;
                dbResultRecord.FirstOperationDate = result.FirstOperationDate;
                dbResultRecord.MinIndex = result.MinIndex;
                dbResultRecord.MaxIndex = result.MaxIndex;
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.Values.AddRangeAsync(valuesFromFile);
                await _context.Results.AddAsync(result);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost("Upload")]
        public async Task<IResult> Upload()
        {
            var uploadedFile = Request.Form.Files.Count() == 0 ? null : Request.Form.Files.First();
            if (uploadedFile == null)
                return Results.Redirect("GetFormForFileUpload");
            var uploadedFileName = uploadedFile.FileName;
            if (!uploadedFileName.EndsWith("csv"))
                return Results.BadRequest("Unsupported file format");
            var parser = new CsvFileParser<Value>();
            using var fileStream = uploadedFile.OpenReadStream();
            try
            {
                IEnumerable<Value> valuesFromFile = parser.Parse(fileStream, Value.FromCsv, Value.Validate).Select(v => { v.FileName = uploadedFileName; return v; });
                var result = new Result(valuesFromFile.OrderBy(v => v.Index));
                await addOrUpdate(valuesFromFile, result);
                return Results.Text("Successfully");
            }
            catch (ValidationException ex)
            {
                return Results.Text(ex.Message);
            }
        }

        [HttpGet("GetFormForFileUpload")]
        public IResult GetForm()
        {
            return Results.File("Form.html", "text/html; charset=utf-8");
        }

        [HttpGet("GetValue")]
        public IResult GetValueRecords(string fileName)
        {
            var dbRecords = _context.Values.Where(v => v.FileName == fileName).ToList();
            if (dbRecords.Count > 0)
            {
                return Results.Json(dbRecords);
            }
            else
            {
                return Results.Text($"No records with fileName={fileName}");
            }
        }

        [HttpGet("GetResults")]
        public IResult GetResultsRecords(string? fileName, DateTime? begin, DateTime? end, double? avgInicatorBegin, double? avgInicatorEnd, int? avgTimeBegin, int? avgTimeEnd)
        {
            var results = _context.Results.AsQueryable();
            if (!string.IsNullOrEmpty(fileName))
            {
                results = results.Where(r => r.FileName == fileName);
            }
            if (begin != null && end != null)
            {
                results = results.Where(r => r.FirstOperationDate >= begin && r.FirstOperationDate <= end);
            }
            if (avgInicatorBegin != 0 && avgInicatorBegin != null && avgInicatorEnd != 0 && avgInicatorEnd != null)
            {
                results = results.Where(r => r.AvgIndexValue >= avgInicatorBegin && r.AvgIndexValue <= avgInicatorEnd);
            }
            if (avgTimeBegin != 0 && avgTimeBegin != null && avgTimeEnd != 0 && avgTimeEnd != null)
            {
                results = results.Where(r => r.AvgCompletionTime >= avgTimeBegin && r.AvgCompletionTime <= avgTimeEnd);
            }
            return Results.Json(results);
        }
    }
}
