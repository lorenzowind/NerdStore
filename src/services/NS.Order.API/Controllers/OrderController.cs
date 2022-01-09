using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NS.Core.Mediator;
using NS.Order.API.Application.Commands;
using NS.Order.API.Application.Queries;
using NS.WebAPI.Core.Controllers;
using NS.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NS.Order.API.Controllers
{
    [Authorize]
    public class OrderController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;
        private readonly IOrderQueries _orderQueries;

        public OrderController(IMediatorHandler mediatorHandler, IAspNetUser user, IOrderQueries orderQueries)
        {
            _mediator = mediatorHandler;
            _user = user;
            _orderQueries = orderQueries;
        }

        [HttpPost("order")]
        public async Task<IActionResult> AddOrder(AddOrderCommand order)
        {
            order.CustomerId = _user.GetUserId();
            return CustomResponse(await _mediator.SendCommand(order));
        }

        [HttpGet("order/last")]
        public async Task<IActionResult> GetLastOrder()
        {
            var order = await _orderQueries.GetLastOrder(_user.GetUserId());
            return order == null ? NotFound() : CustomResponse(order);
        }

        [HttpGet("order/list-customer")]
        public async Task<IActionResult> GetListByCustomer()
        {
            var orders = await _orderQueries.GetListByCustomerId(_user.GetUserId());
            return orders == null ? NotFound() : CustomResponse(orders);
        }
    }
}
