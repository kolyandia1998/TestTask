using System;
using System.Collections.Generic;
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
            var v = new ValueDTO();


            ValueDTO.From(new string[] { "123", "1213" });


           var file = Request.Form.Files.First();
           var uploadPath =  $"{Directory.GetCurrentDirectory()}/uploads";

            Directory.CreateDirectory(uploadPath);
            string fullPath = $"{uploadPath}/{file.FileName}";

            using ( var fileStream = new FileStream(fullPath, FileMode.Create) )
            { 
                 await file.CopyToAsync( fileStream );
            }

            var FileReader = new CsvFileParser();
            List<Value> list =  FileReader.Parse<Value>(fullPath).ToList();



            await Response.WriteAsJsonAsync( list[0]);


  

        }



        [HttpGet("UploadFileForm")]
        public async Task GetForm()
        {
          Response.ContentType = "text/html; charset=utf-8";
          await  Response.SendFileAsync("Form.html");

        }


    }
}
