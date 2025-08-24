using System.Data;
using ClienteService.Models;
using ClienteService.Repositories;

namespace ClienteService.Services
{
    public class ClienteServiceImpl : IClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteServiceImpl(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cliente>> FindByNameAsync(string nome)
        {
            return await _repo.FindByNameAsync(nome);
        }

        public async Task<int> CountAsync()
        {
            return await _repo.CountAsync();
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            var id = await _repo.CreateAsync(cliente);
            cliente.Id = id;
            return cliente;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            return await _repo.UpdateAsync(cliente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}