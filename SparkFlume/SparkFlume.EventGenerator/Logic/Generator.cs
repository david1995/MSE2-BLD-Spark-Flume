using System;
using System.Threading.Tasks;
using SparkFlume.Common.Business;

namespace SparkFlume.EventGenerator.Logic
{
    public class Generator
    {
        private readonly TimeSpan _generationTimeout;
        private readonly Random _random;
        private readonly int _minProductId;
        private readonly int _maxProductId;
        private readonly int _minCustomerId;
        private readonly int _maxCustomerId;
        private readonly decimal _minRevenue;
        private readonly decimal _maxRevenue;

        public Generator(Random randomGenerator, TimeSpan generationTimeout, int minProductId, int maxProductId, int minCustomerId, int maxCustomerId, decimal minRevenue, decimal maxRevenue)
        {
            _random = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            _generationTimeout = generationTimeout;
            _minProductId = minProductId;
            _maxProductId = maxProductId;
            _minCustomerId = minCustomerId;
            _maxCustomerId = maxCustomerId;
            _minRevenue = minRevenue;
            _maxRevenue = maxRevenue;
        }

        public async Task<Event> GetNextEvent()
        {
            await Task.Delay(_generationTimeout);

            var newEvent = _random.Next(0, 1) == 0
                ? (Event)new ViewEvent
                {
                    ProductId = _random.Next(_minProductId, _maxProductId),
                    CustomerId = _random.Next(_minCustomerId, _maxCustomerId),
                    TimeStamp = DateTime.Now
                }
                : new PurchaseEvent
                {
                    ProductId = _random.Next(_minProductId, _maxProductId),
                    CustomerId = _random.Next(_minCustomerId, _maxCustomerId),
                    TimeStamp = DateTime.Now,
                    Revenue = (decimal)_random.NextDouble() * _maxRevenue + _minRevenue
                };

            return newEvent;
        }
    }
}
