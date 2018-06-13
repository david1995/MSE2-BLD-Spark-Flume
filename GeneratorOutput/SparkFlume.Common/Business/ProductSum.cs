namespace SparkFlume.Common.Business
{
    public class ProductSum
    {
        public ProductSum(int id, long views, long purchases, decimal revenue)
        {
            Id = id;
            Views = views;
            Purchases = purchases;
            Revenue = revenue;
        }

        public int Id { get; }

        public long Views { get; }

        public long Purchases { get; }

        public decimal Revenue { get; }
    }
}
