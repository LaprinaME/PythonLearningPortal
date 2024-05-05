using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Models;

namespace PythonLearningPortal.DataContext
{
    public class PythonLearningPortalContext : DbContext
    {
        public DbSet<Роли> Роли { get; set; }
        public DbSet<Аккаунты> Аккаунты { get; set; }
        public DbSet<Пользователи> Пользователи { get; set; }
        public DbSet<Темы> Темы { get; set; }
        public DbSet<Подтемы> Подтемы { get; set; }
        public DbSet<Упражнения> Упражнения { get; set; }
        public DbSet<Тесты> Тесты { get; set; } 

        public PythonLearningPortalContext(DbContextOptions<PythonLearningPortalContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}