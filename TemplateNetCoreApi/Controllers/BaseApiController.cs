using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TemplateNetCoreApi.Service.Interfaces;

namespace TemplateNetCoreApi.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IRepositoryManager _repository;
    protected readonly ILogger<BaseApiController> _logger;
    protected readonly IMapper _mapper;

    public BaseApiController(IRepositoryManager repository, ILogger<BaseApiController> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
}
