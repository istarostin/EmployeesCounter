using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesCounter.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCounter.ORM
{
    public class EFContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProgLang> ProgLangs { get; set; }

        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        internal Task<List<Department>> DepartmentsToListAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=master;Database=testdb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(f => f.DepartmentId);

            modelBuilder
                 .Entity<Employee>()
                 .HasOne(u => u.ProgLang)
                 .WithMany(p => p.Employees)
                 .HasForeignKey(f => f.ProgLangId);

            modelBuilder.Entity<Employee>().HasData(
                new Employee[]
                {
                    new Employee {Id = 1, Name = "Иван", Surname = "Васин", Sex = 'М', Age = 26, DepartmentId = 1, ProgLangId = 1},
                    new Employee {Id = 2, Name = "Сергей", Surname = "Суриков", Sex = 'М', Age = 45, DepartmentId = 1, ProgLangId = 2},
                    new Employee {Id = 3, Name = "Дмитрий", Surname = "Ковалев", Sex = 'М', Age = 32, DepartmentId = 1, ProgLangId = 3},
                    new Employee {Id = 4, Name = "Татьяна", Surname = "Шмидт", Sex = 'Ж', Age = 25, DepartmentId = 2, ProgLangId = 4},
                    new Employee {Id = 5, Name = "Анастасия", Surname = "Зорина", Sex = 'Ж', Age = 35, DepartmentId = 2, ProgLangId = 5},
                    new Employee {Id = 6, Name = "Петр", Surname = "Сергеев", Sex = 'М', Age = 19, DepartmentId = 3, ProgLangId = 1}
                });

            modelBuilder.Entity<Department>().HasData(
                new Department[]
                {
                    new Department {Id = 1, Floor = 5, Title = "Банковский сектор"},
                    new Department {Id = 2, Floor = 5, Title = "Постоянный заказчик"},
                    new Department {Id = 3, Floor = 5, Title = "Автосалоны"}
                });

            modelBuilder.Entity<ProgLang>().HasData(
                new ProgLang[]
                {
                    new ProgLang {Id = 1, Title = "C#"},
                    new ProgLang {Id = 2, Title = "PHP"},
                    new ProgLang {Id = 3, Title = "JavaScript"},
                    new ProgLang {Id = 4, Title = "Java"},
                    new ProgLang {Id = 5, Title = "Python"}
                });
        }

    }
}
