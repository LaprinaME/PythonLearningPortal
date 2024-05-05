using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PythonLearningPortal.Models
{
    public class Темы
    {
        [Key]
        public int Код_темы { get; set; }

        [Required]
        [StringLength(255)]
        public string Название_темы { get; set; }
    }
}