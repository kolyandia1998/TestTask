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
           var file = Request.Form.Files.First();
           var fileName = file.FileName;
           var csvParser = new CsvFileParser();

           using var fStream = file.OpenReadStream();
           var valueList = csvParser.Read(fStream).Select(o => ValueDTO.From(o, fileName)).Where(o => ValueDTO.Validate(o)).ToList();

           var fileNameFromDB = await _context.Values.Where(v => v.FileName == fileName).FirstOrDefaultAsync();

            if (fileNameFromDB != null) {

              _context.RemoveRange(_context.Values.Where(v=>v.FileName==fileName).ToList());
              await _context.SaveChangesAsync();
            }

           await _context.Values.AddRangeAsync(valueList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                if (ex.InnerException != null)
                    await Response.WriteAsync(ex.InnerException.Message);
                else {
                    await Response.WriteAsync(ex.Message);

                }

            }
            
       
           await Response.WriteAsync("Succes");
        }

        [HttpGet("UploadFileForm")]
        public async Task GetForm()
        {
          Response.ContentType = "text/html; charset=utf-8";
          await  Response.SendFileAsync("Form.html");
        }

        [HttpGet("GetRecords")]
        public async Task Get()
        {
           var list = _context.Values.ToList();

           await Response.WriteAsJsonAsync(  _context.Values.ToList()); 
        }


    }
}
