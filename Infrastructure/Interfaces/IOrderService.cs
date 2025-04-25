using Core.DTO;
using Core.DTO.Functions;
using Core.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> list of orders</returns>
        Task<IEnumerable<GetOrderDto>> GetOrdersAsync();

        /// <summary>
        /// Gets all orders with included client information
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> list of orders with clients</returns>
        Task<IEnumerable<Order>> GetOrdersWithClientsAsync();

        /// <summary>
        /// Gets orders by page with specified <paramref name="page"/> and <paramref name="pageSize"/>
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns><see cref="IEnumerable{T}"/> list of paginated orders</returns>
        Task<IEnumerable<GetOrderDto>> GetOrdersByPageAsync(int page, int pageSize);

        /// <summary>
        /// Gets order by specified <paramref name="id"/>
        /// </summary>
        /// <param name="id">Order identifier</param>
        /// <returns><see cref="GetOrderDto"/> with order details</returns>
        Task<GetOrderDto> GetOrderAsync(int id);

        /// <summary>
        /// Gets orders filtered by <paramref name="filter"/> criteria
        /// </summary>
        /// <param name="filter">Filter conditions</param>
        /// <returns><see cref="IEnumerable{T}"/> list of filtered orders</returns>
        Task<IEnumerable<GetOrderDto>> GetOrdersWithFilterAsync(FilterOrderDto filter);

        /// <summary>
        /// Updates order with specified <paramref name="id"/> using <paramref name="orderDto"/> data
        /// </summary>
        /// <param name="id">Order identifier to update</param>
        /// <param name="orderDto">Updated order data</param>
        /// <returns><see cref="GetOrderDto"/> with updated order details</returns>
        Task<GetOrderDto> UpdateOrderAsync(int id, UpdateOrderDto orderDto);

        /// <summary>
        /// Creates new order using <paramref name="orderDto"/> data
        /// </summary>
        /// <param name="orderDto">Order data to create</param>
        /// <returns><see cref="GetOrderDto"/> with created order details</returns>
        Task<GetOrderDto> CreateOrderAsync(CreateOrderDto orderDto);

        /// <summary>
        /// Deletes order with specified <paramref name="id"/>
        /// </summary>
        /// <param name="id">Order identifier to delete</param>
        /// <returns><see cref="bool"/> indicating whether deletion was successful</returns>
        Task<bool> DeleteOrderAsync(int id);

        /// <summary>
        /// Checks if order with specified <paramref name="id"/> exists
        /// </summary>
        /// <param name="id">Order identifier to check</param>
        /// <returns><see cref="bool"/> indicating whether order exists</returns>
        Task<bool> OrderExistsAsync(int id);

        /// <summary>
        /// Gets report with average check information
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> list of average check reports</returns>
        Task<IEnumerable<FuncAvgCheckDto>> GetReportAvgAsync();

        /// <summary>
        /// Gets report with sum of orders on clients' birthdays
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> list of birthday sum reports</returns>
        Task<IEnumerable<FuncSumOnBirthDayDto>> GetReportSumAsync();
    }
}
