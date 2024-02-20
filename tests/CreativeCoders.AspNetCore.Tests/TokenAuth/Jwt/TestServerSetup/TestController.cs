using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class TestController : ControllerBase
{
    [Authorize("TestPolicy")]
    [HttpGet("get-data")]
    public IActionResult GetData()
    {
        return Ok(new { testProp = "TestValue1" });
    }
}
