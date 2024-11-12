using AutoMapper;
using TemplateNetCoreApi.Core.Dtos;
using TemplateNetCoreApi.Core.Models;

namespace TemplateNetCoreApi.Core.Mappings;
public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, StudentDto>();

        CreateMap<StudentCreationDto, Student>();

        CreateMap<StudentUpdateDto, Student>().ReverseMap();
    }
}
