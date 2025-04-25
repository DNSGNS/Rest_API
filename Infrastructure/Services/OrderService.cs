using Core.DTO;
using Core.DTO.Functions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyDBcontext _context;

        public OrderService(MyDBcontext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetOrderDto>> GetOrdersAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Select(o => new GetOrderDto(o))
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetOrdersWithClientsAsync()
        {
            return await _context.Orders.AsNoTracking().Include(o => o.client).ToListAsync();
        }

        public async Task<IEnumerable<GetOrderDto>> GetOrdersByPageAsync(int page, int pageSize)
        {
            return await _context.Orders
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new GetOrderDto(o))
                .ToListAsync();
        }

        public async Task<GetOrderDto> GetOrderAsync(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Where(o => o.id == id)
                .Select(o => new GetOrderDto(o))
                .FirstOrDefaultAsync();

            // Если заказ не найден, вернем null
            return order;
        }
        public async Task<IEnumerable<GetOrderDto>> GetOrdersWithFilterAsync(FilterOrderDto filter)
        {
            var query = _context.Orders.AsQueryable();

            if (filter.amount > 0)
            {
                query = query.Where(o => o.amount > filter.amount);
            }

            if (!string.IsNullOrEmpty(filter.status.ToString()))
            {
                query = query.Where(o => o.status.ToString() == filter.status.ToString());
            }

            if (filter.day > 0)
            {
                query = query.Where(o => o.order_datetime.Day == filter.day);
            }

            if (filter.month > 0)
            {
                query = query.Where(o => o.order_datetime.Month == filter.month);
            }

            if (filter.year > 0)
            {
                query = query.Where(o => o.order_datetime.Year == filter.year);
            }

            if (filter.client_id > 0)
            {
                query = query.Where(o => o.client_id == filter.client_id);
            }

            return await query
                .AsNoTracking()
                .Select(o => new GetOrderDto(o))
                .ToListAsync();
        }

        public async Task<GetOrderDto> UpdateOrderAsync(int id, UpdateOrderDto orderDto)
        {

            Order newOrder = new Order
            {
                id = id,
                amount = orderDto.amount,
                status = orderDto.status,
                order_datetime = orderDto.order_datetime,
                client_id = orderDto.client_id
            };
            


            _context.Entry(newOrder).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return new GetOrderDto(newOrder);

        }
        public async Task<GetOrderDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            Order newOrder = new Order
            {
                amount = orderDto.amount,
                status = orderDto.status,
                order_datetime = orderDto.order_datetime,
                client_id = orderDto.client_id
            };
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return new GetOrderDto(newOrder);
        }


        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false; // Заказ не найден → false
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true; // Успешно удалён → true
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.id == id);
        }


        public async Task<IEnumerable<FuncAvgCheckDto>> GetReportAvgAsync()
        {
            return await _context.GetReportAvgAsync();
            
        }

        public bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.id == id);
        }

        public async Task<IEnumerable<FuncSumOnBirthDayDto>> GetReportSumAsync()
        {
            return await _context.GetReportSumAsync();
        }


    }
}
