using System;
using System.Windows.Forms;
using FilaRapida.Repositories;
using FilaRapida.Models;

namespace FilaRapida.Forms
{
    public partial class frmLogin : Form
    {
        private UsuarioRepository usuarioRepo;
        private int tentativasRestantes = 3;
        
        public frmLogin()
        {
            InitializeComponent();
            usuarioRepo = new UsuarioRepository();
            
            txtSenha.UseSystemPasswordChar = true;
            lblTentativas.Text = $"Tentativas restantes: {tentativasRestantes}";
            lblTentativas.Visible = false;
        }
        
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Por favor, informe o login.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }
            
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, informe a senha.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return;
            }
            
            try
            {
                // ✅✅✅ CORREÇÃO AQUI - Authenticate em vez de Autenticar
                var usuario = usuarioRepo.Authenticate(txtLogin.Text, txtSenha.Text);
            
                if (usuario != null)
                {
                    MessageBox.Show($"Bem-vindo, {usuario.Nome}!", "Login realizado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    var mainForm = new frmMain(usuario);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    tentativasRestantes--;
                    lblTentativas.Text = $"Tentativas restantes: {tentativasRestantes}";
                    lblTentativas.Visible = true;
                    
                    if (tentativasRestantes <= 0)
                    {
                        MessageBox.Show("Número máximo de tentativas excedido. Tente novamente mais tarde.", 
                            "Acesso bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("Login ou senha incorretos.", "Erro de autenticação", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtSenha.Clear();
                        txtSenha.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }
        
        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtLogin.Focus();
        }
    }
}