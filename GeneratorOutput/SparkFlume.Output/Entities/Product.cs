using System;
using System.ComponentModel.DataAnnotations;

namespace SparkFlume.Output.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        public DateTime Minute { get; set; }
        
        public long Views { get; set; }
        
        public long Purchases { get; set; }
        
        public decimal Revenue { get; set; }
    }
}
