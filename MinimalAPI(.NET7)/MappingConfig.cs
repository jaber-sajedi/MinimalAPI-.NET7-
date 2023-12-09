using AutoMapper;
using MinimalAPI_.NET7_.Models;
using MinimalAPI_.NET7_.Models.DTO;

namespace MinimalAPI_.NET7_
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();

        }
    }
}
