using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PythonLearningPortal.Models
{
    public class Пользователи
    {
        [Key]
        public int Код_пользователя { get; set; }

        [Required]
        [StringLength(255)]
        public string ФИО { get; set; }

        [ForeignKey("Аккаунты")]
        public int Код_аккаунта { get; set; }
        public Аккаунты Аккаунты { get; set; }
    }
}