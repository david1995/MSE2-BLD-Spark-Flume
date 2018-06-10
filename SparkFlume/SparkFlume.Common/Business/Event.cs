using System;

namespace SparkFlume.Common.Business
{
    public abstract class Event
    {
        public DateTime TimeStamp { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }
    }
}
