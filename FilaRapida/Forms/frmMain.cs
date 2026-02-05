using System;
using System.Windows.Forms;
using FilaRapida.Models;

namespace FilaRapida.Forms
{
    public partial class frmMain : Form
    {
        private Usuario usuarioLogado;
        
        public frmMain(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            ConfigurarInterface();
        }
        
        private void ConfigurarInterface()
        {
            lblUsuario.Text = $"UsuÃ¡rio: {usuarioLogado.Nome}";
            lblPerfil.Text = $"Perfil: {usuarioLogado.Perfil}";
            
            if (usuarioLogado.Perfil == "ADMIN")
            {
                menuUsuarios.Enabled = true;
                menuProdutos.Enabled = true;
                menuPedidos.Enabled = true;
                menuRelatorios.Enabled = true;
            }
            else if (usuarioLogado.Perfil == "ATENDENTE")
            {
                menuUsuarios.Enabled = false;
                menuProdutos.Enabled = false;
                menuPedidos.Enabled = true;
                menuRelatorios.Enabled = false;
            }
        }
        
        private void menuSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void menuLogout_Click(object sender, EventArgs e)
        {
            var loginForm = new frmLogin();
            loginForm.Show();
            this.Close();
        }
        
        private void menuProdutos_Click(object sender, EventArgs e)
        {
            var formProdutos = new frmProdutos(usuarioLogado);
            formProdutos.ShowDialog();
        }
        
        private void menuPedidos_Click(object sender, EventArgs e)
        {
            var formPedido = new frmPedido(usuarioLogado);
            formPedido.ShowDialog();
        }
        
        private void menuRelatorios_Click(object sender, EventArgs e)
        {
            var formRelatorios = new frmRelatorios(usuarioLogado);
            formRelatorios.ShowDialog();
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = $"Fila RÃ¡pida - {usuarioLogado.Nome}";
        }
    }
}
