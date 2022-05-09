using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command) => Ok(await mediator.Send(command));

        [HttpPost]
        [Route("Create")]
        /*
         
        {
  "FirstName": "f",
  "LastName": "l",
  "EmailAddress": "w@w.ocm",
  "UserName": "u",
  "Password": "p"
}
         */
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command) => Ok(await mediator.Send(command));



        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command) => Ok(await mediator.Send(command));

    }
}
