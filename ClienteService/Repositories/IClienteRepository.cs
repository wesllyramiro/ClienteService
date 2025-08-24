using ClienteService.Models;

namespace ClienteService.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<IEnumerable<Cliente>> FindByNameAsync(string nome);
        Task<int> CountAsync();
        Task<int> CreateAsync(Cliente cliente);
        Task<bool> UpdateAsync(Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}
