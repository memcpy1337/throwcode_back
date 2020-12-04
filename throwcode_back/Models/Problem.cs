using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace throwcode_back.Models
{

    public class Problem
    {
        [ForeignKey("Id")]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Solved { get; set; }
        public int Trying { get; set; }
        [Required]
        public ProblemDescription ProblemDescription { get; set; }
        public  User[] Solved_By { get; set; }
    }

    public class ProblemDescription
    {
        [ForeignKey("Id")]
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public string Description { get; set; }
        public string ExamplesJson { get; set; }
        public string Cheat { get; set; }
        public string InitialCodeJson { get; set; }
        public string TestCasesJson { get; set; }
        public Problem Problem { get; set; }
    }
}
