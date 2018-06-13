using System;

namespace SparkFlume.EventGenerator.Business
{
    public abstract class Event
    {
        public DateTime TimeStamp { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }
    }
}
