using Core.Models;
using Core.Enums;

namespace Core.DTO
{
    public class UpdateOrderDto
    {
        // Сумма заказа
        public decimal amount { get; set; }

        // Статус заказа
        public OrderStatus status { get; set; } = OrderStatus.notprocessed;

        // Дата и время заказа
        public DateTime order_datetime { get; set; }

        // Идентификатор клиента (внешний ключ)
        public int client_id { get; set; }
    }
}
