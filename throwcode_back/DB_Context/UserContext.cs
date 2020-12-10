using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using throwcode_back.Models;
using Newtonsoft.Json;
using throwcode_back.Controllers.ProblemCompilers;

namespace throwcode_back.DB_Context
{
    public class UserContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UserContext() { }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemDescription> ProblemsDescription { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            //        modelBuilder.Entity<User>()
            //.HasMany(p => p.Posts)
            //.WithOne(p => p.User);
            //        modelBuilder.Entity<Profile>()
            //    .HasOne(u => u.User)
            //    .WithOne(u => u.Profile)
            //    .IsRequired();
            User admin = new User
            {
                Id = 1,
                Login = "admin",
                Password = "admin",
                Email = "admin@admin.ru",
                Solved_Problems = null

            };
            User slave = new User
            {
                Id = 2,
                Login = "slave",
                Password = "slave",
                Email = "slave@slave.ru",
                Solved_Problems = null
            };
            Problem summTwo = new Problem
            {
                Id = 1,
                Title = "Найти сумму двух чисел",
                Type = "Easy",
                Solved = 0,
                Trying = 0,
       

            };
            Problem minusTwo = new Problem
            {
                Id = 2,
                Title = "Найти разницу двух чисел",
                Type = "Easy",
                Solved = 0,
                Trying = 0
             
            };
            ProblemDescription minusTwoDesc = new ProblemDescription
            {
                ProblemId = 2,
                Description = "В этой задаче тербуется найти разность двух чисел",
                ExamplesJson = JsonConvert.SerializeObject(new[]
                {
                    new {  input = "a = 2, b = 1", output = "1" },
                    new {  input = "a = 3, b = 3", output = "0" }
                }),
                Cheat = "К этой задаче нет подсказок",
                Id = 2,
                InitialCodeJson = JsonConvert.SerializeObject(new { Javascript = "JS Code Here!", Csharp = "C# Code Here!", CPlus = "C++ Code Here!" }),
                TestCasesJson = JsonConvert.SerializeObject(new TestData[]
                {
                    new TestData { input = new string[] {"2", "1"}, output = "1" },
                    new TestData { input = new string[] {"2", "2"}, output = "0" },
                })

            };
            ProblemDescription summTwoDesc = new ProblemDescription
            {
                ProblemId = 1,
                Description = "В этой задаче тербуется найти сумму двух чисел",
                ExamplesJson = JsonConvert.SerializeObject(new[] 
                { 
                    new {  input = "a = 1, b = 2", output = "3" },
                    new {  input = "a = 3, b = 3", output = "6" }
                }),
                Cheat = "К этой задаче нет подсказок",
                Id = 1,
                InitialCodeJson = JsonConvert.SerializeObject(new { Javascript = "JS Code Here!", Csharp = "C# Code Here!", CPlus = "C++ Code Here!" }),
                TestCasesJson = JsonConvert.SerializeObject(new TestData[]
                {
                    new TestData { input = new string[] {"2", "1"}, output = "3" },
                    new TestData { input = new string[] {"2", "2"}, output = "4" },
                })
            };

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.Id).ValueGeneratedOnAdd();
                b.HasData(admin, slave);
                b.HasMany(p => p.Solved_Problems).WithMany(c => c.Solved_By);
            });
            modelBuilder.Entity<Problem>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.Id).ValueGeneratedOnAdd();
                b.HasData(summTwo, minusTwo);
                b.OwnsOne(p => p.ProblemDescription).HasData(summTwoDesc, minusTwoDesc);
            });
        }
    }
}
