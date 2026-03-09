using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult test()
    {
        return Ok("I am working");
    }

}