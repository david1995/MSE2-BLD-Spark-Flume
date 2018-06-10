using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles

namespace SparkFlume.EventGenerator.Entities
{
    public class EventDto
    {
        public long timestamp { get; set; }

        public string type { get; set; }

        public int customer_id { get; set; }

        public int product_id { get; set; }

        public decimal revenue { get; set; }
    }
}

#pragma warning restore IDE1006 // Naming Styles
