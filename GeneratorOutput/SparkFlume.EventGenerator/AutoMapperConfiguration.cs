using AutoMapper;
using SparkFlume.EventGenerator.Business;
using SparkFlume.EventGenerator.Entities;

namespace SparkFlume.EventGenerator
{
    public class AutoMapperConfiguration
    {

        public void Configure(IMapperConfigurationExpression mapperConfiguration)
        {
            mapperConfiguration.CreateMap<PurchaseEvent, EventDto>()
                               .ForMember(e => e.customer_id, c => c.MapFrom(pe => pe.CustomerId))
                               .ForMember(e => e.product_id, c => c.MapFrom(pe => pe.ProductId))
                               .ForMember(e => e.revenue, c => c.MapFrom(pe => pe.Revenue))
                               .ForMember(e => e.timestamp, c => c.MapFrom(pe => pe.TimeStamp.Ticks))
                               .ForMember(e => e.type, c => c.UseValue("purchase"));

            mapperConfiguration.CreateMap<ViewEvent, EventDto>()
                               .ForMember(e => e.customer_id, c => c.MapFrom(pe => pe.CustomerId))
                               .ForMember(e => e.product_id, c => c.MapFrom(pe => pe.ProductId))
                               .ForMember(e => e.revenue, c => c.UseValue(0.0M))
                               .ForMember(e => e.timestamp, c => c.MapFrom(pe => pe.TimeStamp.Ticks))
                               .ForMember(e => e.type, c => c.UseValue("view"));
        }
    }
}
