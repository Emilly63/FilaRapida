using FilaRapida.Forms;
using FilaRapida.Models;

namespace FilaRapida
{
    static class Program
    {
        public static Usuario UsuarioLogado { get; set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // USUÁRIO TESTE (remova depois)
            UsuarioLogado = new Usuario
            {
                Id = 1,
                Nome = "Administrador Teste",
                Login = "admin",
                Perfil = "ADMIN",
                Ativo = true
            };
            
            Form1 formPrincipal = new Form1();
            Application.Run(formPrincipal);
        }
    }
}
