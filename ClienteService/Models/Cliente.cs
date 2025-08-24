namespace ClienteService.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
    }
}
