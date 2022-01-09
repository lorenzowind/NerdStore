using FluentValidation.Results;
using MediatR;
using NS.Core.Messages;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using NS.Order.API.Application.DTO;
using NS.Order.API.Application.Events;
using NS.Order.Domain.Orders;
using NS.Order.Domain.Vouchers;
using NS.Order.Domain.Vouchers.Specs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NS.Order.API.Application.Commands
{
    public class OrderCommandHandler : CommandHandler, IRequestHandler<AddOrderCommand, ValidationResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMessageBus _bus;

        public OrderCommandHandler(IOrderRepository orderRepository, IVoucherRepository voucherRepository, IMessageBus bus)
        {
            _orderRepository = orderRepository;
            _voucherRepository = voucherRepository;
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(AddOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var order = MapOrder(message);

            if (!await ApplyVoucher(message, order)) return ValidationResult;

            if (!ValidateOrder(order)) return ValidationResult;

            if (!await ProcessPayment(order, message)) return ValidationResult;

            order.AuthorizeOrder();

            order.AddEvent(new SucceedOrderEvent(order.Id, order.CustomerId));

            _orderRepository.Add(order);

            return await PersistData(_orderRepository.UnitOfWork);
        }

        private Domain.Orders.Order MapOrder(AddOrderCommand message)
        {
            var address = new Address
            {
                PublicArea = message.Address.PublicArea,
                Number = message.Address.Number,
                Complement = message.Address.Complement,
                District = message.Address.District,
                ZipCode = message.Address.ZipCode,
                City = message.Address.City,
                State = message.Address.State
            };

            var order = new Domain.Orders.Order(message.CustomerId, message.TotalPrice, message.OrderItens.Select(OrderItemDTO.ToOrderItem).ToList(),
                message.AppliedVoucher, message.Discount);

            order.SetAddress(address);

            return order;
        }

        private async Task<bool> ApplyVoucher(AddOrderCommand message, Domain.Orders.Order order)
        {
            if (!message.AppliedVoucher) return true;

            var voucher = await _voucherRepository.GetVoucherByCode(message.VoucherCode);

            if (voucher == null)
            {
                AddError("Invalid voucher");
                return false;
            }

            var voucherValidation = new VoucherValidation().Validate(voucher);

            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AddError(m.ErrorMessage));
                return false;
            }

            order.ApplyVoucher(voucher);

            voucher.DecreaseQuantity();

            _voucherRepository.Update(voucher);

            return true;
        }

        private bool ValidateOrder(Domain.Orders.Order order)
        {
            var orderPrice = order.TotalPrice;
            var orderDiscount = order.Discount;

            order.CalculateCartPriceWithDiscount();

            if (order.TotalPrice != orderPrice)
            {
                AddError("Total price does not match the order calculation");
                return false;
            }

            if (order.Discount != orderDiscount)
            {
                AddError("Discount value does not match the order calculation");
                return false;
            }

            return true;
        }

        public async Task<bool> ProcessPayment(Domain.Orders.Order order, AddOrderCommand message)
        {
            var initializedOrder = new InitializedOrderIntegrationEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Price = order.TotalPrice,
                PaymentType = 1,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                ExpirationMonthYear = message.CardExpiration,
                CVV = message.CardCvv
            };

            var result = await _bus.RequestAsync<InitializedOrderIntegrationEvent, ResponseMessage>(initializedOrder);

            if (result.ValidationResult.IsValid) return true;

            foreach (var error in result.ValidationResult.Errors)
            {
                AddError(error.ErrorMessage);
            }

            return false;
        }
    }
}
