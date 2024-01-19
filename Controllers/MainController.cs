using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        [HttpPost("Upload")]
        public async Task Upload()
        {
            var uploadedFile = Request.Form.Files.First();
            var uploadedFileName = uploadedFile.FileName;

            var csvParser = new CsvFileParser();
            using var fStream = uploadedFile.OpenReadStream();

            var fileRecords = csvParser.Read(fStream).Select(o => ValueDTO.From(o, uploadedFileName)).Where(o => ValueDTO.Validate(o)).ToList();
            var dbRecords = _context.Values.Where(v => v.FileName == uploadedFileName).ToList();

            if (dbRecords.Count > 0)
            {
                _context.RemoveRange(dbRecords);
                await _context.SaveChangesAsync();
            }
            if (fileRecords.Count > 0 && fileRecords.Count <= 10000)
            {
                await _context.Values.AddRangeAsync(fileRecords);
                await _context.SaveChangesAsync();
                var minDate = fileRecords.Min(v => v.Date);

                var allTime = (fileRecords.Max(v => v.Date) - minDate).Duration();
                var avgCompletionTime = fileRecords.Average(v => v.Second);
                var avgIndicatorValue = fileRecords.Average(v => v.Indicator);
                var numberLineCount = fileRecords.Count();

                var sortedFileRecords = fileRecords.OrderBy(v => v.Indicator);
                var medianIndicator = numberLineCount % 2 == 1 ? fileRecords.ElementAt(numberLineCount / 2).Indicator :
                    (sortedFileRecords.ElementAt(numberLineCount / 2).Indicator + sortedFileRecords.ElementAt(numberLineCount / 2 - 1).Indicator) / 2;
                var maxIndicator = sortedFileRecords.Max(v => v.Indicator);
                var minIndicator = sortedFileRecords.Min(v => v.Indicator);

                var resultDto = new Result();
                resultDto.AllTime = allTime;
                resultDto.AvgIndicatorValue = avgIndicatorValue;
                resultDto.MedianIndicatorValue = medianIndicator;
                resultDto.AvgCompletionTime = (float)avgCompletionTime;
                resultDto.LinesNumber = numberLineCount;
                resultDto.MaxIndicator = maxIndicator;
                resultDto.MinIndicator = minIndicator;
                resultDto.FirstOperationDate = minDate;
                resultDto.FileName = uploadedFileName;

                if (_context.Results.Where(r => r.FileName == uploadedFileName).Count() > 0)
                {
                    _context.Results.Update(resultDto);
                }
                else 
                {
                   await _context.Results.AddAsync(resultDto);
                }
                
                await _context.SaveChangesAsync();



                await Response.WriteAsync("Succes");
            }
            else
            {
                await Response.WriteAsync("Unsupported number of lines in file");
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
