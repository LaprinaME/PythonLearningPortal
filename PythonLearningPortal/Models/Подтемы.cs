using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PythonLearningPortal.Models
{
    public class Подтемы
    {
        [Key]
        public int Код_подтемы { get; set; }

        [Required]
        [StringLength(255)]
        public string Название_подтемы { get; set; }

        [ForeignKey("Темы")]
        public int Код_темы { get; set; }
        public Темы Темы { get; set; }
    }
}