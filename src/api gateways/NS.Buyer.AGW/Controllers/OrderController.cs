using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NS.Buyer.AGW.Models;
using NS.Buyer.AGW.Services;
using NS.WebAPI.Core.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NS.Buyer.AGW.Controllers
{
    [Authorize]
    public class PedidoController : MainController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public PedidoController(
            ICatalogService catalogService,
            ICartService cartService,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _catalogService = catalogService;
            _cartService = cartService;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost]
        [Route("buyer/order")]
        public async Task<IActionResult> AddOrder(OrderDTO order)
        {
            var cart = await _cartService.GetCart();
            var products = await _catalogService.GetProducts(cart.Itens.Select(i => i.ProductId));
            var address = await _customerService.GetAddress();

            if (!await ValidateCartProducts(cart, products)) return CustomResponse();

            PopulateOrderData(cart, address, order);

            return CustomResponse(await _orderService.ConcludeOrder(order));
        }

        [HttpGet("buyer/order/last")]
        public async Task<IActionResult> GetLastOrder()
        {
            var order = await _orderService.GetLastOrder();
            if (order is null)
            {
                AddProcessingError("Order not found");
                return CustomResponse();
            }

            return CustomResponse(order);
        }

        [HttpGet("buyer/order/list-customer")]
        public async Task<IActionResult> GetListByCustomer()
        {
            var orders = await _orderService.GetListByCustomerId();

            return orders == null ? NotFound() : CustomResponse(orders);
        }

        private async Task<bool> ValidateCartProducts(CartDTO cart, IEnumerable<ProductDTO> products)
        {
            if (cart.Itens.Count != products.Count())
            {
                var unavailableItens = cart.Itens.Select(c => c.ProductId).Except(products.Select(p => p.Id)).ToList();

                foreach (var itemId in unavailableItens)
                {
                    var cartItem = cart.Itens.FirstOrDefault(c => c.ProductId == itemId);
                    AddProcessingError($"The item {cartItem.Name} is not available anymore, remove it from the order to continue");
                }

                return false;
            }

            foreach (var cartItem in cart.Itens)
            {
                var catalogProduct = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                if (catalogProduct.Value != cartItem.Price)
                {
                    var errorMsg = $"The product '{cartItem.Name}' changed the price (from: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", cartItem.Price)} to: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", catalogProduct.Value)}) since was added to the cart.";

                    AddProcessingError(errorMsg);

                    var removeResponse = await _cartService.RemoveCartItem(cartItem.ProductId);
                    if (HasResponseErrors(removeResponse))
                    {
                        AddProcessingError($"It was not possible to remove the product '{cartItem.Name}' automatically, _" +
                                                   "remove and add again in case you still want it");
                        return false;
                    }

                    cartItem.Price = catalogProduct.Value;
                    var addResponse = await _cartService.AddCartItem(cartItem);

                    if (HasResponseErrors(addResponse))
                    {
                        AddProcessingError($"It was not possible to update the product '{cartItem.Name}' automatically, _" +
                                                   "add again in case you still want it");
                        return false;
                    }

                    ClearProcessingErrors();
                    AddProcessingError(errorMsg + " We update the cart itens price, verify your order");

                    return false;
                }
            }

            return true;
        }

        private void PopulateOrderData(CartDTO cart, AddressDTO address, OrderDTO order)
        {
            order.VoucherCode = cart.Voucher?.Code;
            order.AppliedVoucher = cart.AppliedVoucher;
            order.TotalPrice = cart.TotalPrice;
            order.Discount = cart.Discount;
            order.OrderItens = cart.Itens;

            order.Address = address;
        }
    }
}
