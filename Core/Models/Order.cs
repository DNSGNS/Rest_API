using System.Text.Json.Serialization;
using Core.Enums;
namespace Core.Models


{

    public class Order
    {
        public int id { get; set; }

        public decimal amount { get; set; }

        public OrderStatus status { get; set; } = OrderStatus.notprocessed;

        public DateTime order_datetime { get; set; }
        
        // Внешний ключ
        public int client_id { get; set; }

        // Навигационное свойство
        public Client? client { get; set; }
    }
}
