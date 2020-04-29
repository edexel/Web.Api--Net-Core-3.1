using AutoMapper;
using Web.Api.Data.Models;
using Web.Api.Object.Requests.User;

namespace Web.Api.Data.Mappers
{
    public class AutoMapping : Profile
    {
        public  AutoMapping()
        {
           
            CreateMap(typeof(CUser), typeof(UserRequest)).ReverseMap();
        }


    }
}
