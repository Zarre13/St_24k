using Microsoft.AspNetCore.Mvc;
using ServiceStatusApi.Models;

namespace ServiceStatusApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    [HttpGet]
    public ActionResult<StatusInfo> GetStatus()
    {
        return Ok(new StatusInfo());
    }
}
