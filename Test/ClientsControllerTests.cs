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
    public class ClientsControllerTests
    {
        private readonly Mock<IClientService> _clientServiceMock;
        private readonly ClientsController _controller;

        public ClientsControllerTests()
        {
            _clientServiceMock = new Mock<IClientService>();
            _controller = new ClientsController(_clientServiceMock.Object);
        }

        [Fact]
        public async Task GetClients_ReturnsOkResult_WithListOfClients()
        {
            // Arrange
            IEnumerable<GetClientDto> expectedClients = new List<GetClientDto>
        {
            new GetClientDto(1,"Иван","Иванов", DateTime.Now),
            new GetClientDto (2, "Мария", "Иванива", DateTime.Now)
        };

            _clientServiceMock
                .Setup(service => service.GetClientsAsync())
                .ReturnsAsync(expectedClients);

            // Act
            var result = await _controller.GetClients();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedClients);
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetClients_RetunsNotFound_WhenNoClients()
        {
            // Arrange
            IEnumerable<GetClientDto> expectedClients = new List<GetClientDto>();

            _clientServiceMock
                .Setup(service => service.GetClientsAsync())
                .ReturnsAsync(expectedClients);

            //Act
            var result = await _controller.GetClients();

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("No clients found");
        }

        [Fact]
        public async Task GetClient_ReturnsGetClientDto()
        {
            int clientid = 1;
            var client = new GetClientDto(clientid, "Иван", "Иванов", DateTime.Now);

            _clientServiceMock
                .Setup(service => service.GetClientAsync(clientid))
                .ReturnsAsync(client);

            var result = await _controller.GetClient(clientid);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GetClientDto>(okResult.Value);
            Assert.Equal(clientid, response?.id);
        }

        [Fact]
        public async Task GetClient_ReturnsNotFound_WhenNoClient()
        {
            int clientid = 99;

            _clientServiceMock
                .Setup(service => service.GetClientAsync(clientid))
                .ReturnsAsync((GetClientDto)null);

            var result = await _controller.GetClient(clientid);

            var NotFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No such client found", NotFoundResult.Value);
        }

        [Fact]
        public async Task GetClientsWithFilter_ReturnsFilteredClients()
        {
            IEnumerable<GetClientDto> expectedClients = new List<GetClientDto>
            {
                new GetClientDto(1,"Иван","Иванов", DateTime.Now),
                new GetClientDto (2, "Мария", "Иванива", DateTime.Now)
            };

            _clientServiceMock
                .Setup(service => service.GetClientsWithFilterAsync(It.IsAny<FilterClientDto>()))
                .ReturnsAsync(expectedClients);

            var result = await _controller.GetClientsWihtFilter(It.IsAny<FilterClientDto>());

            var OkResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetClientsWithFilter_ReturnsNotFound_WhenNoClients()
        {

            _clientServiceMock
                .Setup(service => service.GetClientsWithFilterAsync(It.IsAny<FilterClientDto>()))
                .ReturnsAsync(new List<GetClientDto>());

            var result = await _controller.GetClientsWihtFilter(It.IsAny<FilterClientDto>());

            var NotFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No clients found", NotFoundResult.Value);
        }

        [Fact]
        public async Task GetClientsByPage_ReturnsPaginatedClients()
        {
            int page = 1;
            int pagesize = 2;
            IEnumerable<GetClientDto> expectedClients = new List<GetClientDto>
            {
                new GetClientDto(1,"Иван","Иванов", DateTime.Now),
                new GetClientDto (2, "Мария", "Иванива", DateTime.Now)
            };

            _clientServiceMock
                .Setup(service => service.GetClientsByPageAsync(page,pagesize))
                .ReturnsAsync(expectedClients);

            var result = await _controller.GetClientsByPage(page, pagesize);

            var OkResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetClientsByPage_ReturnsNotFound_WhenNoClients()
        {
            int page = 52;
            int pagesize = 5;
            _clientServiceMock
              .Setup(service => service.GetClientsByPageAsync(page,pagesize))
              .ReturnsAsync(new List<GetClientDto>());

            var result = await _controller.GetClientsByPage(page, pagesize);

            var NotFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No clients found", NotFoundResult.Value);
        }

        [Fact]
        public async Task PostClients_ReturnsGetClientsDto()
        {
            var newClient = new CreateClientDto() {first_name = "Иван", second_name= "Иванов", birth_date = DateTime.Now };
            var client = new GetClientDto(1, "Иван", "Иванов", DateTime.Now);

            _clientServiceMock
                .Setup(service => service.CreateClientAsync(newClient))
                .ReturnsAsync(client);

            var result = await _controller.PostClient(newClient);

            var OkResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(client, OkResult.Value);
        }

        [Fact]
        public async Task PutClient_ReturnsNoContent_WhenUpdated()
        {
            int clientid = 1;
            var UpdatedClient = new GetClientDto(1, "Иван", "Иванов", DateTime.Now);

            _clientServiceMock
                .Setup(service => service.UpdateClientAsync(clientid, It.IsAny<UpdateClientDto>()))
                .ReturnsAsync(UpdatedClient);

            var result = await _controller.PutClient(clientid, It.IsAny<UpdateClientDto>());

            var OkResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutClient_ReturnsNotFound_WhenClientNotExists()
        {
            int clientid = 1;


            _clientServiceMock
                .Setup(service => service.UpdateClientAsync(clientid, It.IsAny<UpdateClientDto>()))
                .ReturnsAsync((GetClientDto)null);

            var result = await _controller.PutClient(clientid, It.IsAny<UpdateClientDto>());

            var NotFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No such client found", NotFoundResult.Value);
        }

        [Fact]
        public async Task  DeleteClient_ReturnsNoContent_WhenDeleted()
        {
            int clientid = 1;

            _clientServiceMock
                .Setup(service => service.DeleteClientAsync(clientid))
                .ReturnsAsync(true);

            var result = await _controller.DeleteClient(clientid);

            var OkResult = Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async Task DeleteClient_ReturnsNotFound_WhenClientNotExists()
        {
            int clientid = 99;

            _clientServiceMock
                .Setup(service => service.DeleteClientAsync(clientid))
                .ReturnsAsync(false);

            var result = await _controller.DeleteClient(clientid);

            var NotFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No such client found", NotFoundResult.Value);

        }


    }
}
