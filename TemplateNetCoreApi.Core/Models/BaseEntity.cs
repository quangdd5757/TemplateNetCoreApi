using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateNetCoreApi.Core.Models
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedDate { get; set; }
    }

    public abstract class BaseTimeEntity
    {
        public long Id { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDate { get; set; }
    }

    public abstract class BaseCreateEntity
    {
        public long Id { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
    }

    public abstract class BaseSimpleEntity
    {
        public long Id { get; set; }
    }
}
