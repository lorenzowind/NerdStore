using Dapper;
using NS.Order.API.Application.DTO;
using NS.Order.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.Order.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDTO> GetLastOrder(Guid customerId);
        Task<IEnumerable<OrderDTO>> GetListByCustomerId(Guid customerId);
        Task<OrderDTO> GetAuthorizedOrders();
    }

    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDTO> GetLastOrder(Guid customerId)
        {

            const string sql = @"SELECT
                                P.ID AS 'ProductId', P.CODE, P.APPLIEDVOUCHER, P.DISCOUNT, P.TOTALPRICE,P.ORDERSTATUS,
                                P.PUBLICAREA,P.NUMBER, P.DISTRICT, P.ZIPCODE, P.COMPLEMENT, P.CITY, P.STATE,
                                PIT.ID AS 'ProductItemId',PIT.PRODUCTNAME, PIT.QUANTITY, PIT.PRODUCTIMAGE, PIT.UNITARYPRICE 
                                FROM ORDERS P 
                                INNER JOIN ORDERITENS PIT ON P.ID = PIT.ORDERID 
                                WHERE P.CUSTOMERID = @customerId 
                                AND P.REGISTRATIONDATE between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND P.ORDERSTATUS = 1 
                                ORDER BY P.REGISTRATIONDATE DESC";

            var order = await _orderRepository.GetConnection()
                .QueryAsync<dynamic>(sql, new { customerId });

            return MapOrder(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetListByCustomerId(Guid customerId)
        {
            var orders = await _orderRepository.GetListByCustomerId(customerId);

            return orders.Select(OrderDTO.ToOrderDTO);
        }

        public async Task<OrderDTO> GetAuthorizedOrders()
        {
            const string sql = @"SELECT 
                                P.ID as 'OrderId', P.ID, P.CUSTOMERID, 
                                PI.ID as 'OrderItemId', PI.ID, PI.PRODUCTID, PI.QUANTITY 
                                FROM ORDERS P 
                                INNER JOIN ORDERITENS PI ON P.ID = PI.ORDERID 
                                WHERE P.ORDERSTATUS = 1                                
                                ORDER BY P.REGISTRATIONDATE";

            //var lookup = new Dictionary<Guid, OrderDTO>();

            var order = await _orderRepository.GetConnection().QueryAsync<OrderDTO, OrderItemDTO, OrderDTO>(sql,
                (p, pi) =>
                {
                    //if (!lookup.TryGetValue(p.Id, out var orderDTO))
                    //    lookup.Add(p.Id, orderDTO = p);

                    p.OrderItens ??= new List<OrderItemDTO>();
                    p.OrderItens.Add(pi);

                    return p;

                }, splitOn: "OrderId,OrderItemId");

            return order.FirstOrDefault();
            //return lookup.Values.OrderBy(p => p.Date).FirstOrDefault();
        }

        private OrderDTO MapOrder(dynamic result)
        {
            if (result.Count == 0) return null;

            var order = new OrderDTO
            {
                Code = result[0].CODE,
                Status = result[0].ORDERSTATUS,
                TotalPrice = result[0].TOTALPRICE,
                Discount = result[0].DISCOUNT,
                AppliedVoucher = result[0].APPLIEDVOUCHER,

                OrderItens = new List<OrderItemDTO>(),
                Address = new AddressDTO
                {
                    PublicArea = result[0].PUBLICAREA,
                    District = result[0].DISTRICT,
                    ZipCode = result[0].ZIPCODE,
                    City = result[0].CITY,
                    Complement = result[0].COMPLEMENT,
                    State = result[0].STATE,
                    Number = result[0].NUMBER
                }
            };

            foreach (var item in result)
            {
                var orderItem = new OrderItemDTO
                {
                    Name = item.PRODUCTNAME,
                    Price = item.UNITARYPRICE,
                    Quantity = item.QUANTITY,
                    Image = item.PRODUCTIMAGE
                };

                order.OrderItens.Add(orderItem);
            }

            return order;
        }
    }
}
