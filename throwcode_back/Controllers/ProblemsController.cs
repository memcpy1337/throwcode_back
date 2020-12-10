using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork_Backend.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using throwcode_back.Controllers.ProblemCompilers;
using throwcode_back.DB_Context;
using throwcode_back.Models;
using throwcode_back.Models.PostModels;

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
        [JwtAuthentication]
        public IActionResult GetProblem(int id)
        {
            var problem = _userContext.Problems.Where(p => p.Id == id).Select(p => p.ProblemDescription).Select(p => new
            {
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

        [HttpPost(Name = "SubmitProblem")]
        [JwtAuthentication]
        [Authorize]
        public IActionResult SubmitProblem([FromBody] PostProblem problem)
        {
            try
            {
                string testData = _userContext.Problems.Where(p => p.Id == problem.Id)
                                                       .Select(p => p.ProblemDescription)
                                                       .Select(p => p.TestCasesJson)
                                                       .FirstOrDefault();
                var user = User.Identities.FirstOrDefault().Claims
                                          .Where(c => c.Type == ClaimTypes.Name)
                                          .Select(c => c.Value)
                                          .SingleOrDefault();
                string runId = user + "_" + problem.Id;
                switch (problem.Lang)
                {
                    case "cplusplus":
                        {
                            CPlus cPlus = new CPlus();
                            cPlus.Run(problem.Code, testData, runId);
                            var problema = _userContext.Problems.Find(problem.Id);
                            if (cPlus.IsCorrect == true)
                            {
                                var userProblems = _userContext.Users.Include(b => b.Solved_Problems)
                                                                  .First(user1 => user1.Login == user).Solved_Problems;
                                if (userProblems.Where(problem1 => problem1.Id == problem.Id).Count() == 0)
                                {
                                    problema.Solved += 1;
                                    userProblems.Add(problema);
                                    _userContext.SaveChanges();
                                }
                            }else
                            {
                                problema.Trying += 1;
                                _userContext.SaveChanges();
                            }

                            return Ok(new { success = cPlus.IsCorrect, response = cPlus.LastOutput });
                        }
                    default:
                        return BadRequest();
                }

            }
            catch (Exception ex)
            {
               return Ok(new { success = true, response = ex.ToString() });
            }
            return Ok();
        }

    }
}