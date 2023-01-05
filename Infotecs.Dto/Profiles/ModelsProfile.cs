using AutoMapper;
using Infotecs.Domain.Models;
using Infotecs.Dto.Models;

namespace Infotecs.Dto.Profiles;

public class ModelsProfile : Profile
{
    public ModelsProfile()
    {
        CreateMap<Result, ResultDto>();
        CreateMap<Value, ValueDto>();
    }
}