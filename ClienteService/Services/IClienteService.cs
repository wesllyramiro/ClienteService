using ClienteService.Models;

namespace ClienteService.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<IEnumerable<Cliente>> FindByNameAsync(string nome);
        Task<int> CountAsync();
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<bool> UpdateAsync(Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}