using C_Tasks.Api.Models;
using C_Tasks.Api.Repository;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;

namespace C_Tasks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ResultRepository _repos;
        public ApiController(ResultRepository repos)
        {
            _repos = repos;
        }

        [HttpGet("/GetResult")]
        public ActionResult GetResult(int SortMethod, string enter) 
        {
            var result = _repos.GetResult(enter);

            if (result.Item2.Count() == 0) 
            {
                var unique = _repos.GetUnique(result.Item1);
                var substring = _repos.GetSubstring(result.Item1);
                var sorted = _repos.Sorted(SortMethod, result.Item1);
                var removed = _repos.GetStringWithDeletedSymbol(result.Item1);
                ResultModel model = new ResultModel
                {
                    Result = result.Item1,
                    Unique = unique,
                    SortedString = sorted,
                    LongSubstring = substring,
                    ResultWithoutSymbol = removed,
                };
                return Ok(model);
            }
            else
            {
                return BadRequest(result.Item2);
            }
            

        }
    }
}
