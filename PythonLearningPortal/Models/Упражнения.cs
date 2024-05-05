using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.Models
{
    public class Упражнения
    {
        [Key]
        public int Код_упражнения { get; set; }

        [Required]
        [StringLength(255)]
        public string Название_упражнения { get; set; }

        [Required]
        public string Описание { get; set; }
    }
}
