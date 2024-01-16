using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    [Keyless]
    [Delimiter(";")]
    [HasHeaderRecord(false)]
   

    public class Value
    {
       
        [Format("yyyy-MM-d_hh-mm-ss")]
        public DateTime Date { get; set; }


       
        public int Second {  get; set; }

 
        public float Indicator { get; set; }



   



    }
}
