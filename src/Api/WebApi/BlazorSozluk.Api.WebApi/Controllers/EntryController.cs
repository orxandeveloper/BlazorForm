using BlazorSozluk.Api.Application.Features.Queries.GetEntries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSozluk.Api.WebApi.Controllers
{
    public class EntryController : Controller
    {
        readonly IMediator mediator;

        public EntryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
         public async Task<IActionResult> GetEntries([FromQuery]GetEntriesQuery query)
        {
            var entries=await mediator.Send(query);
            return Ok(entries);
        }
    }
}
