using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TemplateNetCoreApi.Core.Models;

namespace TemplateNetCoreApi.Repo.Data
{
    public partial class RepositoryContext : IdentityDbContext<User, Role, long> // kế thừa DbContext của Identity
                                                             // để tích hợp định nghĩa/ mối quan hệ/ bảng/ cột 
                                                             // nếu đã sử dụng Data Annotation (các Attribute cho property tại class model) mà không đủ thì mới sử dụng thêm ở đây
                                                             // đây là pp sử dụng Fluent API, cần nạp chồng method OnModelCreating
    {

        public RepositoryContext()
        {
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // cần cài đặt Nuget Pomelo.EntityFrameworkCore.MySql để sử dụng optionBuilder.UseMySql
        => optionsBuilder.UseMySql("Server=dev.ttdesignco.com;port=3306;Database=Staging.QuangTest;Uid=rdteam;Pwd=rdTeam2020aDmin@!;", ServerVersion.Parse("8.0.26-mysql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.ApplyConfiguration(new TeacherData());
            modelBuilder.ApplyConfiguration(new StudentData());
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
