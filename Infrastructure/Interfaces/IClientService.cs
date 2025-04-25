using Core.DTO;
using Core.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces
{
    public interface IClientService
    {
        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> list of clients</returns>
        Task<IEnumerable<GetClientDto>> GetClientsAsync();

        /// <summary>
        /// Gets clients by page with specified <paramref name="page"/> and <paramref name="pageSize"/>
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns><see cref="IEnumerable{T}"/> list of paginated clients</returns>
        Task<IEnumerable<GetClientDto>> GetClientsByPageAsync(int page, int pageSize);

        /// <summary>
        /// Gets client by specified <paramref name="id"/>
        /// </summary>
        /// <param name="id">Client identifier</param>
        /// <returns><see cref="GetClientDto"/> with client details</returns>
        Task<GetClientDto> GetClientAsync(int id);

        /// <summary>
        /// Gets clients filtered by <paramref name="filter"/> criteria
        /// </summary>
        /// <param name="filter">Filter conditions</param>
        /// <returns><see cref="IEnumerable{T}"/> list of filtered clients</returns>
        Task<IEnumerable<GetClientDto>> GetClientsWithFilterAsync(FilterClientDto filter);

        /// <summary>
        /// Updates client with specified <paramref name="id"/> using <paramref name="PutClient"/> data
        /// </summary>
        /// <param name="id">Client identifier to update</param>
        /// <param name="PutClient">Updated client data</param>
        /// <returns><see cref="GetClientDto"/> with updated client details</returns>
        Task<GetClientDto> UpdateClientAsync(int id, UpdateClientDto PutClient);

        /// <summary>
        /// Creates new client using <paramref name="clientDto"/> data
        /// </summary>
        /// <param name="clientDto">Client data to create</param>
        /// <returns><see cref="GetClientDto"/> with created client details</returns>
        Task<GetClientDto> CreateClientAsync(CreateClientDto clientDto);

        /// <summary>
        /// Deletes client with specified <paramref name="id"/>
        /// </summary>
        /// <param name="id">Client identifier to delete</param>
        /// <returns><see cref="bool"/> indicating whether deletion was successful</returns>
        Task<bool> DeleteClientAsync(int id);

        /// <summary>
        /// Checks if client with specified <paramref name="id"/> exists
        /// </summary>
        /// <param name="id">Client identifier to check</param>
        /// <returns><see cref="bool"/> indicating whether client exists</returns>
        Task<bool> ClientExistsAsync(int id);
    }
}
