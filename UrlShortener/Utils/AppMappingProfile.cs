using AutoMapper;
using UrlShortener.Models;
using UrlShortener.Models.Dto;

namespace UrlShortener.Utils
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<LinkDto, Link>();
        }
    }
}
