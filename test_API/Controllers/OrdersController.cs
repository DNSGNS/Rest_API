using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_API;
using Core.DTO;
using Core.DTO.Functions;
using Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        /// <summary>
        /// Получает список всех заказов.
        /// </summary>
        /// <returns>
        /// Список объектов
        /// </returns>
        /// <response code="200">Возвращает список заказов</response>
        /// <response code="204">Запрос выполнен успешно, но заказы не найдены</response>
        /// <response code="500">Внутренняя ошибка сервера при получении заказов</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return !orders.Any()
                 ? NotFound("No orders found")
                 : Ok(orders);
        }

        /// <summary>
        /// Получает список всех заказов с информацией о клиентах.
        /// </summary>
        /// <returns>
        /// Список заказов с включёнными данными о клиентах
        /// </returns>
        /// <response code="200">Успешно возвращает список заказов с клиентами</response>
        /// <response code="204">Запрос выполнен успешно, но заказы с клиентами не найдены</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных</response>
        [HttpGet("With Clients")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersWithClients()
        {
            var orders = await _orderService.GetOrdersWithClientsAsync();
            return !orders.Any()
                 ? NotFound("No orders found")
                 : Ok(orders);
        }



        /// <summary>
        /// Получает список заказов с возможностью фильтрации по различным параметрам, переданным через запрос.
        /// </summary>
        /// <param name="OrderDto"></param>
        /// <returns>Отфильтрованный список заказов</returns>
        /// <response code="200">Успешно возвращает отфильтрованный список заказов</response>
        /// <response code="204">Запрос выполнен успешно, но подходящих заказов не найдено</response>
        /// <response code="400">Неверно указаны параметры фильтрации</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных</response>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromQuery] FilterOrderDto OrderDto)
        {
            var orders = await _orderService.GetOrdersWithFilterAsync(OrderDto);
            return !orders.Any()
                 ? NotFound("No orders found")
                 : Ok(orders);
        }




        /// <summary>
        /// Получает список заказов с поддержкой пагинации. Возвращает заказы для указанной страницы.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации (первоначально — 1).</param>
        /// <param name="pageSize">Количество заказов на странице.</param>
        /// <returns>
        /// Список заказов на указанной странице 
        /// </returns>
        /// <response code="200">Успешно возвращает список заказов на указанной странице.</response>
        /// <response code="400">Неверно указаны параметры пагинации (например, отрицательные значения).</response>
        /// <response code="404">Запрос выполнен успешно, но заказы для указанной страницы не найдены.</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных.</response>
        [HttpGet("ByPage")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByPage([FromQuery] int page, [FromQuery] int pageSize)
        {
            var orders = await _orderService.GetOrdersByPageAsync(page, pageSize);
            return !orders.Any()
                 ? NotFound("No orders found")
                 : Ok(orders);
        }




        /// <summary>
        /// Получает информацию о заказе по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>
        /// Объект типа <see cref="GetOrderDto"/>, содержащий информацию о заказе с указанным идентификатором.
        /// В случае, если заказ не найден, возвращается код ответа 404.
        /// </returns>
        /// <response code="200">Успешно возвращает информацию о заказе с указанным ID.</response>
        /// <response code="404">Заказ с указанным ID не найден.</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id); // Получаем DTO вместо Order
            if (order == null)
            {
                return NotFound("No such order found"); // Возвращаем 404, если заказ не найден
            }
            return Ok(order); // Возвращаем заказ в виде GetOrderDto
        }


        /// <summary>
        /// Обновляет информацию о заказе по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа для обновления.</param>
        /// <param name="orderDto">Объект <see cref="UpdateOrderDto"/>, содержащий данные для обновления заказа.</param>
        /// <returns>
        /// Возвращает код ответа
        /// В случае ошибки возвращает соответствующие коды: 400 для некорректных данных и 404, если заказ не найден.
        /// </returns>
        /// <response code="204">Успешное обновление заказа.</response>
        /// <response code="400">Некорректные данные запроса (например, неверный ID клиента или mismatch в ID).</response>
        /// <response code="404">Заказ с указанным ID не найден.</response>
        /// <response code="500">Внутренняя ошибка сервера при обновлении данных.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, UpdateOrderDto orderDto)
        {
          
                var result = await _orderService.UpdateOrderAsync(id, orderDto);
                if (result == null)
                {
                    return NotFound("No such order found");
                }

                return NoContent();
        }


        /// <summary>
        /// Создаёт новый заказ на основе переданных данных.
        /// </summary>
        /// <param name="orderDto">Объект <see cref="CreateOrderDto"/>, содержащий данные для создания нового заказа.</param>
        /// <returns>
        /// Возвращает код ответа с созданным заказом 
        /// </returns>
        /// <response code="201">Успешно создан новый заказ. Возвращается информация о заказе.</response>
        /// <response code="400">Некорректные данные запроса (например, отсутствие ID клиента).</response>
        /// <response code="500">Внутренняя ошибка сервера при сохранении данных.</response>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(CreateOrderDto orderDto)
        {

            var result = await _orderService.CreateOrderAsync(orderDto);
            return Ok(result);
        }


        /// <summary>
        /// Удаляет заказ по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа, который нужно удалить.</param>
        /// <returns>
        /// Возвращает код ответа:
        /// - `204 No Content` в случае успешного удаления,
        /// - `404 Not Found`, если заказ с указанным идентификатором не найден.
        /// </returns>
        /// <response code="204">Заказ успешно удалён.</response>
        /// <response code="404">Заказ с указанным идентификатором не найден.</response>
        /// <response code="500">Внутренняя ошибка сервера при удалении данных.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result == false)
            {
                return NotFound("No such order found");
            }

            return NoContent();
        }

        private async Task<bool> OrderExists(int id)
        {
            return await _orderService.OrderExistsAsync(id);
        }

        [HttpGet("Function Avg")]
        public async Task<ActionResult<IEnumerable<FuncAvgCheckDto>>> GetReportAvg()
        {
            var result = await _orderService.GetReportAvgAsync();
            return Ok(result);
        }

        [HttpGet("Function Sum")]
        public async Task<ActionResult<IEnumerable<FuncSumOnBirthDayDto>>> GetReportSum()
        {
            var result = await _orderService.GetReportSumAsync();
            return Ok(result);
        }
    }
}
