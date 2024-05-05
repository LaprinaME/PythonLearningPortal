using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PythonLearningPortal.Models
{
    public class Роли
    {
        [Key]
        public int Код_роли { get; set; }

        [Required]
        [StringLength(50)]
        public string Название_роли { get; set; }
    }
}