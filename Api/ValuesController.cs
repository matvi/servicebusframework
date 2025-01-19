using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace servicebusframework.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(new string[] { "value1", "value2", id });
        }
    }
}
