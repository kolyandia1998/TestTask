
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    
    public record Value
    {
        [Key]
        public DateTime Date { get; set; }

        public int Second { get; set; }

        public string FileName { get; set; }

        public float Indicator { get; set; }

    }
}
