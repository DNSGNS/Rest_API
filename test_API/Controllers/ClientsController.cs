using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using test_API;
using Core.DTO;
using Core.Models;
using Core.Enums;
using Infrastructure.Services;
using Infrastructure.Interfaces;


namespace test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Получает список всех клиентов.
        /// </summary>
        /// <returns>
        /// Возвращает список клиентов 
        /// </returns>
        /// <response code="200">Успешно возвращен список клиентов.</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных.</response>
        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var result = await _clientService.GetClientsAsync();
            return !result.Any()
                ? NotFound("No clients found")
                : Ok(result);

        }

        /// <summary>
        /// Получает список клиентов с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы, для которой нужно получить клиентов.</param>
        /// <param name="pageSize">Количество клиентов на одной странице.</param>
        /// <returns>
        /// Возвращает список клиентов 
        /// </returns>
        /// <response code="200">Успешно возвращен список клиентов на указанной странице.</response>
        /// <response code="400">Ошибка в запросе, например, отрицательные значения для page или pageSize.</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных.</response>
        // GET: api/Clients
        [HttpGet("ByPage")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsByPage([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await _clientService.GetClientsByPageAsync(page, pageSize);

            return !result.Any()
               ? NotFound("No clients found")
               : Ok(result);

        }


        /// <summary>
        /// Получает клиента по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор клиента, которого нужно получить.</param>
        /// <returns>
        /// Возвращает клиентский объект
        /// </returns>
        /// <response code="200">Успешно найден клиент. Возвращена информация о клиенте.</response>
        /// <response code="404">Клиент с указанным идентификатором не найден.</response>
        /// <response code="500">Внутренняя ошибка сервера при получении данных.</response>
        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetClientDto>> GetClient(int id)
        {
            var result = await _clientService.GetClientAsync(id);

            if (result == null)
            {
                return NotFound("No such client found");
            }

            return Ok(result);
        }


        /// <summary>
        /// Получает список клиентов с применением фильтров.
        /// </summary>
        /// <param name="ClientDto">DTO, содержащий параметры для фильтрации клиентов:</param>
        /// <returns>
        /// Возвращает отфильтрованный список клиентов
        /// </returns>
        /// <response code="200">Успешно возвращен отфильтрованный список клиентов.</response>
        /// <response code="500">Внутренняя ошибка сервера при обработке запроса.</response>
        // GET: api/Clients
        [HttpGet("With filters")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsWihtFilter([FromQuery] FilterClientDto ClientDto)
        {
            var result = await _clientService.GetClientsWithFilterAsync(ClientDto);
            return !result.Any()
                ? NotFound("No clients found")
                : Ok(result);
        }


        /// <summary>
        /// Обновляет информацию о клиенте по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор клиента, который необходимо обновить.</param>
        /// <param name="client">Объект клиента с обновленными данными.</param>
        /// <returns>
        /// Возвращает статус выполнения операции
        /// </returns>
        /// <response code="204">Успешное обновление данных клиента.</response>
        /// <response code="400">Ошибка запроса, например, если идентификаторы не совпадают.</response>
        /// <response code="404">Клиент с указанным идентификатором не найден.</response>
        /// <response code="500">Внутренняя ошибка сервера при обновлении данных клиента.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, UpdateClientDto client)
        {

            var result = await _clientService.UpdateClientAsync(id, client);

            if (result == null)
            {
                return NotFound("No such client found");
            }

            return NoContent();
        }



        /// <summary>
        /// Создает нового клиента.
        /// </summary>
        /// <param name="clientDto">DTO, содержащий информацию о новом клиенте.</param>
        /// <returns>
        /// Возвращает результат выполнения операции:
        /// - Если клиент успешно создан, возвращает объект клиента с его идентификатором.
        /// </returns>
        /// <response code="201">Клиент успешно создан. Возвращается объект клиента с его идентификатором.</response>
        /// <response code="400">Ошибка запроса, если входные данные клиента некорректны.</response>
        /// <response code="500">Внутренняя ошибка сервера при создании клиента.</response>
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(CreateClientDto clientDto)
        {

            var result = await _clientService.CreateClientAsync(clientDto);
            return Ok(result);

        }


        /// <summary>
        /// Удаляет клиента по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор клиента, которого необходимо удалить.</param>
        /// <returns>
        /// Возвращает статус выполнения операции
        /// </returns>
        /// <response code="204">Клиент успешно удален.</response>
        /// <response code="404">Клиент с указанным идентификатором не найден.</response>
        /// <response code="500">Внутренняя ошибка сервера при удалении клиента.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);

            if (result == false)
            {
                return NotFound("No such client found");
            }

            return NoContent();
        }

        private async Task<bool> ClientExists(int id)
        {
            return await _clientService.ClientExistsAsync(id);
        }
    }
}
