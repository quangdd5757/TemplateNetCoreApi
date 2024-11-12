using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateNetCoreApi.Core.Models;

namespace TemplateNetCoreApi.Repo.Data;

public class TeacherData : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasData(
            new Teacher
            {
                Id = 1,
                Name = "John",
                Subject = "Maths"
            },

            new Teacher
            {
                Id = 2,
                Name = "Femi",
                Subject = "English"
            });
    }
}
