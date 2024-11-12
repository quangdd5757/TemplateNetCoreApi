using Microsoft.EntityFrameworkCore;
using TemplateNetCoreApi.Core.Models;
using TemplateNetCoreApi.Repo.Data;
using TemplateNetCoreApi.Repo.GenericRepository.Service;
using TemplateNetCoreApi.Service.Interfaces;

namespace TemplateNetCoreApi.Service.Services;

public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task CreateTeacher(Teacher teacher) => await CreateAsync(teacher);

    public async Task<IEnumerable<Teacher>> GetAllTeachers(bool trackChanges)
        => await FindAllAsync(trackChanges).Result.OrderBy(c => c.Name).ToListAsync();

    public async Task<Teacher?> GetTeacher(int teacherId, bool trackChanges)
        => await FindByConditionAsync(c => c.Id.Equals(teacherId), trackChanges).Result.SingleOrDefaultAsync();
}
