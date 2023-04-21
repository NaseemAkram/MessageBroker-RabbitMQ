using Microsoft.AspNetCore.Mvc;

namespace RabbitMQProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class func_ImplementController : ControllerBase
    {

        private Func<string, string> _helloFunc = (name) =>
        {
            return $"hello func: {name}";
        };


        [HttpGet("name")]

        public async Task<IActionResult> Get(string name)
        {
            string msg = _helloFunc(name);
            return Ok(msg);
        }
    }
}
