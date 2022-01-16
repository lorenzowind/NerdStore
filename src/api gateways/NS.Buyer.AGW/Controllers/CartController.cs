using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NS.Buyer.AGW.Models;
using NS.Buyer.AGW.Services;
using NS.Buyer.AGW.Services.gRPC;
using NS.WebAPI.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NS.Buyer.AGW.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        private readonly ICartService _cartService;
        private readonly ICartGrpcService _cartGrpcService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, ICartGrpcService cartGrpcService, 
            ICatalogService catalogService, IOrderService orderService)
        {
            _cartService = cartService;
            _cartGrpcService = cartGrpcService;
            _catalogService = catalogService;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("buyer/cart")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _cartService.GetCart());
        }

        [HttpGet]
        [Route("buyer/cart-quantity")]
        public async Task<int> GetQuantityCart()
        {
            var cart = await _cartService.GetCart();
            return cart?.Itens.Sum(i => i.Quantity) ?? 0;
        }

        [HttpPost]
        [Route("buyer/cart/itens")]
        public async Task<IActionResult> AddCartItem(CartItemDTO item)
        {
            var product = await _catalogService.GetProductById(item.ProductId);

            await ValidateCartItem(product, item.Quantity);
            if (!IsValidOperation()) return CustomResponse();

            item.Name = product.Name;
            item.Price = product.Value;
            item.Image = product.Image;

            var response = await _cartService.AddCartItem(item);

            return CustomResponse(response);
        }

        [HttpPut]
        [Route("buyer/cart/itens/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItemDTO item)
        {
            var product = await _catalogService.GetProductById(item.ProductId);

            await ValidateCartItem(product, item.Quantity);
            if (!IsValidOperation()) return CustomResponse();

            var response = await _cartService.UpdateCartItem(productId, item);

            return CustomResponse(response);
        }

        [HttpDelete]
        [Route("buyer/cart/itens/{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var product = await _catalogService.GetProductById(productId);

            if (product == null)
            {
                AddProcessingError("Invalid product");
                return CustomResponse();
            }

            var response = await _cartService.RemoveCartItem(productId);

            return CustomResponse(response);
        }

        [HttpPost]
        [Route("buyer/cart/apply-voucher")]
        public async Task<IActionResult> ApplyVoucher([FromBody] string voucherCode)
        {
            var voucher = await _orderService.GetVoucherByCode(voucherCode);

            if (voucher == null)
            {
                AddProcessingError("Invalid voucher");
                return CustomResponse();
            }

            var response = await _cartService.ApplyCartVoucher(voucher);

            return CustomResponse(response);
        }

        private async Task ValidateCartItem(ProductDTO product, int quantity)
        {
            if (product == null) AddProcessingError("Invalid product");
            if (quantity < 1) AddProcessingError($"Quantity of product \"{product.Name}\" should be greater than 1");

            var cart = await _cartService.GetCart();
            var cartItem = cart.Itens.FirstOrDefault(i => i.ProductId == product.Id);

            if (cartItem != null && cartItem.Quantity + quantity > product.StorageQuantity)
            {
                AddProcessingError($"Product \"{product.Name}\" has only {product.StorageQuantity} units available");
                return;
            }

            if (quantity > product.StorageQuantity) 
                AddProcessingError($"Product \"{product.Name}\" has only {product.StorageQuantity} units available");
        }
    }
}
