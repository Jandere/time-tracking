using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult<Result> HandleResult(Result result)
    {
        return result.Succeeded ? Ok() : BadRequest(result.ErrorsAsString);
    }
}