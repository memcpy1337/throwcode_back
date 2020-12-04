using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace throwcode_back.Models
{
    public class User
    {
        [ForeignKey("Id")]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Problem[] Solved_Problems { get; set; }
    }

}
