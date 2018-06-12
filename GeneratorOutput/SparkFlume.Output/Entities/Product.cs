using System;
using System.ComponentModel.DataAnnotations;

namespace SparkFlume.Output.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Key]
        public DateTime Minute { get; set; }

        public long Views { get; set; }

        public long Purchases { get; set; }
    }
}
