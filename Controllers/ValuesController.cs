using Microsoft.AspNetCore.Mvc;
using TestTask.Data;
using TestTask.FileReader;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        private readonly ValueContext _context;

        public ValuesController(ValueContext context)
        {
            _context = context;
        }





  /*      private async Task(string fileName) {


            var averageIndicator = _context.Values.Where(v => v.FileName == fileName).Average(v => v.Indicator);
            var averageSecond = _context.Values.Where(v => v.FileName == fileName).Average(v => v.Second);
            var minIndicatorValue = _context.Values.Where(v => v.FileName == fileName).Max(v => v.Indicator);


        }*/


 

 
        [HttpPost("Upload")]
        public async Task Upload()
        {       
           var uploadedFile = Request.Form.Files.First();
           var uploadedFileName = uploadedFile.FileName;

           var csvParser = new CsvFileParser();
           using var fStream = uploadedFile.OpenReadStream();

           var fileRecords = csvParser.Read(fStream).Select(o => ValueDTO.From(o, uploadedFileName)).Where(o => ValueDTO.Validate(o)).ToList();
           var dbRecords =  _context.Values.Where(v => v.FileName == uploadedFileName).ToList();

            if (dbRecords.Count>0) 
            {
              _context.RemoveRange(dbRecords);
              await _context.SaveChangesAsync();
            }
            if (fileRecords.Count>0 && fileRecords.Count <= 10000)
            {
                await _context.Values.AddRangeAsync(fileRecords);
                await _context.SaveChangesAsync();
                await Response.WriteAsync("Succes");
            }
            else 
            {
                await Response.WriteAsync("Unsupported number of lines in file");
            }
        }



        [HttpGet("UploadFileForm")]
        public async Task GetForm()
        {
          Response.ContentType = "text/html; charset=utf-8";
          await  Response.SendFileAsync("Form.html");
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











    }
}
