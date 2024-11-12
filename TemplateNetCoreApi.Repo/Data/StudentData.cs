using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateNetCoreApi.Core.Models;

namespace TemplateNetCoreApi.Repo.Data;

public class StudentData : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasData(
            new Student
            {
                Id = 1,
                Name = "Azeez",
                Class = "Science",
                TeacherId = 1
            },

            new Student
            {
                Id = 2,
                Name = "Kamal",
                Class = "Management",
                TeacherId = 2
            },

            new Student
            {
                Id = 3,
                Name = "Benson",
                Class = "Science",
                TeacherId = 1
            });
    }
}
