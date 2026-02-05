namespace FilaRapida.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; // ← Adicione = string.Empty
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Perfil { get; set; } = "ATENDENTE"; // ← Valor padrão
        public bool Ativo { get; set; } = true;
        
        public Usuario()
        {
            Ativo = true;
        }
    }
}