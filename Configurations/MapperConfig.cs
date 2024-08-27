using AutoMapper;
using CoreApiInNet.Data;
using CoreApiInNet.Model;
using CoreApiInNet.Users;
using Microsoft.AspNetCore.Identity;

namespace CoreApiInNet.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig() 
        {
            CreateMap<DbModelData, HelpingModelData>().ReverseMap();
            CreateMap<FullDataModel, DbModelData>().ReverseMap();

            CreateMap<ApiUserDto, IdentityUser>().ReverseMap();
        }
    }
}
