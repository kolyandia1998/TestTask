using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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


 

 
        [HttpPost("Upload")]
        public async Task Upload()
        {       
           var uploadedFile = Request.Form.Files.First();
           var uploadedFileName = uploadedFile.FileName;

           var csvParser = new CsvFileParser();
           using var fStream = uploadedFile.OpenReadStream();

           var fileRecords = csvParser.Read(fStream).Select(o => ValueDTO.From(o, uploadedFileName)).Where(o => ValueDTO.Validate(o)).ToList();
           var dbRecords =  _context.Values.Where(v => v.FileName == uploadedFileName).ToList();

            if (dbRecords!=null) 
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

            if (dbRecords != null)
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
