using Microsoft.AspNetCore.Mvc;
using NS.Core.Mediator;
using NS.Customer.API.Application.Commands;
using NS.WebAPI.Core.Controllers;
using System;
using System.Threading.Tasks;

namespace NS.Customer.API.Controllers
{
    public class CustomerController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomerController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatorHandler
                .SendCommand(new RegisterCustomerCommand(Guid.NewGuid(), "user1", "user1@email.com", "77598418098"));

            return CustomResponse(result);
        }
    }
}
