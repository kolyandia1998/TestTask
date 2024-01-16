using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
           var uploadPath =  $"{Directory.GetCurrentDirectory()}/uploads";

            Directory.CreateDirectory(uploadPath);
            string fullPath = $"{uploadPath}/{file.FileName}";

           
            var csvParser = new CsvFileParser();
            using var fStream = file.OpenReadStream();
            foreach (var item in csvParser.Read(fStream)) { 
         
                 await Response.WriteAsJsonAsync( ValueDTO.From(item));
                
                }
        }



        



        [HttpGet("UploadFileForm")]
        public async Task GetForm()
        {
          Response.ContentType = "text/html; charset=utf-8";
          await  Response.SendFileAsync("Form.html");

        }


    }
}
