using System;
using System.Windows.Forms;
using FilaRapida.Forms;
namespace FilaRapida
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CriarMenu();
            this.Text = "Fila Rápida - Sistema de Gerenciamento";
            this.WindowState = FormWindowState.Maximized; // Abrir maximizado
        }

        private void CriarMenu()
        {
            // Cria MenuStrip (barra de menus)
            MenuStrip menuStrip = new MenuStrip();
            
            // Menu Arquivo
            ToolStripMenuItem arquivoMenu = new ToolStripMenuItem("&Arquivo");
            
            ToolStripMenuItem sairItem = new ToolStripMenuItem("&Sair");
            sairItem.Click += (s, e) => Application.Exit();
            arquivoMenu.DropDownItems.Add(sairItem);
            
            // Menu Cadastros
            ToolStripMenuItem cadastrosMenu = new ToolStripMenuItem("&Cadastros");
            
            ToolStripMenuItem produtosItem = new ToolStripMenuItem("&Produtos");
            produtosItem.Click += (s, e) => AbrirFormProdutos();
            
            ToolStripMenuItem usuariosItem = new ToolStripMenuItem("&Usuários");
            usuariosItem.Click += (s, e) => AbrirFormUsuarios();
            
            cadastrosMenu.DropDownItems.AddRange(new[] { produtosItem, usuariosItem });
            
            // Menu Operações
            ToolStripMenuItem operacoesMenu = new ToolStripMenuItem("&Operações");
            
            ToolStripMenuItem pedidosItem = new ToolStripMenuItem("&Pedidos");
            pedidosItem.Click += (s, e) => AbrirFormPedidos();
            
            ToolStripMenuItem relatoriosItem = new ToolStripMenuItem("&Relatórios");
            relatoriosItem.Click += (s, e) => AbrirFormRelatorios();
            
            operacoesMenu.DropDownItems.AddRange(new[] { pedidosItem, relatoriosItem });
            
            // Adiciona todos os menus
            menuStrip.Items.AddRange(new[] { arquivoMenu, cadastrosMenu, operacoesMenu });
            
            // Área principal (Panel)
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = System.Drawing.Color.LightGray;
            
            // Label de boas-vindas
            Label lblBoasVindas = new Label();
            lblBoasVindas.Text = "Bem-vindo ao Sistema Fila Rápida!\n\nSelecione uma opção no menu acima.";
            lblBoasVindas.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            lblBoasVindas.AutoSize = true;
            lblBoasVindas.Location = new System.Drawing.Point(50, 50);
            
            panelPrincipal.Controls.Add(lblBoasVindas);
            
            // Adiciona controles ao form
            this.Controls.AddRange(new Control[] { panelPrincipal, menuStrip });
            this.MainMenuStrip = menuStrip;
        }

        // Métodos para abrir forms (serão implementados)
      private void AbrirFormProdutos()
{
   
    // Use o form de teste
    var formTeste = new FilaRapida.Forms.frmTesteProdutos();
    formTeste.Show();
}

        
        private void AbrirFormUsuarios()
        {
            MessageBox.Show("Abrindo cadastro de usuários...", "Usuários");
        }
        
        private void AbrirFormPedidos()
        {
            MessageBox.Show("Abrindo gestão de pedidos...", "Pedidos");
        }
        
        private void AbrirFormRelatorios()
        {
            MessageBox.Show("Abrindo relatórios...", "Relatórios");
        }
    }
}