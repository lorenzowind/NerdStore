using Microsoft.AspNetCore.Mvc;
using NS.Core.Mediator;
using NS.Customer.API.Application.Commands;
using NS.Customer.API.Models;
using NS.WebAPI.Core.Controllers;
using NS.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NS.Customer.API.Controllers
{
    public class CustomerController : MainController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAspNetUser _user;

        public CustomerController(IMediatorHandler mediatorHandler, ICustomerRepository customerRepository, IAspNetUser user)
        {
            _customerRepository = customerRepository;
            _mediatorHandler = mediatorHandler;
            _user = user;
        }

        [HttpGet("customer/address")]
        public async Task<IActionResult> GetAddress()
        {
            var address = await _customerRepository.GetAddressById(_user.GetUserId());
            return address == null ? NotFound() : CustomResponse(address);
        }

        [HttpPost("customer/address")]
        public async Task<IActionResult> AddAddress(AddAddressCommand address)
        {
            address.CustomerId = _user.GetUserId();
            return CustomResponse(await _mediatorHandler.SendCommand(address));
        }
    }
}
