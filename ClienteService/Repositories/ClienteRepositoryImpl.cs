using System.Data;
using Dapper;
using ClienteService.Models;

namespace ClienteService.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IDbConnection _db;
        public ClienteRepository(IDbConnection db) { _db = db; }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            var sql = "SELECT Id, Nome, Email, Telefone FROM Clientes";
            return await _db.QueryAsync<Cliente>(sql);
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            var sql = "SELECT Id, Nome, Email, Telefone FROM Clientes WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Cliente>> FindByNameAsync(string nome)
        {
            var sql = "SELECT Id, Nome, Email, Telefone FROM Clientes WHERE Nome LIKE @Nome";
            return await _db.QueryAsync<Cliente>(sql, new { Nome = $"%{nome}%" });
        }

        public async Task<int> CountAsync()
        {
            var sql = "SELECT COUNT(1) FROM Clientes";
            return await _db.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> CreateAsync(Cliente cliente)
        {
            var sql = "INSERT INTO Clientes (Nome, Email, Telefone) VALUES (@Nome, @Email, @Telefone); SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await _db.ExecuteScalarAsync<int>(sql, cliente);
            return id;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            var sql = "UPDATE Clientes SET Nome = @Nome, Email = @Email, Telefone = @Telefone WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, cliente);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Clientes WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}