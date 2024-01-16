using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IndexAttribute = CsvHelper.Configuration.Attributes.IndexAttribute;

namespace TestTask.Models
{

    public record Value
    {
        public DateTime Date { get; set; }

        public int Second {  get; set; }


        public float Indicator { get; set; }

    }
}
