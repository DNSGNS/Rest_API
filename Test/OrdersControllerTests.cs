using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Services;
using Core.Models;
using Core.DTO;
using test_API.Controllers;
using Infrastructure.Interfaces;


namespace Test
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _controller = new OrdersController(_orderServiceMock.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOk_WithListOfOrders()
        {
            var orders = new List<GetOrderDto>
            {
                new GetOrderDto(1,156,Core.Enums.OrderStatus.completed,DateTime.Now,1),
                new GetOrderDto(2,200,Core.Enums.OrderStatus.completed,DateTime.Now,1)
            };
            _orderServiceMock
                .Setup(service => service.GetOrdersAsync())
                .ReturnsAsync(orders);

            var result = await _controller.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(orders, okResult.Value);
        }

        [Fact]
        public async Task GetOrders_ReturnsNotFound_WhenEmpty()
        {
            _orderServiceMock.Setup(s => s.GetOrdersAsync())
                             .ReturnsAsync(new List<GetOrderDto>());

            var result = await _controller.GetOrders();

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found", notFound.Value);
        }

        [Fact]
        public async Task GetOrdersWithClients_ReturnsGetOrderDto()
        {
            var orders = new List<Order> { new Order() };

            _orderServiceMock.Setup(s => s.GetOrdersWithClientsAsync())
                             .ReturnsAsync(orders);

            var result = await _controller.GetOrdersWithClients();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public async Task GetOrdersWithClients_ReturnsNotFound_WhenEmpty()
        {
            _orderServiceMock.Setup(s => s.GetOrdersWithClientsAsync())
                         .ReturnsAsync(new List<Order> { new Order ()});

            var result = await _controller.GetOrders();

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found", notFound.Value);
        }


        [Fact]
        public async Task GetOrderById_ReturnsOk_WhenExists()
        {
            int orderid = 1;
            var order = new GetOrderDto(1, 156, Core.Enums.OrderStatus.completed, DateTime.Now, 1);

            _orderServiceMock.Setup(s => s.GetOrderAsync(orderid))
                             .ReturnsAsync(order);

            var result = await _controller.GetOrder(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<GetOrderDto>(ok.Value);
            Assert.Equal(orderid, value.id);
        }

        [Fact]
        public async Task GetOrderById_ReturnsNotFound()
        {
            _orderServiceMock.Setup(s => s.GetOrderAsync(It.IsAny<int>()))
                             .ReturnsAsync((GetOrderDto)null!);

            var result = await _controller.GetOrder(99);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No such order found", notFound.Value);
        }

        [Fact]
        public async Task GetOrdersWithFilter_ReturnsOk()
        {
            var orders = new List<GetOrderDto>
            {
                new GetOrderDto(1,156,Core.Enums.OrderStatus.completed,DateTime.Now,1),
                new GetOrderDto(2,200,Core.Enums.OrderStatus.completed,DateTime.Now,1)
            };

            _orderServiceMock.Setup(s => s.GetOrdersWithFilterAsync(It.IsAny<FilterOrderDto>()))
                             .ReturnsAsync(orders);

            var result = await _controller.GetOrders(new FilterOrderDto());

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(orders, okResult.Value);
        }

        [Fact]
        public async Task GetOrdersWithFilter_ReturnsNotFound()
        {
            _orderServiceMock.Setup(s => s.GetOrdersWithFilterAsync(It.IsAny<FilterOrderDto>()))
                             .ReturnsAsync(new List<GetOrderDto>());

            var result = await _controller.GetOrders(new FilterOrderDto());

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found", notFound.Value);
        }

        [Fact]
        public async Task GetByPage_ReturnsOk()
        {
            int page = 2;
            int pagesize = 2;
            var orders = new List<GetOrderDto>
            {
                new GetOrderDto(1,156,Core.Enums.OrderStatus.completed,DateTime.Now,1),
                new GetOrderDto(2,200,Core.Enums.OrderStatus.completed,DateTime.Now,1)
            };

            _orderServiceMock.Setup(s => s.GetOrdersByPageAsync(page,pagesize))
                             .ReturnsAsync(orders);

            var result = await _controller.GetByPage(page,pagesize);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(orders, okResult.Value);

        }

        [Fact]
        public async Task GetByPage_ReturnsNotFound()
        {

            _orderServiceMock.Setup(s => s.GetOrdersByPageAsync(85, 14))
                             .ReturnsAsync(new List<GetOrderDto>());

            var result = await _controller.GetByPage(85, 14);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found", notFound.Value);
        }


        [Fact]
        public async Task PutOrder_ReturnsNoContent_WhenSuccessful()
        {
            var order = new GetOrderDto(1, 156, Core.Enums.OrderStatus.completed, DateTime.Now, 1);

            _orderServiceMock.Setup(s => s.UpdateOrderAsync(1, It.IsAny<UpdateOrderDto>()))
                             .ReturnsAsync(order);

            var result = await _controller.PutOrder(1, new UpdateOrderDto());

            Assert.IsType<NoContentResult>(result);
            
        }

        [Fact]
        public async Task PutOrder_ReturnsNotFound_WhenOrderNotFound()
        {
            _orderServiceMock.Setup(s => s.UpdateOrderAsync(1, It.IsAny<UpdateOrderDto>()))
                             .ReturnsAsync((GetOrderDto)null!);

            var result = await _controller.PutOrder(1, new UpdateOrderDto());

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No such order found", notFound.Value);
        }

        [Fact]
        public async Task PostOrder_ReturnsOkResult_WithCreatedOrder()
        {
            var order = new GetOrderDto(1, 156, Core.Enums.OrderStatus.completed, DateTime.Now, 1);
            _orderServiceMock.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderDto>()))
                             .ReturnsAsync(order);

            var result = await _controller.PostOrder(new CreateOrderDto());

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            ok.Value.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNoContent_WhenSuccessful()
        {
            _orderServiceMock.Setup(s => s.DeleteOrderAsync(1))
                             .ReturnsAsync(true);

            var result = await _controller.DeleteOrder(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNotFound_WhenOrderMissing()
        {
            _orderServiceMock.Setup(s => s.DeleteOrderAsync(1))
                             .ReturnsAsync(false);

            var result = await _controller.DeleteOrder(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No such order found", notFound.Value);
        }
    }
}
