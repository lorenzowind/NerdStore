using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NS.Core.Messages.Integration;
using NS.Identity.API.Models;
using NS.Identity.API.Services;
using NS.MessageBus;
using NS.WebAPI.Core.Controllers;

namespace NS.Identity.API.Controllers
{
    [Route("api/identity")]
    public class AuthController : MainController
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMessageBus _bus;

        public AuthController(AuthenticationService authenticationService, IMessageBus bus)
        {
            _authenticationService = authenticationService;
            _bus = bus;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUp(UserRegistration userRegistration)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegistration.Email,
                Email = userRegistration.Email,
                EmailConfirmed = true
            };

            var result = await _authenticationService.UserManager.CreateAsync(user, userRegistration.Password);

            if (result.Succeeded)
            {
                var customerResult = await RegisterCustomer(userRegistration);

                if (!customerResult.ValidationResult.IsValid)
                {
                    await _authenticationService.UserManager.DeleteAsync(user);
                    return CustomResponse(customerResult.ValidationResult);
                }

                return CustomResponse(await _authenticationService.GenerateJwt(user.Email));
            }

            foreach (var error in result.Errors)
            {
                AddProcessingError(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> SignIn(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authenticationService.SignInManager.PasswordSignInAsync(
                userLogin.Email, 
                userLogin.Password, 
                false, 
                true
            );

            if (result.Succeeded)
            {
                return CustomResponse(await _authenticationService.GenerateJwt(userLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AddProcessingError("User temporarily blocked due to invalid attempts");
                return CustomResponse();
            }

            AddProcessingError("Incorrect user or password");
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegisterCustomer(UserRegistration userRegistration)
        {
            var user = await _authenticationService.UserManager.FindByEmailAsync(userRegistration.Email);

            var registeredUser =
                new RegisteredUserIntegrationEvent(Guid.Parse(user.Id), userRegistration.Name, userRegistration.Email, userRegistration.Cpf);

            try
            {
                return await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registeredUser);
            }
            catch
            {
                await _authenticationService.UserManager.DeleteAsync(user);
                throw;
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> HandleRefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AddProcessingError("Invalid refresh token");
                return CustomResponse();
            }

            var token = await _authenticationService.GetRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                AddProcessingError("Expired refresh token");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GenerateJwt(token.Username));
        }
    }
}
