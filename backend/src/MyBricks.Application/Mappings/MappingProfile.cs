using AutoMapper;

namespace MyBricks.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Currently we do mostly manual mapping in handlers for better control, 
        // but AutoMapper is set up here for any complex nested entity->DTO mappings
        // e.g., CreateMap<LegoSet, LegoSetDto>();
    }
}
