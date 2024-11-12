using Microsoft.EntityFrameworkCore;
using TemplateNetCoreApi.Core.Models;
using TemplateNetCoreApi.Repo.Data;
using TemplateNetCoreApi.Repo.GenericRepository.Service;
using TemplateNetCoreApi.Service.Interfaces;

namespace TemplateNetCoreApi.Service.Services;

public class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task CreateStudentForTeacher(int teacherId, Student student)
    {
        student.TeacherId = teacherId;
        await CreateAsync(student);
    }

    public async Task DeleteStudent(Student student) => await RemoveAsync(student);

    public async Task<Student?> GetStudent(int teacherId, int studentId, bool trackChanges)
        => await FindByConditionAsync(e => e.TeacherId.Equals(teacherId) && e.Id.Equals(studentId), trackChanges).Result.SingleOrDefaultAsync();
}
