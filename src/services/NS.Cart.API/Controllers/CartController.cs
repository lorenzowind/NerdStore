using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NS.Cart.API.Data;
using NS.Cart.API.Models;
using NS.WebAPI.Core.Controllers;
using NS.WebAPI.Core.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NS.Cart.API.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly CartContext _context;

        public CartController(IAspNetUser user, CartContext context)
        {
            _user = user;
            _context = context;
        }

        [HttpGet("cart")]
        public async Task<CustomerCart> GetCart()
        {
            return await GetCustomerCart() ?? new CustomerCart();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddCartItem(CartItem item)
        {
            var cart = await GetCustomerCart();

            if (cart == null)
                ManipulateNewCart(item);
            else
                ManipulateExistingCart(cart, item);

            if (!IsValidOperation()) return CustomResponse();

            await PersistData();
            return CustomResponse();
        }

        [HttpPost("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItem item)
        {
            var cart = await GetCustomerCart();

            var cartItem = await GetValidatedCartItem(productId, cart, item);

            if (cartItem == null) return CustomResponse();

            cart.UpdateQuantity(cartItem, item.Quantity);

            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();

            _context.CartItens.Update(cartItem);
            _context.CustomerCarts.Update(cart);

            await PersistData();
            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var cart = await GetCustomerCart();

            var cartItem = await GetValidatedCartItem(productId, cart);

            if (cartItem == null) return CustomResponse();

            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();

            cart.RemoveItem(cartItem);

            _context.CartItens.Remove(cartItem);
            _context.CustomerCarts.Update(cart);

            await PersistData();
            return CustomResponse();
        }

        [HttpPost]
        [Route("cart/apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(Voucher voucher)
        {
            var cart = await GetCustomerCart();

            cart.ApplyVoucher(voucher);

            _context.CustomerCarts.Update(cart);

            await PersistData();
            return CustomResponse();
        }

        private async Task<CustomerCart> GetCustomerCart()
        {
            return await _context.CustomerCarts
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        private void ManipulateNewCart(CartItem item)
        {
            var cart = new CustomerCart(_user.GetUserId());
            cart.AddItem(item);

            ValidateCart(cart);
            _context.CustomerCarts.Add(cart);
        }

        private void ManipulateExistingCart(CustomerCart cart, CartItem item)
        {
            var existingCartItem = cart.IsExistingCartItem(item);

            cart.AddItem(item);
            ValidateCart(cart);

            if (existingCartItem)
                _context.CartItens.Update(cart.GetCartItemByProductId(item.ProductId));
            else
                _context.CartItens.Add(item);

            _context.CustomerCarts.Update(cart);
        }

        private async Task<CartItem> GetValidatedCartItem(Guid productId, CustomerCart cart, CartItem item = null)
        {
            if (item != null && productId != item.ProductId)
            {
                AddProcessingError("Invalid product");
                return null;
            }

            if (cart == null)
            {
                AddProcessingError("Cart not found");
                return null;
            }

            var cartItem = await _context.CartItens
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (cartItem == null || !cart.IsExistingCartItem(cartItem))
            {
                AddProcessingError("Item not found in the cart");
                return null;
            }

            return cartItem;
        }

        private async Task PersistData()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddProcessingError("An error occurred while persisting data");
        }

        private void ValidateCart(CustomerCart cart)
        {
            if (!cart.IsValid()) cart.ValidationResult.Errors.ToList().ForEach(e => AddProcessingError(e.ErrorMessage));
        }
    }
}
