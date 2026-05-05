using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Budget API is running. Current DateTime: " + DateTime.Now.ToString());
        }
    }
}
