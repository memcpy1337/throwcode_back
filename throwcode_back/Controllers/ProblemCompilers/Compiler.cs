using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace throwcode_back.Controllers.ProblemCompilers
{
    public class Compiler
    {
        public bool IsCorrect { get; set; }
        public string LastOutput { get; set; }
        public int RuntimeMS { get; set; }
        public long peakPagedMem = 0,
                 peakWorkingSet = 0,
                 peakVirtualMem = 0;
    }
}
