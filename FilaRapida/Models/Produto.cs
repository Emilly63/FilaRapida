using System;

namespace FilaRapida.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }

        public string Status => Ativo ? "Ativo" : "Inativo";
        public string PrecoFormatado => Preco.ToString("C2");

        public Produto()
        {
            Ativo = true;
            CriadoEm = DateTime.Now;
        }

        public bool Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome) || Nome.Length < 3)
                return false;
            
            if (Preco <= 0)
                return false;
            
            return true;
        }
    }
}
