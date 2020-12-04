using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using throwcode_back.DB_Context;

namespace SocialNetwork_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProblemsController : ControllerBase
    {
        private readonly UserContext _userContext;
        public ProblemsController(UserContext context)
        {
            _userContext = context;
            Console.WriteLine();
        }
        [HttpGet(Name = "GetProblemsPreview")]
        public IActionResult GetProblemsPreview()
        {
            var problems = _userContext.Problems.Select(p => new
            {
                id = p.Id,
                title = p.Title,
                type = p.Type,
                solved = p.Solved,
                trying = p.Trying
            });
            if (problems == null)
            {
                return NotFound();
            }
            var output = new
            {
                items = problems,
                itemsNumber = problems.Count()
            };
            return Ok(output);
        }
        [HttpGet("{id}", Name = "GetProblem")]
        public IActionResult GetProblem(int id)
        {
            var problem = _userContext.Problems.Where(p => p.Id == id).Select(p => p.ProblemDescription).Select(p => new { 
                id = p.Id,
                text = p.Description,
                cheats = p.Cheat,
                initialCode = p.InitialCodeJson,
                examples = p.ExamplesJson
            }).FirstOrDefault();
            if (problem == null)
            {
                return NotFound();
            }
            var output = new
            {
                item = problem
            };
            return Ok(output);
        }

    }
}