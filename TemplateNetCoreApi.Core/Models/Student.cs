﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateNetCoreApi.Core.Models
{
    public class Student // model demo
    {
        [Column("StudentId")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Student name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Class is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Class is 20 characters.")]
        public string? Class { get; set; }


        [ForeignKey(nameof(Teacher))]
        public long TeacherId { get; set; }


        public Teacher? Teacher { get; set; }
    }
}
