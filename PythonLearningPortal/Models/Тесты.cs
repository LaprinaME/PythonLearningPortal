using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.Models
{
    public class Тесты
    {
        [Key]
        public int Код_теста { get; set; }

        [Required]
        [StringLength(255)]
        public string Название_теста { get; set; }
    }
}
