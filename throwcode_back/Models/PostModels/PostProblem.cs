using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace throwcode_back.Models.PostModels
{
    public class PostProblem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Lang { get; set; }
    }
}
