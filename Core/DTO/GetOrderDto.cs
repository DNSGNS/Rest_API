using Core.Models;
using Core.Enums;

namespace Core.DTO
{
    public class GetOrderDto
    {

        public GetOrderDto(Order order)
        {
            id = order.id;
            amount = order.amount;
            status = order.status;
            order_datetime = order.order_datetime;
            client_id = order.client_id;
        }

        public GetOrderDto(int id, decimal amount, OrderStatus status, DateTime order_datetime, int client_id)
        {
            this.id = id;
            this.amount = amount;
            this.status = status;
            this.order_datetime = order_datetime;
            this.client_id = client_id;
        }

        public int id { get; set; }

        public decimal amount { get; set; }

        public OrderStatus status { get; set; } = OrderStatus.notprocessed;

        public DateTime order_datetime { get; set; }

        // Внешний ключ
        public int client_id { get; set; }
    }
}
